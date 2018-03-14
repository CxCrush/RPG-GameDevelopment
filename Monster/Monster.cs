using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;


public enum MonsterID
{
    normal,boss
}
public class Monster
{
    public string name;
    public int level;
    public int id;

    public int hp;
    public int maxHp;
    public int mp;
    public int maxMp;

    public int atk;
    public int def;

    public Monster(XmlElement xml)
    {
        name = XmlTools.GetStringAttribute(xml, "name");
        level = XmlTools.GetIntAttribute(xml, "level");
        id = XmlTools.GetIntAttribute(xml, "id");
        hp = XmlTools.GetIntAttribute(xml, "hp");
        maxHp = XmlTools.GetIntAttribute(xml, "maxHp");
        mp = XmlTools.GetIntAttribute(xml, "mp");
        maxMp = XmlTools.GetIntAttribute(xml, "maxMp");
        atk = XmlTools.GetIntAttribute(xml, "atk");
        def = XmlTools.GetIntAttribute(xml, "def");
    }

    public Monster()
    {

    }
    public Monster Clone()
    {
        Monster monster = new Monster();

        monster.name = this.name;
        monster.level = this.level;
        monster.hp = this.hp;
        monster.maxHp = this.maxHp;
        monster.mp = this.mp;
        monster.maxMp = this.maxMp;
        monster.atk = this.atk;
        monster.def = this.def;

        return monster;
    }
}

