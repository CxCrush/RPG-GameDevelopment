using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BagView:BaseView
{
    Transform[] bagGrids;//所有背包格子

    GameObject gridContent;

    Button btn_PutInOrder;  //整理背包按钮

    int count; //背包里物品种数(有东西的格子数)

    protected override void Start()
    {
        base.Start();

        //先后顺序必须正确

        //先找到所有格子
        gridContent = GameObject.FindWithTag("BagGridContent");
        LoadGrids();

        //再从文件中读取背包中的物品信息
        MyEventSystem.AddListener(EventsNames.addProp, AddProp);
        PropModel.Instance.LoadBagInfoFromXml();
      
        btn_PutInOrder = transform.FindChild("Btn_PutInOrder").GetComponent<Button>();
        btn_PutInOrder.onClick.AddListener(OnPutInOrder);
    }

    protected override void Update()
    {
         if (Input.GetKeyDown(KeyCode.F))
        {
            int i=UnityEngine.Random.Range(0,PropModel.Instance.propInfoList.Count);
            PropVo prop = PropModel.Instance.propInfoList[i].Clone();
            PropModel.Instance.AddProp(prop);
        }

        base.Update();
    }

    public override void OnEnter()
    {
        base.OnEnter();
    }

    public override void OnExit()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public override void OnResume()
    {
        base.OnResume();
    }


    void AddProp(System.Object obj)
    {
        PropVo newProp = (obj as PropVo).Clone();

        int index = GetFirstIndexOfEmpty();

        if (index != -1)
        {
            //生成预制物
            GameObject propPrefab = ResourceManager.Instance.Load("Prop/PropModel");
            GameObject propGo = Instantiate(propPrefab);

            //设置父物体
            propGo.transform.SetParent(bagGrids[index]);
            propGo.transform.localPosition = Vector3.zero;
            propGo.transform.localScale = Vector3.one;

            //添加管理脚本
            GridItemRenderer renderer = propGo.GetComponent<GridItemRenderer>();
            if (renderer==null)
            {
                renderer = propGo.AddComponent<GridItemRenderer>();
            }

            //设置脚本属性
            renderer.SetData(newProp, bagGrids);
            //加进背包信息容器
            PropModel.Instance.bagInfoList.Add(newProp);

            //更新物品数量
            count = PropModel.Instance.bagInfoList.Count;
        }

        else
        {
            //生成数量不足提示窗口
            print("背包已满,充值扩充背包!");
        }
    }

    int GetFirstIndexOfEmpty()
    {
        for (int i = 0; i < bagGrids.Length; i++)
        {
            if (bagGrids[i]!=null&&bagGrids[i].childCount==0)
            {
                return i;
            }
        }

        return -1;
    }
    void LoadGrids()
    {
        if (gridContent==null)
        {
            return;
        }

        bagGrids = new Transform[gridContent.transform.childCount];

        for (int i = 0; i < gridContent.transform.childCount; i++)
        {
            bagGrids[i] = gridContent.transform.GetChild(i);
        }
    }

    //一键整理
    void OnPutInOrder()
    {
        //背包为空,直接返回
        if (count==0)
        {
            return;
        }

        //整理思路:从前往后遍历所有格子,将后面格子的东西移到前面的格子

        int k=0; //记录找到的第一个空格子之后的第一个不空的格子的后一个,以免多次无用查找
        for (int i = 0; i < bagGrids.Length; i++)
        {
            //找到第一个空格子,从后面一个格子开始找到一个不为空的,将其放在第i个格子下
            if (bagGrids[i].childCount==0)
            {
                k=k>(i+1)?k:i+1;

                for (int j = k; j < bagGrids.Length;j++ )
                {
                    //不为空,移到前面第一个空格子
                    if (bagGrids[j].childCount != 0)
                    {
                        //将j下面的物品父物体设为i
                        Transform childJ = bagGrids[j].GetChild(0);
                        childJ.SetParent(bagGrids[i]);
                        childJ.localPosition = Vector3.zero;
                        childJ.localScale = Vector3.one;

                        //更新K
                        k = j + 1;
                        break;
                    }
                }
            }
        }
    }

    public void OnApplicationQuit()
    {
        //存储背包物品信息
        PropModel.Instance.SaveBagInfoToXml();
    }


}
