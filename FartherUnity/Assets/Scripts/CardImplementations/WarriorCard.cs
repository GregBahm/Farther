using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class WarriorCard : Card
{
    public int Level { get; }

    public WarriorCard(int level) 
        : base(CardType.Warrior)
    {
        Level = level;
    }
}