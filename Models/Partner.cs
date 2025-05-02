using System.ComponentModel.DataAnnotations;

namespace InsurancePartnerManagement.Models;

public class Partner
{
    public int Id { get; set; }

    [Required]
    [StringLength(255, MinimumLength = 2)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(255, MinimumLength = 2)]
    public string LastName { get; set; }

    [StringLength(500)]
    public string Address { get; set; }

    [Required]
    [RegularExpression(@"^\d{20}$", ErrorMessage = "PartnerNumber mora imati točno 20 znamenki.")]
    public string PartnerNumber { get; set; }

    public string? CroatianPIN { get; set; }

    [Required]
    [Range(1, 2, ErrorMessage = "PartnerTypeId mora biti 1 (Personal) ili 2 (Legal).")]
    public int PartnerTypeId { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string CreateByUser { get; set; }

    [Required]
    public bool IsForeign { get; set; }

    [Required]
    [StringLength(20, MinimumLength = 10)]
    public string ExternalCode { get; set; }

    [Required]
    [RegularExpression(@"^[MFN]$", ErrorMessage = "Gender mora biti M, F ili N.")]
    public char Gender { get; set; }

    // Dodatna svojstva za broj polica i ukupni iznos polica
    public int TotalPolicies { get; set; }
    public decimal TotalPolicyAmount { get; set; }
}