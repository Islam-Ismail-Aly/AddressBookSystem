using AddressBook.Application.DTOs.Authentication;
using FluentValidation;

namespace AddressBook.Application.Validations.Authentication
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First Name is required.")
                .MaximumLength(30).WithMessage("First Name must be at most 30 characters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .MaximumLength(30).WithMessage("Last Name must be at most 30 characters.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("User Name is required.")
                .MaximumLength(30).WithMessage("User Name must be at most 30 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(30).WithMessage("Email must be at least 20 characters.")
                .EmailAddress().WithMessage("Invalid email address.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MaximumLength(8).WithMessage("Password must be at most 8 characters.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Confirm Password is required.")
                .MaximumLength(8).WithMessage("Confirm Password must be at most 8 characters.")
                .Equal(x => x.Password).WithMessage("Passwords do not match.");
        }
    }
}
