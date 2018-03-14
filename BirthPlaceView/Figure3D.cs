using UnityEngine;
using System.Collections;

public class Figure3D : MonoBehaviour {

	// Use this for initialization
	void Start () {

        Player player = PlayerInfoModel.Instance.SelectedPlayer;

        //实例化3D模型
        GameObject go = Instantiate(PlayerInfoModel.Instance.playerBody[player.occupation, player.sex]);
        go.transform.position = transform.position;

        //设置相机目标
        //GameObject figure3DCamera = GameObject.Find("Figure3DCamera");
        //ThirdPersonCamera script = figure3DCamera.GetComponent<ThirdPersonCamera>();
        //script.target = go.transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
