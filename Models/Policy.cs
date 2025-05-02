using System.ComponentModel.DataAnnotations;

namespace InsurancePartnerManagement.Models;

public class Policy
{
    public int Id { get; set; }

    [Required]
    [StringLength(15, MinimumLength = 10, ErrorMessage = "Broj police mora imati između 10 i 15 znakova.")]
    public string? PolicyNumber { get; set; } = string.Empty;

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Iznos police mora biti veći od 0.")]
    public decimal PolicyAmount { get; set; }

    [Required]
    public int PartnerId { get; set; } // Veza s partnerom
}