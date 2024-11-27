namespace SelfCheckoutServiceMachine.Models;

public class DiscountCard
{
    public int Id { get; private set; }
    public string Number { get; private set; }
    public decimal Discount { get; private set; }

    public DiscountCard()
    {
    }

    public class Builder
    {
        private readonly DiscountCard _discountCard = new DiscountCard();

        public Builder SetId(int id)
        {
            _discountCard.Id = id;
            return this;
        }

        public Builder SetNumber(string number)
        {
            _discountCard.Number = number;
            return this;
        }

        public Builder SetDiscount(decimal discount)
        {
            _discountCard.Discount = discount;
            return this;
        }

        public DiscountCard Build()
        {
            return _discountCard;
        }
    }
}