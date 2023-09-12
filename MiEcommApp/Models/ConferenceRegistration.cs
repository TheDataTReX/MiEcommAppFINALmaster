using System;
using System.ComponentModel.DataAnnotations;

namespace MiEcommApp.Models
{
    public class ConferenceRegistration
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [CustomAgeValidation(18)]
        public DateTime BirthDate { get; set; }

        [DataType(DataType.Url)]
        public string Website { get; set; }

        [MaxLength(500)]
        public string Bio { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        public string Affiliation { get; set; }

        [Required]
        public string PaperTitle { get; set; }
    }

    public class CustomAgeValidationAttribute : ValidationAttribute
    {
        private readonly int _minimumAge;

        public CustomAgeValidationAttribute(int minimumAge)
        {
            _minimumAge = minimumAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime birthDate)
            {
                if (birthDate.AddYears(_minimumAge) <= DateTime.Today)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult($"You must be at least {_minimumAge} years old to register.");
                }
            }
            return new ValidationResult("Invalid birthdate");
        }
    }
}

