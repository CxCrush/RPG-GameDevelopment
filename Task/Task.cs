using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

public class Task
{
    public int id;  //任务标识
    public string name;  //任务名称
    public string desc;   //任务描述
    public int completion; //完成度
    public int targetQuantitiy; //目标量
    private bool isDone;

    public bool IsDone
    {
        get { return isDone = completion >= targetQuantitiy; }
    }
    public Task(XmlElement xml)
    {
        SetData(xml);
    }
    public virtual void SetData(XmlElement xml)
    {
        name = XmlTools.GetStringAttribute(xml, "name");
        desc = XmlTools.GetRichTextAttribute(xml, "desc",'%','\n');
        completion = XmlTools.GetIntAttribute(xml, "completion");
        targetQuantitiy = XmlTools.GetIntAttribute(xml, "targetQuantitiy");
        
    }
}
