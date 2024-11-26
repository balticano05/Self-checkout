using SelfCheckoutServiceMachine.Models;
using SelfCheckoutServiceMachine.Repository;

namespace SelfCheckoutServiceMachine.Service;

public class DiscountCardService
{
    private DiscountCardRepository _discountCardRepository;

    public DiscountCardService()
    {
        _discountCardRepository = new DiscountCardRepository();
    }

    public DiscountCard SearchDiscountCardByNumber(string number)
    {
        for (int i = 0; i < _discountCardRepository.GetAll().Count; i++)
        {
            if (_discountCardRepository.GetAll()[i].Number.Equals(number))
            {
                return _discountCardRepository.Get(i+1);
            }
        }

        return null;
    }

    public DiscountCard CreateNewDiscountCard(string cardNumber)
    {
        return new DiscountCard.Builder()
            .SetDiscount(1m)
            .SetNumber(cardNumber)
            .Build();
    }
    
}