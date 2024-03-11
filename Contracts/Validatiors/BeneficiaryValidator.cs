using FluentValidation;
using MobileTopup.Contracts.Exceptions;
using MobileTopup.Contracts.Models;

namespace MobileTopup.Contracts.Validatiors
{
    public class BeneficiaryValidator : AbstractValidator<Beneficiary>
    {
        public BeneficiaryValidator()
        {
            RuleFor(b => b.NickName)
                .MaximumLength(20).WithMessage(BeneficiaryExceptions.BenefenciryNickNameLenghtExceeded);
        }
    }
}
