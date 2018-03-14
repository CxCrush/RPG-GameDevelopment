using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;


//UI界面类型枚举
public enum UIPanelType
{
   
    knapsack=1,  //背包
    main, //主界面
    shop,  //商店
    skill,    //技能
    system,  //系统设置
    task,  //任务
    chat, //npc对话
    playerInfo //人物属性界面
}
public class UIManager : Singleton<UIManager> 
{
    //存储UI预制物路径
    Dictionary<UIPanelType,string> UIPanelPathDic =new Dictionary<UIPanelType,string>();

    //存储UI界面脚本引用
    public Dictionary<UIPanelType,BaseView> UIViewDic=new Dictionary<UIPanelType,BaseView>();

    //使用栈管理打开的界面
    public Stack<BaseView> viewStack = new Stack<BaseView>();

    //UI界面的父物体
    Transform canvas;
    public UIManager()
    {
        LoadPanelPath();
    }

    //加载资源
    void LoadPanelPath()
    {
        TextAsset xml=ResourceManager.Instance.Load<TextAsset>("Configs/UI");
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xml.text);

        XmlNode root=xmlDoc.SelectSingleNode("root");

        XmlNodeList list = root.SelectNodes("UI");

        for (int i = 0; i < list.Count;i++ )
        {
            XmlElement element=list[i] as XmlElement;
            int id = XmlTools.GetIntAttribute(element, "id");
            string path = XmlTools.GetStringAttribute(element, "path");

            //存储进容器
            UIPanelPathDic[(UIPanelType)id] = path;
        }
    }

    public void LoadUI(UIPanelType ui)
    {
        //
        if (UIViewDic.ContainsKey(ui))
        {
            if(UIViewDic[ui].gameObject!=null)
            {
                UIViewDic[ui].SwitchOnOff();
                return;
            }
        }

        //打开新的界面
        BaseView newView = GetView(ui);
        newView.OnEnter();
    }

    public void UnLoadUI(UIPanelType ui)
    {
        UIViewDic[ui].OnExit();

    }

    BaseView GetView(UIPanelType ui)
    {
        //
        if (UIViewDic.ContainsKey(ui))
        {
            return UIViewDic[ui];
        }

        //生成新的UI对象并添加脚本
        GameObject uiPrefab = ResourceManager.Instance.Load("UIPanel/"+UIPanelPathDic[ui]) as GameObject;
        GameObject uiGo = GameObject.Instantiate(uiPrefab);

        //设置父物体
        if (canvas==null)
        {
            canvas=GameObject.Find("Canvas").transform;
        }

        uiGo.transform.SetParent(canvas);

        //设置属性
        BaseView view=uiGo.GetComponent<BaseView>();

        if (view==null)
        {
            view = AddComponentToGameObj(uiGo, ui);
        }

        //存进容器
        UIViewDic.Add(ui, view);
        return view;
    }

    BaseView AddComponentToGameObj(GameObject obj,UIPanelType ui)
    {
        BaseView view = null;

        switch (ui)
        {
            case UIPanelType.knapsack: view = obj.AddComponent<BagView>(); view.shortcutKey = KeyCode.B;break;
            case UIPanelType.shop: view = obj.AddComponent<ShopView>(); view.shortcutKey = KeyCode.H; break;
            case UIPanelType.skill: view = obj.AddComponent<SkillView>(); view.shortcutKey = KeyCode.K; break;
            case UIPanelType.system: view = obj.AddComponent<SystemSetttingView>(); view.shortcutKey = KeyCode.I; break;
            case UIPanelType.task: view = obj.AddComponent<TaskView>(); view.shortcutKey = KeyCode.T; break;
            case UIPanelType.chat: view = obj.AddComponent<ChatView>(); view.shortcutKey = KeyCode.U; break;
            case UIPanelType.playerInfo: view = obj.AddComponent<PlayerInfoView>(); view.shortcutKey = KeyCode.C; break;
            case UIPanelType.main: view = obj.AddComponent<MainView>(); break;
            default: break;
        }

        return view;
    }
}
