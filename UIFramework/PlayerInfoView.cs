using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;

public class PlayerInfoView:BaseView
{
    //人物属性显示
    Text hp;
    Text mp;
    Text atk;
    Text def;
    Text strength;
    Text intelligence;
    Text magicPower;
    Text agility;

    //属性提升按钮
    Button addAtk;
    Button addDef;
    Button addStrength;
    Button addIntelligence;
    Button addMagicPower;
    Button addAgility;


    Player selfPlayer;//人物属性脚本引用

    void Awake()
    {
        hp = transform.FindChild("HP").GetComponent<Text>();
        mp = transform.FindChild("MP").GetComponent<Text>();
     
        atk = transform.FindChild("Atk").GetComponent<Text>();
        addAtk = atk.transform.FindChild("Button").GetComponent<Button>();

        def = transform.FindChild("Def").GetComponent<Text>();
        addDef = def.transform.FindChild("Button").GetComponent<Button>();

        strength = transform.FindChild("Strength").GetComponent<Text>();
        addStrength = strength.transform.FindChild("Button").GetComponent<Button>();

        intelligence = transform.FindChild("Intelligence").GetComponent<Text>();
        addIntelligence = intelligence.transform.FindChild("Button").GetComponent<Button>();

        magicPower = transform.FindChild("MagicPower").GetComponent<Text>();
        addMagicPower = magicPower.transform.FindChild("Button").GetComponent<Button>();

        agility = transform.FindChild("Agility").GetComponent<Text>();
        addAgility = agility.transform.FindChild("Button").GetComponent<Button>();
    }
    protected override void Start()
    {
        base.Start();

        selfPlayer=PlayerInfoModel.Instance.SelectedPlayer;

        hp.text = "HP:" + selfPlayer.Hp + "/" + selfPlayer.MaxHp;

        mp.text = "MP:" + selfPlayer.Mp + "/" + selfPlayer.MaxMp;

        atk.text = "攻击:" + selfPlayer.Atk;
        addAtk.onClick.AddListener(() => selfPlayer.Atk++);

        def.text = "防御:" + selfPlayer.Def;
        addDef.onClick.AddListener(() => selfPlayer.Def++);

        strength.text = "力量:" + selfPlayer.Strength;
        addStrength.onClick.AddListener(() => selfPlayer.Strength++);

        intelligence.text = "智力:" + selfPlayer.Intelligence;
        addIntelligence.onClick.AddListener(() => selfPlayer.Intelligence++);

        magicPower.text = "法强:" + selfPlayer.MagicPower;
        addMagicPower.onClick.AddListener(() => selfPlayer.MagicPower++);

        agility.text = "敏捷:" + selfPlayer.Agility;
        addAgility.onClick.AddListener(() => selfPlayer.Agility++);
    }

    protected override void Update()
    {
        base.Update();

        hp.text = "HP:" + selfPlayer.Hp + "/" + selfPlayer.MaxHp;

        mp.text = "MP:" + selfPlayer.Mp + "/" + selfPlayer.MaxMp;

        atk.text = "攻击:" + selfPlayer.Atk;

        def.text = "防御:" + selfPlayer.Def;

        strength.text = "力量:" + selfPlayer.Strength;

        intelligence.text = "智力:" + selfPlayer.Intelligence;

        magicPower.text = "法强:" + selfPlayer.MagicPower;

        agility.text = "敏捷:" + selfPlayer.Agility;
    }

    void LateUpdate()
    {

    }

    public void OnApplicationQuit()
    {
        PlayerInfoModel.Instance.SavePlayerInfoToXml();
    }

}
