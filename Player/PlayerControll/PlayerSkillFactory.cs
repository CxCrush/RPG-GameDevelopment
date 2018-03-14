using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public static class PlayerSkillFactory
{
    public static PlayerSkill GetPlayerSkill(int id,Player player)
    {
        PlayerSkill skill = null;

        switch ((OccupationType)player.occupation)
        {
            case OccupationType.Warrior: skill = new WarriorSkill(id,player);
                break;
            case OccupationType.Support: skill = new SupportSkill(id, player);
                break;
            case OccupationType.Mage: skill = new MageSkill(id, player);
                break;
            case OccupationType.Assassin: skill = new AssassinSkill(id, player);
                break;
            default:
                break;
        }

        return skill;
    }
}

