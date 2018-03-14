using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {

    Button btn;

    public UIPanelType type;
	// Use this for initialization
	void Start () {

        btn = GetComponent<Button>();
        if (btn != null)
        {
            btn.onClick.AddListener(() => UIManager.Instance.LoadUI(type));
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
