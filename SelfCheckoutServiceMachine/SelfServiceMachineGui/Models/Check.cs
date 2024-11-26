namespace SelfCheckoutServiceMachine.Models;

public class Check
{
    public DateTime CreatedAt { get; private set; }
    public List<Product> Products { get; private set; }
    public DiscountCard DiscountCard { get; private set; }

    public class Builder
    {
        private readonly Check _check = new Check();

        public Builder SetCreatedAt(DateTime createdAt)
        {
            _check.CreatedAt = createdAt;
            return this;
        }

        public Builder SetProducts(List<Product> products)
        {
            _check.Products = products ?? new List<Product>();
            return this;
        }

        public Builder SetDiscountCard(DiscountCard discountCard)
        {
            _check.DiscountCard = discountCard;
            return this;
        }

        public Check Build()
        {
            return _check;
        }
    }
}