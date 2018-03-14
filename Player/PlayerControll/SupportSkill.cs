using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SupportSkill : PlayerSkill
{
    
    public SupportSkill(int _id, Player player)
        : base(_id)
    {
        mpCost = 40 * id;
        damage = player.Atk + player.Intelligence + player.Agility + player.MagicPower + player.Strength + mpCost;
        
    }

    public override void Skill()
    {

    }
}
