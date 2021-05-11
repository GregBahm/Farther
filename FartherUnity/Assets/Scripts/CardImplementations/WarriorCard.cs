using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class WarriorCard : Card
{
    public int Level { get; }

    public int TurnsWounded { get; }

    public WarriorCard(int level = 0, int turnsWounded = 0) 
        : base(CardType.Warrior)
    {
        Level = level;
        TurnsWounded = turnsWounded;
    }
}