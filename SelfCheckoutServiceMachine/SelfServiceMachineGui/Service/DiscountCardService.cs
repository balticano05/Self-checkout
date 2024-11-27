using SelfCheckoutServiceMachine.Models;
using SelfCheckoutServiceMachine.Repository;

namespace SelfCheckoutServiceMachine.Service;

public class DiscountCardService
{
    private readonly DiscountCardRepository _discountCardRepository;

    public DiscountCardService()
    {
        _discountCardRepository = new DiscountCardRepository();
    }

    public DiscountCard SearchDiscountCardByNumber(string number)
    {
        return _discountCardRepository.GetAll()
            .FirstOrDefault(card => card.Number.Equals(number));
    }

    public DiscountCard CreateNewDiscountCard(string cardNumber)
    {
        return new DiscountCard.Builder()
            .SetDiscount(1m)
            .SetNumber(cardNumber)
            .Build();
    }
}