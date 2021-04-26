using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class Card
{
    public CardType Type { get; }

    public Card(CardType type)
    {
        Type = type;
    }
}