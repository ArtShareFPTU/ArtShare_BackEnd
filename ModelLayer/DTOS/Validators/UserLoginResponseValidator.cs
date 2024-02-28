using FluentValidation;
using ModelLayer.DTOS.Request.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.DTOS.Validators
{
    public class UserLoginResponseValidator : AbstractValidator<LoginAccountResponse>
    {
        public UserLoginResponseValidator() {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required")
                /*.MinimumLength(6).WithMessage("Password is at least 6 characters")*/;
        }
    }
}
