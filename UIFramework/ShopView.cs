using UnityEngine;
using System.Collections;

public class ShopView : BaseView
{

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
}
