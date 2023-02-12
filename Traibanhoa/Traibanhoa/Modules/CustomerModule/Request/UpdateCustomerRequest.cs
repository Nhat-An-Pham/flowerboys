using FluentValidation;
using System;

namespace Traibanhoa.Modules.CustomerModule.Request
{
    public class UpdateCustomerRequest
    {
        public Guid CustomerId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phonenumber { get; set; }
        public bool? Gender { get; set; }
        public string Avatar { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsGoogle { get; set; }
        public bool? IsBlocked { get; set; }
    }
    public class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
    {
        public UpdateCustomerRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty().NotNull();
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Email).NotEmpty().NotNull();
            RuleFor(x => x.Password).NotEmpty().NotNull();
            RuleFor(x => x.Phonenumber).NotEmpty().NotNull();
            RuleFor(x => x.Gender).NotEmpty().NotNull();
            RuleFor(x => x.Avatar).NotEmpty().NotNull();
            RuleFor(x => x.CreatedDate).NotEmpty().NotNull();
            RuleFor(x => x.UpdatedDate).NotEmpty().NotNull();
            RuleFor(x => x.IsGoogle).NotEmpty().NotNull();
            RuleFor(x => x.IsBlocked).NotEmpty().NotNull();
        }
    }
}
