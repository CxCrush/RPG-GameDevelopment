using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public class PlayerSkill
{
    public string name;

    protected int id;

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    protected int occupation; //职业

    public int Occupation
    {
        get { return occupation; }
        set { occupation = value; }
    }

    protected int mpCost;  //耗蓝量

    public int MpCost
    {
        get { return mpCost; }
        set { mpCost = value; }
    }

    protected int level;  //等级

    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    protected int neededLevel;  //能够学习的等级

    public int NeededLevel
    {
        get { return neededLevel; }
        set { neededLevel = value; }
    }

    protected int damage; //伤害值

    public int Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    protected float damageRadius;  //伤害半径

    public float DamageRadius
    {
        get { return damageRadius; }
        set { damageRadius = value; }
    }

    protected float cdTime;  //技能冷却时间

    public float CdTime
    {
        get { return cdTime; }
        set { cdTime = value; }
    }

    protected KeyCode shortCutKey;  //快捷键

    public KeyCode ShortCutKey
    {
        get { return shortCutKey; }
        set { shortCutKey = value; }
    }

    int hasLearned;  //是否学会,0表示没学会,1表示已经学会
    public int HasLearned
    {
        get { return hasLearned; }
        set { hasLearned = value; }
    }

    public PlayerSkill(int _id)
    {
        name = "";
        id =_id;
        occupation =0;
        mpCost = 50;
        level = 1;
        neededLevel = 1;
        damage = 0;
        cdTime = 0;
        shortCutKey = (KeyCode)(id+KeyCode.Alpha0);
        hasLearned = 0;
    }

   

    public virtual void  SetData(XmlElement xml)
    {
        occupation = XmlTools.GetIntAttribute(xml, "occupation");
        mpCost = XmlTools.GetIntAttribute(xml, "mpCost");
        level = XmlTools.GetIntAttribute(xml, "level");
        neededLevel = XmlTools.GetIntAttribute(xml, "neededLevel");
        damage = XmlTools.GetIntAttribute(xml, "damage");
        hasLearned = XmlTools.GetIntAttribute(xml, "hasLearned");
    }

    public virtual void Skill()
    {

    }

}
