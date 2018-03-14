using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text;

public enum DescPanelPos
{
    UpperLeft,UpperRight,DownLeft,DownRight
}
public class GridItemRenderer : MonoBehaviour,IPointerClickHandler,IDragHandler,
    IBeginDragHandler,IEndDragHandler,IPointerEnterHandler,IPointerExitHandler

{
    PropVo selfProp;//道具数据对象引用

    Vector3 offset;//鼠标和UI之间的偏移量

    Image propImg;  //道具显示的图片

    Text num; //道具数量显示文本
    StringBuilder numStr=new StringBuilder();

    Transform originalParent;  //源位置
    Transform canvas;
    Transform[] bagGrids; //所有的背包格子

    Transform[] dirFramePos=new Transform[4]; //四个方位矩形框

    bool isDrag = false; //是否正在拖拽

    GameObject descPanelGo;//描述板对象
	// Use this for initialization
	void Start () {

        propImg = transform.GetComponent<Image>();
        num=transform.FindChild("Num").GetComponent<Text>();
        canvas = GameObject.Find("Canvas").transform;
        InitDescPanelPos();
	}
	
    void InitDescPanelPos()
    {
        for (int i = 0; i < dirFramePos.Length;i++)
        {
            switch ((DescPanelPos)i)
            {
                case DescPanelPos.UpperLeft:
                    dirFramePos[i] = GameObject.Find(DescPanelPos.UpperLeft.ToString()).transform;
                    break;
                case DescPanelPos.UpperRight:
                    dirFramePos[i] = GameObject.Find(DescPanelPos.UpperRight.ToString()).transform;
                    break;
                case DescPanelPos.DownLeft:
                    dirFramePos[i] = GameObject.Find(DescPanelPos.DownLeft.ToString()).transform;
                    break;
                case DescPanelPos.DownRight:
                    dirFramePos[i] = GameObject.Find(DescPanelPos.DownRight.ToString()).transform;
                    break;
                default:
                    break;
            }
        }
       
    }
	// Update is called once per frame
	void Update () {

        num.text = selfProp.num+"";
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        //正在拖拽时不生成描述板
        if (isDrag)
        {
            return;
        }

        CreateDescPanel();
      
    }

    void CreateDescPanel()
    {
        if (descPanelGo == null)
        {
            GameObject descPanelPrefab = ResourceManager.Instance.Load("Prop/DescPanel");
            descPanelGo = Instantiate(descPanelPrefab);

            //先将描述板的父物体设置为道具图片
            descPanelGo.transform.SetParent(transform);

            //设置位置和中心点
            descPanelGo.transform.localPosition = Vector3.zero;
            SetDescPanelPivot(descPanelGo.transform);

            //再次将父物体设置为图片祖父
            //在设为Canvas的子物体 
            if (canvas == null)
            {
                canvas = GameObject.Find("Canvas").transform;
            }

            descPanelGo.transform.SetParent(canvas);
            //****重要!!!!!!必须设置缩放比例
            descPanelGo.transform.localScale = Vector3.one;

            //添加对象管理脚本
            PropDescPanel descPanelScript = descPanelGo.AddComponent<PropDescPanel>();
            descPanelScript.SetData(selfProp);
        }
    }
    void SetDescPanelPivot(Transform descPanelTransform)
    {
        DescPanelPos posIndex =(DescPanelPos) GetPosOfMouse();

        switch (posIndex)
        {
            case DescPanelPos.UpperLeft:
                (descPanelTransform as RectTransform).pivot = new Vector2(0, 1);
                break;
            case DescPanelPos.UpperRight:
                (descPanelTransform as RectTransform).pivot = new Vector2(1, 1);
                break;
            case DescPanelPos.DownLeft:
                (descPanelTransform as RectTransform).pivot = new Vector2(0, 0);
                break;
            case DescPanelPos.DownRight:
                (descPanelTransform as RectTransform).pivot = new Vector2(1, 0);
                break;
            default:
                break;
        }

        
    }

    //得到当前鼠标在哪那个方位矩形框内
    int GetPosOfMouse()
    {
        for (int i = 0; i < dirFramePos.Length;i++)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(dirFramePos[i] as RectTransform,Input.mousePosition))
            {
                return i;
            }
        }

        return -1;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (descPanelGo!=null)
        {
            Destroy(descPanelGo);
            descPanelGo = null;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        if (descPanelGo != null)
        {
            Destroy(descPanelGo);
            descPanelGo = null;
        }
      

        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform as RectTransform, Input.mousePosition,
            null, out localPos);

        offset = -localPos;//计算偏移量

        //记住原来位置
        originalParent = transform.parent;
        //设置父物体为祖父级
        transform.SetParent(canvas);

    }

    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent as RectTransform, Input.mousePosition,
            null, out localPos);

       transform.localPosition = offset + new Vector3(localPos.x, localPos.y);

    }

    public void OnEndDrag(PointerEventData eventData)
    {

        isDrag = false;
     
        if (bagGrids==null)
        {
            return;
        }

        //遍历所有背包格子
        for (int i = 0; i < bagGrids.Length;i++)
        {
            //在格子内
            if (RectTransformUtility.RectangleContainsScreenPoint(bagGrids[i] as RectTransform,Input.mousePosition))
            {
                //格子内没东西,放入格子
                if (bagGrids[i].childCount==0)
                {
                    transform.SetParent(bagGrids[i]);
                }

                //有东西,交换位置
                else
                {
                    Transform child = bagGrids[i].GetChild(0);
                    child.SetParent(originalParent);
                    child.localPosition = Vector3.zero;
                    transform.SetParent(bagGrids[i]);
                }

                transform.localPosition = Vector3.zero;
                transform.localScale = Vector3.one;
                originalParent = transform.parent;
                //生成提示板
                CreateDescPanel();
                return;
            }
        }

        //没在任何一个格子内
        transform.SetParent(originalParent);
        transform.localPosition = Vector3.zero;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //正在拖拽时直接返回
        if (isDrag)
        {
            return;
        }

        //右键使用,左键退出
        if (eventData.button != UnityEngine.EventSystems.PointerEventData.InputButton.Right)
        {
            return;
        }

        //双击使用
        if (eventData.clickCount < 2)
        {
            return;
        }

        if (selfProp.num>0)
        {
            selfProp.num--;
            selfProp.UseEffect();
        }

        if (selfProp.num<=0)
        {
            //销毁道具
            PropModel.Instance.RemoveProp(selfProp);
            //销毁道具描述板
            if (descPanelGo!=null)
            {
                Destroy(descPanelGo);
            }
            Destroy(gameObject);
        }
    }

    public void SetData(PropVo prop,Transform[] bagGrids)
    {
        selfProp = prop;

        if (propImg==null)
        {
            propImg = transform.GetComponent<Image>();
        }

        Sprite sprite = ResourceManager.Instance.Load<Sprite>("Prop/" + selfProp.id);
        propImg.sprite = sprite;

        this.bagGrids = bagGrids;
    }



}
