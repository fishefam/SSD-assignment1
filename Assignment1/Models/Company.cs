namespace Assignment1.Models
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Company Name")]
        public string Name { get; set; } = null!;

        [Required]
        [Range(0, 200, ErrorMessage = "Years in business must be between 0 and 200.")]
        [Display(Name = "Years in Business")]
        public int YearsInBusiness { get; set; }

        [Required]
        [Url(ErrorMessage = "Please enter a valid website URL.")]
        [Display(Name = "Website")]
        public string Website { get; set; } = null!;

        [StringLength(50)]
        [Display(Name = "Province")]
        public string? Province { get; set; }
    }
}
