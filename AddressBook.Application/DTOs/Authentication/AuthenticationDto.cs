namespace AddressBook.Application.DTOs.Authentication
{
    public class AuthenticationDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public IEnumerable<string> Roles { get; set; } = new HashSet<string>();
        public string Token { get; set; } = string.Empty;
        public bool IsAuthenticated { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
