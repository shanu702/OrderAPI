using FluentValidation;
using Order.Entities;

namespace OrderAPI.Validators
{
    public class OrderRequestValidator : AbstractValidator<OrderRequest>
    {
        public OrderRequestValidator()
        {
            RuleFor(x => x.OrderId).NotNull().GreaterThan(0);
            RuleFor(x => x.ProductName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.Quantity).GreaterThan(0);
        }
    }
}
