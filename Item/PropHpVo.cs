using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

public class PropHpVo : PropVo
{
    public int hp;

    public override PropVo Clone()
    {
        PropHpVo prop = new PropHpVo();

        prop.id = this.id;
        prop.name = this.name;
        prop.desc = this.desc;
        prop.max_num = this.max_num;
        prop.hp = this.hp;
        prop.num = 1;

        return prop;
    }

    public PropHpVo()
    {
       
    }
    
    public PropHpVo(XmlElement xml)
    {
        SetData(xml);
    }
    
    public override void SetData(XmlElement xml)
    {
        id = XmlTools.GetIntAttribute(xml, "id");
        name = XmlTools.GetRichTextAttribute(xml, "name");
        desc = XmlTools.GetRichTextAttribute(xml, "desc");
        max_num = XmlTools.GetIntAttribute(xml, "max_num");
        hp = XmlTools.GetIntAttribute(xml, "hp");

        num = 1;
    }

    public override void UseEffect()
    {
        //发送加血信息
        MyEventSystem.Dispatch(EventsNames.updatePlayerHp, hp);
    }
}
