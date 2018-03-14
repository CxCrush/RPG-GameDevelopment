using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

public class PropGoldBoxVo : PropVo
{
    public PropGoldBoxVo(XmlElement xml)
    {
        SetData(xml);
    }
}

public class PropSilverBoxVo : PropVo
{
    public PropSilverBoxVo(XmlElement xml)
    {
        SetData(xml);
    }
}

public class PropBronzeBoxVo : PropVo
{
    public PropBronzeBoxVo(XmlElement xml)
    {
        SetData(xml);
    }
}

public class PropGoldKeyVo : PropVo
{
    public PropGoldKeyVo(XmlElement xml)
    {
        SetData(xml);
    }
}

public class PropSilverKeyVo : PropVo
{
    public PropSilverKeyVo(XmlElement xml)
    {
        SetData(xml);
    }
}

public class PropBronzeKeyVo : PropVo
{
    public PropBronzeKeyVo(XmlElement xml)
    {
        SetData(xml);
    }
}
