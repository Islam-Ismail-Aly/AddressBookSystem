using System.ComponentModel.DataAnnotations;

namespace AddressBook.Core.Entities
{
    public class EmployeeAddressBook : BaseEntity
    {

        [Required]
        public string FullName { get; set; }

        [Required]
        public int JobTitleId { get; set; }
        public virtual Job JobTitle { get; set; }

        [Required]
        public int DepartmentId { get; set; }
        public virtual Department Department { get; set; }

        [Required]
        [Phone]
        public string MobileNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Photo { get; set; }

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Year;
                if (DateOfBirth.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
