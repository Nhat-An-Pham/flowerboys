using FluentValidation;
using System;
using Traibanhoa.Modules.TypeModule.Request;

namespace Traibanhoa.Modules.UserModule.Request
{
    public class CreateCustomerRequest
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phonenumber { get; set; }
        public bool? Gender { get; set; }
        public string Avatar { get; set; }
        public int? Role { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool? IsGoogle { get; set; }
        public bool? IsBlocked { get; set; }
    }
    public class CreateUserRequestValidator : AbstractValidator<CreateCustomerRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.Username).NotEmpty().NotNull();
            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Email).NotEmpty().NotNull();
            RuleFor(x => x.Password).NotEmpty().NotNull();
            RuleFor(x => x.Phonenumber).NotEmpty().NotNull();
            RuleFor(x => x.Gender).NotEmpty().NotNull();
            RuleFor(x => x.Avatar).NotEmpty().NotNull();
            RuleFor(x => x.Role).NotEmpty().NotNull();
            RuleFor(x => x.CreatedDate).NotEmpty().NotNull();
            RuleFor(x => x.UpdatedDate).NotEmpty().NotNull();
            RuleFor(x => x.IsGoogle).NotEmpty().NotNull();
            RuleFor(x => x.IsBlocked).NotEmpty().NotNull();
        }
    }
}
