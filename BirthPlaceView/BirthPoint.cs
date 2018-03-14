using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BirthPoint : MonoBehaviour
{

    // Use this for initialization
    void Awake()
    {
        //实例化玩家
        Player selectedPlayer=PlayerInfoModel.Instance.SelectedPlayer;
        GameObject player=Instantiate(PlayerInfoModel.Instance.playerBody[selectedPlayer.occupation,selectedPlayer.sex]);
        player.transform.position = transform.position;

        //添加组件和脚本
        Rigidbody body=player.AddComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.FreezeRotation;
   
        player.AddComponent<NavMeshAgent>().baseOffset=-0.15f;
        player.GetComponent<NavMeshAgent>().stoppingDistance = 0.5f;

        player.AddComponent<PlayerController>();
        //设置数据
        Transform headBar = player.transform.FindChild("PlayerHeadBar");
        PlayerHeadBar headBarScript = headBar.GetComponent<PlayerHeadBar>();
        headBarScript.SetData(selectedPlayer);

        //设置相机目标
        GameObject mainCamera = Camera.main.gameObject;
        ThirdPersonCamera script = mainCamera.GetComponent<ThirdPersonCamera>();
        script.target = player.transform;
    }
    void Start()
    {
      
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
