using SelfCheckoutServiceMachine.Mapper;
using SelfCheckoutServiceMachine.Models;
using SelfCheckoutServiceMachine.Utils;

namespace SelfCheckoutServiceMachine.Repository;

public class DiscountCardRepository
{
    private List<DiscountCard> _discountCards;
    private DiscountCardMapper _discountCardMapper;
    private string filePath;

    public DiscountCardRepository()
    {
        _discountCardMapper = new DiscountCardMapper();
        filePath = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, 
            "Resources", 
            "DiscountCard.csv"
        );
        _discountCards = _discountCardMapper
            .MapStringListToDiscountCards(CsvFileReader
                .ReadFile(filePath));
    }

    public void Add(DiscountCard discountCard)    
    {
        _discountCards.Add(discountCard);
    }

    public DiscountCard Get(int id) 
    {
        foreach (var discountCard in _discountCards)
        {
            if (discountCard.Id.Equals(id))
            {
                return discountCard;
            }
        }
        return null;
    }

    public List<DiscountCard> GetAll() 
    {
        return _discountCards;
    }

    public void Update(DiscountCard updatedDiscountCard)
    {
        for (int i = 0; i < _discountCards.Count; i++)
        {
            if (_discountCards[i].Id.Equals(updatedDiscountCard.Id))
            {
                _discountCards[i] = updatedDiscountCard;
                break;
            }
        }
    }

    public void Save()
    {
        CsvFileWriter.WriteFile(filePath, _discountCardMapper.MapDiscountCardsToStringList(_discountCards));
    }
}