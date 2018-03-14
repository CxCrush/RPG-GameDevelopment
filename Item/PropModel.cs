using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;

public class PropModel : Singleton<PropModel>
{
    //从xml读取的道具模板信息表
    public List<PropVo> propInfoList = new List<PropVo>();

    //背包实时存储的道具信息
    public List<PropVo> bagInfoList = new List<PropVo>();

    string accountPath;//账号路径
    static string itemInfoPath="/ItemInfo.xml";//用户物品信息存储路径
    public PropModel()
    {
        LoadPropInfoFromXml();
        accountPath = PlayerInfoModel.Instance.accountPath;
    }

    public void LoadPropInfoFromXml()
    {
        //预先配置好的道具信息
        TextAsset xml = ResourceManager.Instance.Load<TextAsset>("Configs/Prop");

        XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.LoadXml(xml.text);
        XmlNode root = XmlDoc.SelectSingleNode("root");
        XmlNodeList list = root.SelectNodes("prop");
        for (int i = 0; i < list.Count; i++)
        {

            XmlElement element = list[i] as XmlElement;

            int idVal=XmlTools.GetIntAttribute(element,"id");
            PropId id = (PropId)idVal;
            PropVo prop = PropFactory.GetProp(id,element);
            propInfoList.Add(prop);
        }
    }

    public void AddProp(PropVo prop)
    {
        for (int i = 0; i < bagInfoList.Count;i++)
        {
            if (prop.id == bagInfoList[i].id && bagInfoList[i].num<prop.max_num)
            {
                bagInfoList[i].num++;
                return;
            }
        }

        MyEventSystem.Dispatch(EventsNames.addProp,prop);
    }

    public void RemoveProp(PropVo prop)
    {
        if (bagInfoList.Contains(prop))
        {
            bagInfoList.Remove(prop);
        }
    }
    public void LoadBagInfoFromXml()
    {
        string path = accountPath + itemInfoPath;

        if (File.Exists(path))
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            XmlNode root = xmlDoc.SelectSingleNode("root");
            XmlNodeList list = root.SelectNodes("item");

            //遍历所有物品存储的信息
            for (int i = 0; i < list.Count;i++ )
            {
                XmlElement element = list[i] as XmlElement;
                int id=XmlTools.GetIntAttribute(element,"id");
                int num=XmlTools.GetIntAttribute(element,"num");

                //遍历物品模板信息
                for (int j = 0; j < propInfoList.Count;j++)
                {
                    //对比是否为同一种物品
                    PropVo prop=propInfoList[j];

                    if (id==prop.id)
                    {
                        PropVo newProp = prop.Clone();
                       
                        //添加物品num次
                        for (int k = 0; k < num;k++ )
                        {
                            AddProp(newProp);
                        }

                        break;
                    }
                }
            }
        }
    }

    public void SaveBagInfoToXml()
    {
        string path=accountPath+itemInfoPath;
        //有文件存在
        if (File.Exists(path))
        {
           //先清空xml
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(path);
            xmlDoc.RemoveAll();
            File.Delete(path);

            //再次写入
            CreateItemXml(path);
        }

        //第一次存档,创建新文件
        else
        {
            CreateItemXml(path);
        }
    }

    void CreateItemXml(string path)
    {
        XmlDocument xmlDoc = new XmlDocument();
        XmlElement root = xmlDoc.CreateElement("root");
        for (int i = 0; i < bagInfoList.Count; i++)
        {
            PropVo prop = bagInfoList[i];

            //只需存储id和数量
            XmlElement item = xmlDoc.CreateElement("item");
            item.SetAttribute("id", prop.id.ToString());
            item.SetAttribute("num", prop.num.ToString());
            root.AppendChild(item);
        }

        xmlDoc.AppendChild(root);
        xmlDoc.Save(path);
    }

    
}
