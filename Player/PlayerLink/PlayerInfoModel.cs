using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;


public enum OccupationType
{
    Warrior, Support, Mage, Assassin
}

public enum Gender
{
    Male,Female
}
public class PlayerInfoModel:Singleton<PlayerInfoModel>
{
    static string xmlPath = "/playerInfo.xml";
    static string directoryPath = "Configs/";
    string playerInfoPath;
    public string accountPath;

    public PlayerInfoModel()
    {
        LoadCharacterImg();
    }

    public Sprite[,] headImg;  //角色头像
    public GameObject[,] playerBody; //角色模型
    public string[] occupationName;

    //存储已创建的角色信息
    public List<Player> characterInfoList = new List<Player>();
    
    
    //存储选择的角色信息
    Player selectedPlayer;
    int selectedIndex;

    public int SelectedIndex
    {
        get { return selectedIndex; }
        set { selectedIndex = value; }
    }

    public Player SelectedPlayer
    {
        get { return selectedPlayer; }
        set { selectedPlayer = value; }
    }

    public void LoadXml(string userName)
    {
        //保存账号路径
        accountPath = directoryPath + userName;

        playerInfoPath = accountPath + xmlPath; //保存玩家信息存储路径

        //加载角色信息
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(playerInfoPath);

        XmlNode root = xmlDoc.SelectSingleNode("root");

        XmlNodeList characters = root.SelectNodes("character");

        for (int i = 0; i < characters.Count;i++ )
        {
            //保存角色信息并添加进容器
            int occupation = XmlTools.GetIntAttribute(characters[i] as XmlElement, "occupation");
            string name = XmlTools.GetStringAttribute(characters[i] as XmlElement, "name");
            Player charInfo = PlayerFactory.GeneratePlayer((OccupationType)occupation, name);
            charInfo.SetData(characters[i] as XmlElement);
            characterInfoList.Add(charInfo);
        }
    }

    public void AddCharacter(string name,int occupation,int server,int sex ,int level=1)
    {
        //创建玩家类
        Player player = PlayerFactory.GeneratePlayer((OccupationType)occupation, name);

        //找到账号路径并添加角色信息
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(playerInfoPath);

        XmlNode root = xmlDoc.SelectSingleNode("root");
        XmlElement character = xmlDoc.CreateElement("character");
        character.SetAttribute("name", name);
        character.SetAttribute("level", level.ToString());
        character.SetAttribute("exp", player.Exp.ToString());
        character.SetAttribute("server", server.ToString());
        character.SetAttribute("sex", sex.ToString());
        character.SetAttribute("money", player.Money.ToString());
        character.SetAttribute("occupation", occupation.ToString());
        character.SetAttribute("hp", player.Hp.ToString());
        character.SetAttribute("maxHp", player.MaxHp.ToString());
        character.SetAttribute("mp", player.Mp.ToString());
        character.SetAttribute("maxMp", player.MaxMp.ToString());
        character.SetAttribute("atk", player.Atk.ToString());
        character.SetAttribute("def", player.Def.ToString());
        character.SetAttribute("strength", player.Strength.ToString());
        character.SetAttribute("intelligence", player.Intelligence.ToString());
        character.SetAttribute("magicPower", player.MagicPower.ToString());
        character.SetAttribute("agility", player.Agility.ToString());
        root.AppendChild(character);

        player.SetData(character);
        //添加进容器
        characterInfoList.Add(player);
        xmlDoc.Save(playerInfoPath);

        MyEventSystem.Dispatch(EventsNames.updateCharacter);
    }

    public void RemoveCharacter(int index)
    {
        //找到账号路径并添加角色信息
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(playerInfoPath);

        XmlNode root = xmlDoc.SelectSingleNode("root");
        XmlNodeList list = root.SelectNodes("character");

        if (index<list.Count)
        {
            //更新xml
            root.RemoveChild(list[index]);

            //更新容器
            characterInfoList.RemoveAt(index);

            xmlDoc.Save(playerInfoPath);
        }
       
    }
    void LoadCharacterImg()
    {
        //角色图片路径
        string headImgPath = "Characters/Head/";
        string playerBodyPath = "Characters/Body/";

   
        headImg = new Sprite[4, 2];
        playerBody = new GameObject[4,2];

        occupationName = new string[4];
        //加载资源

        for (int i = 0; i < headImg.GetLength(0); i++)
        {
            for (int j = 0; j < headImg.GetLength(1); j++)
            {
                OccupationType type = (OccupationType)i;
                Gender gender = (Gender)j;
                string sex = gender.ToString();
                string occupation = type.ToString();
                //头像图片
                headImg[i, j] = ResourceManager.Instance.Load<Sprite>(headImgPath + occupation + "_" + sex);
                //大图
                playerBody[i, j] = ResourceManager.Instance.Load(playerBodyPath + occupation + "_" + sex);

                switch (type)
                {
                    case OccupationType.Warrior:
                        occupationName[i] = "战士";
                        break;
                    case OccupationType.Support:
                     
                        occupationName[i] = "辅助";
                        break;
                    case OccupationType.Mage:

                        occupationName[i] = "法师";
                        break;
                    case OccupationType.Assassin:
                        occupationName[i] = "刺客";
                        break;
                    default:
                        break;
                }
        
            }
        }
    }

    //存档
    public void SavePlayerInfoToXml()
    {
        Player player = SelectedPlayer;
        //恢复血量
        player.Hp = player.MaxHp;
        player.Mp = player.MaxMp;

        XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.Load(playerInfoPath);

        XmlNode root = xmlDoc.SelectSingleNode("root");
        XmlNodeList list = root.SelectNodes("character");

        for (int i = 0; i < list.Count;i++ )
        {
            if (i == selectedIndex)
            {
                XmlElement character = list[i] as XmlElement;
                character.SetAttribute("name", player.name);
                character.SetAttribute("level", player.level.ToString());
                character.SetAttribute("exp", player.Exp.ToString());
                character.SetAttribute("server", player.server.ToString());
                character.SetAttribute("sex", player.sex.ToString());
                character.SetAttribute("occupation", player.occupation.ToString());
                character.SetAttribute("hp", player.Hp.ToString());
                character.SetAttribute("maxHp", player.MaxHp.ToString());
                character.SetAttribute("mp", player.Mp.ToString());
                character.SetAttribute("maxMp", player.MaxMp.ToString());
                character.SetAttribute("atk", player.Atk.ToString());
                character.SetAttribute("def", player.Def.ToString());
                character.SetAttribute("strength", player.Strength.ToString());
                character.SetAttribute("intelligence", player.Intelligence.ToString());
                character.SetAttribute("magicPower", player.MagicPower.ToString());
                character.SetAttribute("agility", player.Agility.ToString());
              
                xmlDoc.Save(playerInfoPath);
                return;
            }
        }
       
    }
}
