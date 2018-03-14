using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;


public class Player
{
    static int basicExp = 1000;

    public Vector3 pos;  //玩家位置

    public string name;
    public int level;
    public int server;
    public int occupation;
    public int sex;

    protected int hp;

    public int Hp
    {
        get { return hp; }
        set { hp = value; }
    }

    protected int maxHp;

    public int MaxHp
    {
        get { return maxHp; }
        set { maxHp = value; }
    }

    protected int mp;

    public int Mp
    {
        get { return mp; }
        set { mp = value; }
    }
    protected int maxMp;

    public int MaxMp
    {
        get { return maxMp; }
        set { maxMp = value; }
    }

    protected int atk;

    public int Atk
    {
        get { return atk; }
        set { atk = value; }
    }
    protected int def;

    public int Def
    {
        get { return def; }
        set { def = value; }
    }
    protected int magicPower;

    public int MagicPower
    {
        get { return magicPower; }
        set { magicPower = value; }
    }
    protected int strength;

    public int Strength
    {
        get { return strength; }
        set { strength = value; }
    }
    protected int intelligence;

    public int Intelligence
    {
        get { return intelligence; }
        set { intelligence = value; }
    }

    protected int agility;

    public int Agility
    {
        get { return agility; }
        set { agility = value; }
    }

    //当前经验值
    protected int exp;

    public int Exp
    {
        get { return exp; }
        set { exp = value; }
    }

    //最大经验值
    protected int maxExp;
    public int MaxExp
    {
        get { return maxExp; }
        set { maxExp = value; }
    }

    //拥有金钱
    protected int money;

    public int Money
    {
        get { return money; }
        set { money = value; }
    }

    public virtual  void SetData(XmlElement xml)
    {
        level = XmlTools.GetIntAttribute(xml, "level");
        exp = XmlTools.GetIntAttribute(xml, "exp");
        this.maxExp = basicExp * level;
        money = XmlTools.GetIntAttribute(xml, "money");
        server = XmlTools.GetIntAttribute(xml, "server");
        sex = XmlTools.GetIntAttribute(xml, "sex");

        hp = XmlTools.GetIntAttribute(xml, "hp");
        maxHp = XmlTools.GetIntAttribute(xml, "maxHp");
        mp = XmlTools.GetIntAttribute(xml, "mp");
        maxMp = XmlTools.GetIntAttribute(xml, "maxMp");

        atk = XmlTools.GetIntAttribute(xml, "atk");
        def = XmlTools.GetIntAttribute(xml, "def");
        strength = XmlTools.GetIntAttribute(xml, "strength");
        intelligence = XmlTools.GetIntAttribute(xml, "intelligence");
        magicPower = XmlTools.GetIntAttribute(xml, "magicPower");
        agility = XmlTools.GetIntAttribute(xml, "agility");
    }
    public Player()
    {
        pos = Vector3.zero;
        this.name ="";
        this.occupation =0;
        this.server =0;
        this.sex = 0;
        this.level =1;
        this.exp = 0;
       

        this.hp = 0;
        this.maxHp = 0;
        this.mp = 0;
        this.maxMp = 0;

        this.atk = 0;
        this.def =0;
        this.strength = 0;
        this.intelligence = 0;
        this.magicPower = 0;
        this.agility = 0;
    }

    public virtual Player Clone()
    {
        return null;
    }
}
