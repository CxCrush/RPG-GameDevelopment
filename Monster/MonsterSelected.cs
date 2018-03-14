using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class MonsterSelected : MonoBehaviour
{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Selected()
    {
        MyEventSystem.Dispatch(EventsNames.setPlayerAttackTarget, gameObject);
        print("目标选中");
    }
}
