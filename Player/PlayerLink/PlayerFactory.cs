using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class PlayerFactory
{
    public static Player GeneratePlayer(OccupationType id,string name)
    {
        Player player = null;

        switch (id)
        {
            case OccupationType.Warrior:
                player = new Warrior(name);
                break;
            case OccupationType.Support:
                player = new Support(name);
                break;
            case OccupationType.Mage:
                player = new Mage(name);
                break;
            case OccupationType.Assassin:
                player = new Assassin(name);
                break;
            default:
                break;
        }

        return player;
    }
}
