using FluentValidation;
using System;
using Traibanhoa.Modules.TypeModule.Request;

namespace Traibanhoa.Modules.BasketModule.Request
{
    public class CreateBasketRequest
    {
        public Guid BasketId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? View { get; set; }
        public decimal? BasketPrice { get; set; }
        public int? Status { get; set; }
    }
    public class CreateBasketRequestValidator : AbstractValidator<CreateBasketRequest>
    {
        public CreateBasketRequestValidator()
        {
            RuleFor(x => x.Title).NotEmpty().NotNull();
            RuleFor(x => x.Description).NotEmpty().NotNull();
            RuleFor(x => x.ImageUrl).NotEmpty().NotNull();
            RuleFor(x => x.View).NotEmpty().NotNull();
            RuleFor(x => x.BasketPrice).NotEmpty().NotNull();
            RuleFor(x => x.Status).NotEmpty().NotNull();
            RuleFor(x => x.CreatedDate).NotEmpty().NotNull();
            RuleFor(x => x.UpdatedDate).NotEmpty().NotNull();
        }
    }
}
