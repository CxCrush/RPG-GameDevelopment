using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerHeadBar : MonoBehaviour
{

    //人物信息
    Text playerInfo;
    //血条
    Slider hp;
    Player selfPlayer;

	// Use this for initialization
 
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        transform.forward = Camera.main.transform.forward;

        if (selfPlayer!=null)
        {
            hp.value = selfPlayer.Hp;
        }
      
	}

    public void SetData(Player player)
    {
        selfPlayer = player;

        playerInfo = transform.FindChild("PlayerInfo").GetComponent<Text>();
        playerInfo.text = player.name + " " +
            PlayerInfoModel.Instance.occupationName[player.occupation]+ " LV " + player.level;

        hp = transform.FindChild("HpBar").GetComponent<Slider>();
        hp.wholeNumbers = true;
        hp.maxValue = player.MaxHp;
        hp.value = player.Hp;
    }

    
}
