using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TaskView : BaseView
{
    Text desc;  //任务描述
    Text progress;   //进度
    Button giveUpBtn;  //放弃任务
    Task curTask;  //当前任务数据引用
    protected override void Start()
    {
        base.Start();
    
        desc = transform.FindChild("Desc").GetComponent<Text>();

        progress = transform.FindChild("Progress").GetComponent<Text>();
        giveUpBtn = transform.FindChild("GiveUpBtn").GetComponent<Button>();

        MyEventSystem.AddListener(EventsNames.setTaskdata, SetData);
        MyEventSystem.AddListener(EventsNames.updateTaskData, UpdateTaskData);
    }

    protected override void Update()
    {
        base.Update();

        if (curTask != null)
        {
            desc.text = "任务描述：\n" + curTask.desc + curTask.targetQuantitiy;
            progress.text = "任务进度：" + curTask.completion + "/" + curTask.targetQuantitiy;
        }
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

    void SetData(System.Object obj)
    {
        curTask = obj as Task;
    }

    void UpdateTaskData()
    {
        if(curTask!=null)
        {
            curTask.completion++;
            if(curTask.IsDone)
            {
                MyEventSystem.Dispatch(EventsNames.completeTask);
            }
        }
    }
}
