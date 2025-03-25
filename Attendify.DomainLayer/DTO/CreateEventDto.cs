using System.ComponentModel.DataAnnotations;

namespace Attendify.DomainLayer.DTO
{
    public class CreateEventDto
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Title must be between 2 and 100 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-\.\,\!\?]+$", ErrorMessage = "Title can only contain letters, numbers, spaces, and basic punctuation (.-,!?).")]
        public string Title { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-\.\,\!\?\'\""\(\)]+$", ErrorMessage = "Description can only contain letters, numbers, spaces, and basic punctuation (.-,!?'\"()).")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date and time are required.")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date and time format.")]
        [FutureDate(ErrorMessage = "Date and time must be in the future.")]
        public DateTime DateTime { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Location must be between 2 and 200 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-\.\,\#\/]+$", ErrorMessage = "Location can only contain letters, numbers, spaces, and basic punctuation (.-,#/).")]
        public string Location { get; set; } = string.Empty;

        [Required(ErrorMessage = "Created by is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Created by must be between 2 and 50 characters.")]
        [RegularExpression(@"^[a-zA-Z\s\-\']+$", ErrorMessage = "Created by can only contain letters, spaces, hyphens, and apostrophes.")]
        public string CreatedBy { get; set; } = string.Empty;
    }
    public class FutureDateAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is DateTime dateTime)
            {
                return dateTime > DateTime.Now;
            }
            return false;
        }
    }
}
