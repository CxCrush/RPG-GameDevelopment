using UnityEngine;
using System.Collections;
using System.Xml;

public enum PropId
{
    goldBox=10001,silverBox,bronzeBox,goldKey,silverkey,bronzeKey,hp,mp
}
public class PropVo  
{
    public int id; //物品表示id
    public string name; //名字
    public string desc;  //描述
    public int max_num;  //最大数量
    public int num;     //当前数量
    public int price;  //价格

    public  PropVo(XmlElement xml)
    {
        SetData(xml);
    }

    public virtual void SetData(XmlElement xml)
    {
        id = XmlTools.GetIntAttribute(xml, "id");
        name = XmlTools.GetRichTextAttribute(xml, "name");
        desc = XmlTools.GetRichTextAttribute(xml, "desc");
        max_num = XmlTools.GetIntAttribute(xml, "max_num");
        price = XmlTools.GetIntAttribute(xml, "price");

        num = 1;
    }
    public virtual PropVo Clone()
    {
        PropVo prop = new PropVo();

        prop.id=this.id ;
        prop.name = this.name;
        prop.desc = this.desc;
        prop.max_num = this.max_num;
        prop.num = 1;

        return prop;
    }

    public PropVo()
    {
        id = 0;
        name = "";
        desc = "";
        max_num =0;
        num = 1;
    }

    public virtual void UseEffect()
    {

    }


}
