using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MonsterHeadBar : MonoBehaviour {

	// Use this for initialization

    Text monsterInfo;  //怪物信息
    Slider hp; //血条

    Monster monster; //数据模型引用

	void Start () {

       
    }
	
	// Update is called once per frame
	void Update () {

        transform.forward = Camera.main.transform.forward;

        if (hp!=null)
        {
            hp.value = monster.hp;
        }
       
	}

    public void SetData(Monster _monster)
    {
        monster = _monster;

        monsterInfo = transform.FindChild("Info").GetComponent<Text>();
        monsterInfo.text = monster.name + " " + " LV " + monster.level;

        hp = transform.GetComponent<Slider>();
        hp.wholeNumbers = true;
        hp.maxValue = monster.maxHp;
        hp.value = monster.hp;
    }
}
