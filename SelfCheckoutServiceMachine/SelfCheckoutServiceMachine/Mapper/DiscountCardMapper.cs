using System.Globalization;
using SelfCheckoutServiceMachine.Models;

namespace SelfCheckoutServiceMachine.Mapper;

public class DiscountCardMapper : CsvMapper<DiscountCard>
{
    public List<DiscountCard> MapStringListToDiscountCards(List<string> lineDiscountCards)
    {
        return MapStringListToObjects(lineDiscountCards, parts =>
        {
            if (parts.Length != 3)
            {
                throw new FormatException("Invalid number of parts in line.");
            }

            return new DiscountCard.Builder()
                .SetId(int.Parse(parts[0]))
                .SetNumber(parts[1])
                .SetDiscount(decimal.Parse(parts[2], CultureInfo.InvariantCulture))
                .Build();
        });
    }

    public List<string> MapDiscountCardsToStringList(List<DiscountCard> discountCards)
    {
        var header = "id;number;discount_amount";
        var lines = new List<string> { header };

        lines.AddRange(MapObjectsToStringList(discountCards, discountCard =>
            $"{discountCard.Id};{discountCard.Number};" +
            $"{discountCard.Discount.ToString(CultureInfo.InvariantCulture)}"
        ));

        return lines;
    }
}