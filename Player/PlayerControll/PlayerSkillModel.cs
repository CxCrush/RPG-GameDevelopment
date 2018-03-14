using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlayerSkillModel:Singleton<PlayerSkillModel>
{
    public PlayerSkillModel()
    {
        Player player = PlayerInfoModel.Instance.SelectedPlayer;

        for (int i = 0; i < 4;i++ )
        {
            PlayerSkill skill = PlayerSkillFactory.GetPlayerSkill(i+1, player);
            skillList.Add(skill);
        }
    }

    //存储已生成的技能效果预制物
    public Dictionary<SkillID, GameObject> skillEffectGoDic = new Dictionary<SkillID, GameObject>();
    
    //技能列表
    public List<PlayerSkill> skillList=new List<PlayerSkill>();
    public GameObject  CreateEffectGo(SkillID path)
    {
        GameObject effectGo=null;
        OccupationType type=(OccupationType)PlayerInfoModel.Instance.SelectedPlayer.occupation;
        string occupation = type.ToString();

        if (skillEffectGoDic.ContainsKey(path))
        {
            effectGo = skillEffectGoDic[path];
            effectGo.SetActive(true);
            Destroy script = effectGo.GetComponent<Destroy>();
            script.ReAwake();
        }

        else
        {
            //生成新的对象
            GameObject effectPrefab = ResourceManager.Instance.Load("Effect_Prefeb/" + occupation+"_"+path.ToString());
            effectGo = GameObject.Instantiate(effectPrefab);
            skillEffectGoDic.Add(path, effectGo);
        }

        return effectGo;
    }

}
