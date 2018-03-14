using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

public class XmlTools
{
    public static string GetStringAttribute(XmlElement xml,string content)
    {
        return xml.GetAttribute(content);
    }

    public static int GetIntAttribute(XmlElement xml,string content)
    {
        int iRet = int.Parse(xml.GetAttribute(content));
        return iRet;
    }

    public static string GetRichTextAttribute(XmlElement xml,string content,char oldChar='$',char newChar='<')
    {
        string strRet = xml.GetAttribute(content);
        strRet = strRet.Replace(oldChar, newChar);
        return strRet;
    }

}
