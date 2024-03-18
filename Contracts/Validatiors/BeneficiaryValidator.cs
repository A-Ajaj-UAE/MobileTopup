using FluentValidation;
using MobileTopup.Contracts.Exceptions;
using MobileTopup.Contracts.Requests;

namespace MobileTopup.Contracts.Validatiors
{
    public class BeneficiaryValidator : AbstractValidator<AddBeneficiaryRequest>
    {
        public BeneficiaryValidator()
        {
            RuleFor(b => b.NickName)
                .MaximumLength(20).WithMessage(BeneficiaryExceptions.BenefenciryNickNameLenghtExceeded);
        }
    }
}
