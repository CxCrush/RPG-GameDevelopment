using UnityEngine;
using System.Collections;

public class SkillView : BaseView
{

    public override void OnExit()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
    }
}
