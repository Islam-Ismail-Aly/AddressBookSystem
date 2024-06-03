namespace AddressBook.Application.DTOs.AddressBooks
{
    public class EmployeeAddressBookDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string Department { get; set; }
        public string MobileNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public int Age { get; set; }
        public DateTime CreatedOn { get; set; }
    }

    //public class JobDto
    //{
    //    public int Id { get; set; }
    //    public string Title { get; set; }
    //}

    //public class DepartmentDto
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    
}
