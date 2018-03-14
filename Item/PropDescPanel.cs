using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PropDescPanel : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetData(PropVo prop)
    {
        Image descImg = transform.FindChild("DescImg").GetComponent<Image>();
        Sprite sprite=ResourceManager.Instance.Load<Sprite>("Prop/" + prop.id);
        descImg.sprite = sprite;

        Text name = transform.FindChild("Name").GetComponent<Text>();
        name.text = prop.name;

        Text desc = transform.FindChild("Desc").GetComponent<Text>();
        desc.text = prop.desc;
        
    }
}
