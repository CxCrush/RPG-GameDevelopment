using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MainView : BaseView
{
    Image headImg;

    Slider hp;
    Slider mp;
    Slider exp;

    Text hpValue;
    Text mpValue;
    Text expValue;
    Text level;

    Player player;
    protected override void Start()
    {
        base.Start();

        player = PlayerInfoModel.Instance.SelectedPlayer;
        //加载头像
        Transform head=transform.FindChild("Head");
        headImg = head.FindChild("HeadImg").GetComponent<Image>();
        headImg.sprite = PlayerInfoModel.Instance.headImg[player.occupation, player.sex];

        //加载血量和魔法值信息
        hp = head.FindChild("HP").GetComponent<Slider>();
        hp.wholeNumbers = true;
        hp.maxValue = player.MaxHp;
        hp.value = player.Hp;
        hpValue = hp.transform.FindChild("Text").GetComponent<Text>();
        hpValue.text = "HP " + hp.value + "/" + hp.maxValue;

        mp = head.FindChild("MP").GetComponent<Slider>();
        mp.wholeNumbers = true;
        mp.maxValue = player.MaxMp;
        mp.value = player.Mp;
        mpValue = mp.transform.FindChild("Text").GetComponent<Text>();
        mpValue.text = "MP " + mp.value + "/" + mp.maxValue;
        MyEventSystem.AddListener(EventsNames.updatePlayerMp, UpadatePlayerMp);

        //经验条信息加载
        exp = head.FindChild("Exp").GetComponent<Slider>();
        exp.wholeNumbers = true;
        exp.maxValue = player.MaxExp;
        exp.value = player.Exp;
        expValue = exp.transform.FindChild("Text").GetComponent<Text>();
        expValue.text = "Exp " + exp.value + "/" + exp.maxValue;
        MyEventSystem.AddListener(EventsNames.updatePlayerExp, UpadatePlayerExp);

        level = exp.transform.FindChild("Level").GetComponent<Text>();
        level.text = "LV " + player.level;
    }

    protected override void Update()
    {
        //base.Update();
        hp.maxValue = player.MaxHp;
        hp.value = player.Hp;
        hpValue.text = "HP " + hp.value + "/" + hp.maxValue;

        mp.maxValue = player.MaxMp;
        mp.value = player.Mp;
        mpValue.text = "MP " + mp.value + "/" + mp.maxValue;

        //经验条信息加载
        exp.maxValue = player.MaxExp;
        exp.value = player.Exp;
        expValue.text = "Exp " + exp.value + "/" + exp.maxValue;

        level.text = "LV " + player.level;
    }

    //更新经验值
    void UpadatePlayerExp(System.Object obj)
    {
        player.Exp += (int)obj;

        if (player.Exp>=player.MaxExp)
        {
            player.level++;
            player.Exp -= player.MaxExp;
            player.MaxExp += 1000;
            player.MaxHp += 100;
            player.MaxMp += 100;
        }
    }

    void UpadatePlayerMp(System.Object obj)
    {
        player.Mp += (int)obj;
        Mathf.Clamp(player.Mp, 0, player.MaxMp);
    }
}
