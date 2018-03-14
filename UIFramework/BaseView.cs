using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class BaseView : MonoBehaviour {

    protected Button btn_Close;

    public UIPanelType selfType;

    protected CanvasGroup canvasGroup;

    public KeyCode shortcutKey; //打开关闭快捷键
	// Use this for initialization
	protected virtual void Start ()
    {

        Transform btn = transform.FindChild("CloseButton");

        if (btn!=null)
        {
            btn_Close = btn.GetComponent<Button>();
            UIEventListener.Get(btn_Close).onClick=onClose;
        }

        canvasGroup = GetComponent<CanvasGroup>();
	}
	
	// Update is called once per frame
	protected virtual void Update () {

        if (Input.GetKeyDown(shortcutKey))
        {
            SwitchOnOff();
        }
	}

    public virtual void SwitchOnOff()
    {
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        if (canvasGroup != null)
        {
            if (canvasGroup.alpha == 0.0f)
            {
                OnResume();
            }

            else
                OnExit();
        }
      
    }
    public virtual void OnEnter()
    {
        //设置位置
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;

        if (canvasGroup==null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }
        
        if (canvasGroup!=null)
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;

            //设置在父物体下的层级渲染最后渲染,遮挡其他界面
            transform.SetAsLastSibling();
        }

    }

    protected void onClose(GameObject go)
    {
        OnExit();
    }
    public virtual void OnExit()
    {
        if (canvasGroup!= null)
        {
            canvasGroup.alpha = 0.0f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public virtual void OnResume()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            //设置在父物体下的层级渲染最后渲染,遮挡其他界面
            transform.SetAsLastSibling();
        }
    }

    public virtual void OnDestroy()
    {
        if (UIManager.Instance.UIViewDic.Count>0)
        {
            UIManager.Instance.UIViewDic.Clear();
        }
        if (UIManager.Instance.viewStack.Count>0)
        {
            UIManager.Instance.viewStack.Clear();
        }
    }


}
