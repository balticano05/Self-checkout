using System.IO;
using SelfCheckoutServiceMachine.Mapper;
using SelfCheckoutServiceMachine.Models;
using SelfCheckoutServiceMachine.Utils;

namespace SelfCheckoutServiceMachine.Repository;

public class DiscountCardRepository
{
    private List<DiscountCard> _discountCards;
    private readonly DiscountCardMapper _discountCardMapper;
    private readonly string _filePath;

    public DiscountCardRepository()
    {
        _discountCardMapper = new DiscountCardMapper();
        _filePath = Path.Combine(
            Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName,
            "Resources",
            "DiscountCard.csv"
        );
        _discountCards = _discountCardMapper
            .MapStringListToDiscountCards(CsvFileReader.ReadFile(_filePath));
    }

    public void Add(DiscountCard discountCard)
    {
        _discountCards.Add(discountCard);
    }

    public DiscountCard Get(int id)
    {
        return _discountCards.FirstOrDefault(card => card.Id.Equals(id));
    }

    public List<DiscountCard> GetAll()
    {
        return _discountCards;
    }

    public void Update(DiscountCard updatedDiscountCard)
    {
        var cardIndex = _discountCards
            .FindIndex(card => card.Id.Equals(updatedDiscountCard.Id));
        
        if (cardIndex != -1)
        {
            _discountCards[cardIndex] = updatedDiscountCard;
        }
    }

    public void Save()
    {
        CsvFileWriter.WriteFile(
            _filePath, 
            _discountCardMapper.MapDiscountCardsToStringList(_discountCards)
        );
    }
}