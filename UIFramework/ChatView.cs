using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using DG.Tweening;
using UnityEngine.UI;
public class ChatView : BaseView,IPointerClickHandler
{
    Button acceptBtn;
    Text content;
    Task curTask;
    protected override void Start()
    {
        base.Start();
        content=transform.FindChild("ChatContent").GetComponent<Text>();
        acceptBtn = transform.FindChild("AcceptBtn").GetComponent<Button>();

        curTask = TaskModel.Instance.taskQueue.Dequeue();
        acceptBtn.onClick.AddListener(AcceptTask);
    }
    public override void OnExit()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //canvasGroup.DOFade(1, 0.5f);
        //设置位置
        transform.localScale = Vector3.one;
        transform.localPosition = new Vector3(0, -200);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
      
    }

    IEnumerator PrintByWord()
    {
        while (true)
        {
           
        }
    }

    void AcceptTask()
    {
        MyEventSystem.Dispatch(EventsNames.setTaskdata, curTask);
        OnExit();
    }
}
