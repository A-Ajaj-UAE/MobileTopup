using FluentValidation;
using MobileTopup.Contracts.Exceptions;
using MobileTopup.Contracts.Domain.Entities;

namespace MobileTopup.Contracts.Validatiors
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Beneficiaries)
                .Must(beneficiaries => beneficiaries?.Count(b => b.IsActive) <= 5)
                .WithMessage(BeneficiaryExceptions.MaxActiveBenfenciryExceeded);
        }
    }
}
