using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlayerCreateView : MonoBehaviour
{
    

    Toggle[] headImgGrid;

    Sprite[,] headImg;
    GameObject[,] playerBody;

    InputField nickName;  //名字输入框
    Button btn_Create;  //创建按钮
    Dropdown serverList; //服务器列表

    GameObject curBodyGo; //当前选择的对象模型
    int index = 0;  //当前选择的下标
    int lastIndex = 0; //之前选择的下标

    Transform stage; //角色模型显示平台
    GameObject allCharacters;//所有可选角色父物体
    Dictionary<int, GameObject> playerBodyGoList = new Dictionary<int, GameObject>();//管控所有角色的容器

    CanvasGroup selfCanvasGroup; //自己的canvasGroup组件
    CanvasGroup otherCanvasGroup; //选择界面的canvas组件

	// Use this for initialization

    void Awake()
    {
        btn_Create = transform.FindChild("Btn_Create").GetComponent<Button>();
        btn_Create.onClick.AddListener(OnCreateCharacter);
        btn_Create.interactable = false;

        nickName = transform.FindChild("NickName").GetComponent<InputField>();
        nickName.onValueChanged.AddListener(Check);

        serverList = transform.FindChild("ServerList").GetComponent<Dropdown>();

        //初始状态为隐藏
        selfCanvasGroup = GetComponent<CanvasGroup>();
        selfCanvasGroup.alpha = 0;
        selfCanvasGroup.blocksRaycasts = false;
    }
	void Start () {

        allCharacters = new GameObject();
        allCharacters.name = "[Characters]";

        GameObject selectView = GameObject.Find("SelectView");
        otherCanvasGroup = selectView.GetComponent<CanvasGroup>();

        stage = GameObject.Find("Stage").transform;

        headImg = PlayerInfoModel.Instance.headImg;
        playerBody = PlayerInfoModel.Instance.playerBody;

        Init();
	}
	
	// Update is called once per frame
	void Update () {

        if (curBodyGo != null)
        {
            curBodyGo.SetActive(selfCanvasGroup.alpha != 0);
        }
           
	}

    void Init()
    {
        headImgGrid = new Toggle[headImg.GetLength(0)*2];

        for (int i = 0; i < headImgGrid.Length;i++ )
        {
            Transform grid = transform.FindChild("CharacterGrid" + i);
            headImgGrid[i] = grid.GetComponent<Toggle>();

            Image background = headImgGrid[i].transform.FindChild("Background").GetComponent<Image>();
            Image checkmark = background.transform.GetChild(0).GetComponent<Image>();
            background.sprite = headImg[i / headImg.GetLength(1), i % headImg.GetLength(1)];
            checkmark.sprite = headImg[i / headImg.GetLength(1), i % headImg.GetLength(1)];

            //默认选中第一个
           if (i==0)
           {
               headImgGrid[i].isOn = true;
               IsOn(true);
           }

           else
           {
               headImgGrid[i].isOn = false;
           }

           headImgGrid[i].onValueChanged.AddListener(IsOn);
        }
    }

    void Check(string content)
    {
        btn_Create.interactable = !string.IsNullOrEmpty(nickName.text);
    }

    void IsOn(bool isOn)
    {
        for (int i = 0; i < headImgGrid.Length; i++)
        {
            if (headImgGrid[i].isOn)
            {
                index = i;

                //之前显示对象模型禁用
                if (curBodyGo != null)
                {
                    if (lastIndex == index)
                    {
                        return;
                    }
                    else
                        curBodyGo.SetActive(false);
                }

                //在角色显示台显示目前选中的角色模型

                //已经实例化，（激活）
                if (playerBodyGoList.ContainsKey(index))
                {
                    curBodyGo = playerBodyGoList[index];
                    curBodyGo.SetActive(true);
                }

                //还未实例化
                else
                {
                    GameObject playerBodyGo = Instantiate(playerBody[i / headImg.GetLength(1), i % headImg.GetLength(1)]);
                    playerBodyGo.transform.position = stage.position + Vector3.up * 0.3f;
                    playerBodyGo.transform.SetParent(allCharacters.transform);
                    curBodyGo = playerBodyGo;

                    //设置属性
                    Player player = PlayerFactory.GeneratePlayer((OccupationType)(i / headImg.GetLength(1)), "");
                    Transform headBar = curBodyGo.transform.FindChild("PlayerHeadBar");
                    PlayerHeadBar headBarScript = headBar.GetComponent<PlayerHeadBar>();
                    headBarScript.SetData(player);
                    //加进容器
                    playerBodyGoList.Add(index, curBodyGo);
                }

                //更新摄像机目标
                GameObject mainCamera = Camera.main.gameObject;
                ThirdPersonCamera script = mainCamera.GetComponent<ThirdPersonCamera>();
                script.target = curBodyGo.transform;

                lastIndex = index;

                break;
            }
        }
    }

    void OnCreateCharacter()
    {
        //回到选择界面
        otherCanvasGroup.alpha = 1;
        otherCanvasGroup.blocksRaycasts = true;
        //关闭创建界面
        selfCanvasGroup.alpha = 0;
        selfCanvasGroup.blocksRaycasts = false;

        if (curBodyGo!=null)
        {
            curBodyGo.SetActive(false);
        }

        PlayerInfoModel.Instance.AddCharacter(nickName.text, index/headImg.GetLength(1), serverList.value+1,index%2);
    }
}
