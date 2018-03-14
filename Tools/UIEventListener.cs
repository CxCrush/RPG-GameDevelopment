using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class UIEventListener : UnityEngine.EventSystems.EventTrigger
{
    public delegate void VoidDelegate(GameObject go);
    public VoidDelegate onClick;
    public VoidDelegate onDown;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onUp;
    public VoidDelegate onSelect;
    public VoidDelegate onUpdateSelect;
    public VoidDelegate onDrag;
    public VoidDelegate onEndDrag;
    public VoidDelegate onBeginDrag;
    public VoidDelegate onPressing;
    public VoidDelegate onDoubleClick;

    public delegate void VoidBoolDelegate(GameObject go,bool state);
    public VoidBoolDelegate onToolTip;

    public delegate void VoidObjDelegate(GameObject go1, GameObject go2);
    public VoidObjDelegate onDrop;

    public delegate void ToggleDelegate(Toggle toggle);
    public ToggleDelegate onToggleChange;

    public delegate void EventDelegate(PointerEventData eventData);
    public EventDelegate onPointerDown;
    public EventDelegate onPointerUp;
    public EventDelegate onDraging;
    static public UIEventListener Get(UIBehaviour ui)
    {
        GameObject go = ui.gameObject;
        UIEventListener listener = go.GetComponent<UIEventListener>();
        if (listener == null) listener = go.AddComponent<UIEventListener>();
        return listener;
    }

    static public UIEventListener Get(GameObject go)
    {
        UIEventListener listener = go.GetComponent<UIEventListener>();
        if (listener == null) listener = go.AddComponent<UIEventListener>();
        return listener;
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null) onClick(gameObject);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null) onDown(gameObject);
        if (onPointerDown != null) onPointerDown(eventData);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null) onEnter(gameObject);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (onExit != null) onExit(gameObject);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (onUp != null) onUp(gameObject);
        if (onPointerUp != null) onPointerUp(eventData);
    }
    public override void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null) onSelect(gameObject);
        if (onToggleChange != null) onToggleChange(gameObject.GetComponent<Toggle>());
    }
    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelect != null) onUpdateSelect(gameObject);
        if (onToggleChange != null) onToggleChange(gameObject.GetComponent<Toggle>());
    }
    public override void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null) onDrag(gameObject);
        if (onDraging != null) onDraging(eventData);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        if (onEndDrag != null) onEndDrag(gameObject);
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (onBeginDrag != null) onBeginDrag(gameObject);
    }
}