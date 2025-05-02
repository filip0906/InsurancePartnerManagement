using Microsoft.AspNetCore.Mvc;
using InsurancePartnerManagement.Models;
using InsurancePartnerManagement.Repositories;

namespace InsurancePartnerManagement.Controllers;

public class PolicyController : Controller
{
    private readonly PolicyRepository _repository;

    public PolicyController(PolicyRepository repository)
    {
        _repository = repository;
    }

    public IActionResult Create(int partnerId)
    {
        ViewBag.PartnerId = partnerId;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Policy policy)
    {
        if (!ModelState.IsValid)
        {
            return View(policy);
        }

        await _repository.AddPolicyAsync(policy);
        TempData["SuccessMessage"] = "Polica uspješno dodana!";
        return RedirectToAction("Index", "Partner");
    }
}