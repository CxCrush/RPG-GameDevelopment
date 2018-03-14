using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class UIDrag :EventTrigger 
{
    Vector3 offset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public override void OnBeginDrag(PointerEventData eventData)
    {
        Vector2 localPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent as RectTransform, Input.mousePosition,
            null, out localPos);

        offset = -localPos;
    }

    public override void OnDrag(PointerEventData eventData)
    {
        Vector2 localPos;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.parent as RectTransform, Input.mousePosition,
            null, out localPos);

        transform.parent.localPosition = new Vector3(localPos.x,localPos.y) + offset;
    }

    
}
