using SelfCheckoutServiceMachine.Models;
using SelfCheckoutServiceMachine.Service;

namespace SelfCheckoutServiceMachine.Controller;

public class DiscountCardController
{
    private DiscountCardService _discountCardService;

    public DiscountCardController()
    {
        _discountCardService = new DiscountCardService();
    }

    public DiscountCard searchDiscountCardByNumber(string number)
    {
        return _discountCardService.searchDiscountCardByNumber(number);
    }
    
}