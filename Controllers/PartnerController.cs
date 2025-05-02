using Microsoft.AspNetCore.Mvc;
using InsurancePartnerManagement.Models;
using InsurancePartnerManagement.Repositories;
using Microsoft.Extensions.Logging;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;
using System.Diagnostics;

namespace InsurancePartnerManagement.Controllers
{
    public class PartnerController : Controller
    {
        private readonly PartnerRepository _repository;
        private readonly ILogger<PartnerController> _logger;
        private readonly string _connectionString;

        public PartnerController(PartnerRepository repository, ILogger<PartnerController> logger, IConfiguration configuration)
        {
            _repository = repository;
            _logger = logger;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var partners = await _repository.GetAllPartnersWithPoliciesAsync();
                return View(partners);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška prilikom dohvaćanja partnera.");
                var errorModel = new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Message = "Došlo je do greške prilikom dohvaćanja partnera."
                };
                return View("Error", errorModel);
            }
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Partner partner)
        {
            if (!ModelState.IsValid)
            {
                return View(partner);
            }

            try
            {
                await _repository.AddPartnerAsync(partner);
                TempData["SuccessMessage"] = "Partner uspješno dodan!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Greška prilikom dodavanja partnera.");
                ModelState.AddModelError("", "Došlo je do greške prilikom spremanja partnera.");
                return View(partner);
            }
        }

        public async Task<IEnumerable<Partner>> GetAllPartnersAsync()
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                return await connection.QueryAsync<Partner>("SELECT * FROM Partners ORDER BY CreatedAtUtc DESC");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Greška prilikom dohvaćanja partnera: {ex.Message}");
                throw;
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var partner = await _repository.GetPartnerByIdAsync(id);
            if (partner == null)
            {
                return NotFound();
            }
            return PartialView("_PartnerDetails", partner);
        }
    }
}