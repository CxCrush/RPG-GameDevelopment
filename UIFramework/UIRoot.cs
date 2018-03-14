using UnityEngine;
using System.Collections;

public class UIRoot : MonoBehaviour {

	// Use this for initialization
	void Start () {

        UIManager.Instance.LoadUI(UIPanelType.main);

        UIManager.Instance.LoadUI(UIPanelType.knapsack);
        UIManager.Instance.UnLoadUI(UIPanelType.knapsack);

        UIManager.Instance.LoadUI(UIPanelType.task);
        UIManager.Instance.UnLoadUI(UIPanelType.task);

        UIManager.Instance.LoadUI(UIPanelType.skill);
        UIManager.Instance.UnLoadUI(UIPanelType.skill);

        UIManager.Instance.LoadUI(UIPanelType.system);
        UIManager.Instance.UnLoadUI(UIPanelType.system);

        UIManager.Instance.LoadUI(UIPanelType.shop);
        UIManager.Instance.UnLoadUI(UIPanelType.shop);

        UIManager.Instance.LoadUI(UIPanelType.chat);
        UIManager.Instance.UnLoadUI(UIPanelType.chat);

        UIManager.Instance.LoadUI(UIPanelType.playerInfo);
        UIManager.Instance.UnLoadUI(UIPanelType.playerInfo);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

}
