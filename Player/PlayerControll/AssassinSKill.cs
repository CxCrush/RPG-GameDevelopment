using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class AssassinSkill : PlayerSkill
{
    public AssassinSkill(int _id, Player player)
        : base(_id)
    {
        mpCost = 30 * id;
        damage = player.Atk + player.Intelligence + player.Agility + player.MagicPower + player.Strength + mpCost;
    }

    public override void Skill()
    {

    }
}
