using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

public class PropMpVo : PropVo
{
    public int mp;

    public override PropVo Clone()
    {
        PropMpVo prop = new PropMpVo();

        prop.id = this.id;
        prop.name = this.name;
        prop.desc = this.desc;
        prop.max_num = this.max_num;
        prop.mp = this.mp;
        prop.num = 1;

        return prop;
    }

    public PropMpVo()
    {
        
    }

    public PropMpVo(XmlElement xml)
    {
        SetData(xml);
    }

    public override void SetData(XmlElement xml)
    {
        id = XmlTools.GetIntAttribute(xml, "id");
        name = XmlTools.GetRichTextAttribute(xml, "name");
        desc = XmlTools.GetRichTextAttribute(xml, "desc");
        max_num = XmlTools.GetIntAttribute(xml, "max_num");
        mp = XmlTools.GetIntAttribute(xml, "mp");

        num = 1;
    }

    public override void UseEffect()
    {
        //发送加血信息
        MyEventSystem.Dispatch(EventsNames.updatePlayerMp, mp);
    }
}
