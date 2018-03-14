using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public enum SkillID
{
    Skill1,Skill2,Skill3,Skill4
}
public class PlayerSkillTrigger : MonoBehaviour {

    Image cdImg;
    Button btn;

    public PlayerState selfState;
    public KeyCode keyCode;
    PlayerSkill skill;
    public float cdTime = 3;
    public SkillID id;

    int neededMp;//需要消耗的魔法值

    Player player;//玩家脚本引用
   
    bool isCDing = false;//是否在冷却中
	// Use this for initialization
    void Awake()
    {

    }
	void Start () {
        cdImg = transform.FindChild("CD").GetComponent<Image>();
        cdImg.fillAmount = 0.0f;

        btn = GetComponent<Button>();
        btn.onClick.AddListener(PlayerSkillRealease);

        player = PlayerInfoModel.Instance.SelectedPlayer;
	}
	
	// Update is called once per frame
	void Update () {

        int i=(int)id;
        int curMp = player.Mp;
        int mpCost=PlayerSkillModel.Instance.skillList[i].MpCost;

        if (Input.GetKeyDown(keyCode) && !isCDing &&curMp>=mpCost)
        {
            PlayerSkillRealease();
            //耗蓝
            player.Mp -= mpCost;
        }

        if (player.Mp<=0)
        {
            cdImg.fillAmount = 1.0f;
        }
        btn.interactable =cdImg.fillAmount == 0.0f;
	}

    void Check()
    {
        switch (selfState)
        {
            case PlayerState.Attack1:
                if (player.Mp<50)
                {

                }
                break;
            case PlayerState.Attack2:
                break;
            case PlayerState.Attack3:
                break;
            case PlayerState.Attack4:
                break;
            default:
                break;
        }
    }
    void PlayerSkillRealease()
    {
        cdImg.fillAmount = 1.0f;
        isCDing = true;
        MyEventSystem.Dispatch(EventsNames.addPlayerSkillTrigger, selfState);
        StartCoroutine(PlayerSkillTriggerCoolDown());
    }

    //
    IEnumerator PlayerSkillTriggerCoolDown()
    {
        while (true)
        {
            cdImg.fillAmount -=Time.deltaTime/cdTime;

            if (cdImg.fillAmount<=0)
            {
                isCDing = false;
                break;
            }

            yield return null;
        }
    }

   
}
