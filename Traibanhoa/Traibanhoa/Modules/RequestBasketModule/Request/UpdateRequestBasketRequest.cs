using FluentValidation;
using System;

namespace Traibanhoa.Modules.RequestBasketModule.Request
{
    public class UpdateRequestBasketRequest
    {
        public Guid RequestBasketId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? RequestStatus { get; set; }
        public Guid? CreateBy { get; set; }
        public Guid? ConfirmBy { get; set; }
    }
    public class UpdateRequestBasketRequestValidator : AbstractValidator<UpdateRequestBasketRequest>
    {
        public UpdateRequestBasketRequestValidator()
        {
            RuleFor(x => x.RequestBasketId).NotEmpty().NotNull();
            RuleFor(x => x.Title).NotEmpty().NotNull();
            RuleFor(x => x.ImageUrl).NotEmpty().NotNull();
            RuleFor(x => x.CreatedDate).NotEmpty().NotNull();
            RuleFor(x => x.RequestStatus).NotEmpty().NotNull();
            RuleFor(x => x.CreateBy).NotEmpty().NotNull();
            RuleFor(x => x.ConfirmBy).NotEmpty().NotNull();
        }
    }
}
