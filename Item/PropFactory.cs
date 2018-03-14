using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

public class PropFactory
{
    public static PropVo GetProp(PropId id,XmlElement xml)
    {
        PropVo prop=null;

        switch (id)
        {
            case PropId.goldBox: prop = new PropGoldBoxVo(xml);
                break;
            case PropId.silverBox: prop = new PropSilverBoxVo(xml);
                break;
            case PropId.bronzeBox: prop = new PropBronzeBoxVo(xml);
                break;
            case PropId.goldKey: prop = new PropGoldKeyVo(xml);
                break;
            case PropId.silverkey: prop = new PropSilverKeyVo(xml);
                break;
            case PropId.bronzeKey: prop = new PropBronzeKeyVo(xml);
                break;
            case PropId.hp: prop = new PropHpVo(xml);
                break;
            case PropId.mp: prop = new PropMpVo(xml);
                break;
            default:
                break;
        }

        return prop;
    }
}
