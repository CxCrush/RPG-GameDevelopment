using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public class Assassin:Player
{

    public Assassin(string name)
    {
        occupation = (int)OccupationType.Assassin;
        this.name = name;
        level = 1;

        //加载配置文件
        TextAsset xml = ResourceManager.Instance.Load<TextAsset>("Configs/Player");
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml.text);

        XmlNode root = xmlDoc.SelectSingleNode("root");
        XmlElement assassin = root.SelectSingleNode("assassin") as XmlElement;

        SetData(assassin);
    }
   
    public Assassin()
    {

    }

    public override Player Clone()
    {
        Assassin newPlayer = new Assassin();

        newPlayer.name = this.name;
        newPlayer.occupation = this.occupation;
        newPlayer.server = this.server;
        newPlayer.sex = this.sex;
        newPlayer.level = this.level;

        newPlayer.hp = this.hp;
        newPlayer.maxHp = this.maxHp;
        newPlayer.mp = this.mp;
        newPlayer.maxMp = this.maxMp;

        newPlayer.atk = this.atk;
        newPlayer.def = this.def;
        newPlayer.strength = this.strength;
        newPlayer.intelligence = this.intelligence;
        newPlayer.magicPower = this.magicPower;

        return newPlayer;
    }
}
