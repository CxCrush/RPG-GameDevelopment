using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine;

public class Support:Player
{
    public Support(string name)
    {
        occupation = (int)OccupationType.Support;
        this.name = name;

        //加载配置文件
        TextAsset xml = ResourceManager.Instance.Load<TextAsset>("Configs/Player");
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml.text);

        XmlNode root = xmlDoc.SelectSingleNode("root");
        XmlElement support = root.SelectSingleNode("support") as XmlElement;

        SetData(support);
    }

    public Support()
    {

    }

    public override Player Clone()
    {
        Support newPlayer = new Support();

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
