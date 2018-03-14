using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

public class TaskModel : Singleton<TaskModel>
{
    public Queue<Task> taskQueue = new Queue<Task>();
    public TaskModel()
    {
        InitTaskList();
    }

    void InitTaskList()
    {
        //加载任务信息
        TextAsset xml=ResourceManager.Instance.Load<TextAsset>("Configs/Task");
        XmlDocument xmlDoc=new XmlDocument();
        xmlDoc.LoadXml(xml.text);

        XmlNode root=xmlDoc.SelectSingleNode("root");
        XmlNodeList list=root.SelectNodes("task");

        for(int i=0;i<list.Count;i++)
        {
            Task task = new Task(list[i] as XmlElement);

            if(!task.IsDone)
            {
                taskQueue.Enqueue(task);
            }
        }
    }
    
}
