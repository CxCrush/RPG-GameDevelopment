using System;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class MonsterModel:Singleton<MonsterModel>
{
    //存储怪物数据模板
    public List<Monster> monsterList = new List<Monster>();
    //存储怪物预制物
    public List<GameObject> monsterPrefabList = new List<GameObject>();

    public MonsterModel()
    {
        LoadMonsterVo();
    }

    //加载怪物数据信息
    void LoadMonsterVo()
    {
        XmlDocument xmlDoc = new XmlDocument();
        TextAsset xml = ResourceManager.Instance.Load<TextAsset>("Configs/Monster");
        xmlDoc.LoadXml(xml.text);

        XmlNode root = xmlDoc.SelectSingleNode("root");
        XmlNodeList list = root.SelectNodes("monster");
        for (int i = 0; i < list.Count;i++ )
        {
            //加载模板数据
            Monster monster = new Monster(list[i] as XmlElement);
            monsterList.Add(monster);

            //加载预制物信息
            GameObject monsterPrefab = ResourceManager.Instance.Load("Monsters/" + monster.id);
            monsterPrefabList.Add(monsterPrefab);
            
        }
    }

}
