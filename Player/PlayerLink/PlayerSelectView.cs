using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
public class PlayerSelectView : MonoBehaviour
{

    const int maxNum = 3;

    Toggle[] headImgGrid = new Toggle[maxNum];  //上限三个角色格子

    Sprite[,] headImg;
    GameObject[,] playerBody;

    Transform stage; //角色站立台;
   
    int characterCount; //已创建角色数量
    string[] occupationName;

    Button btn_Enter;  //进入游戏按钮
    Button btn_Create;  //创建角色按钮
    Button btn_Delete;  //删除角色按钮

    Image processBg;  //加载进度背景
    Slider process;  //加载进度条

    int index = 0; //当前选中的角色头像下标
    GameObject curBodyGo; //当前显示的角色模型对象
    int lastIndex =-1; //上一个选中的角色下标
    List<GameObject> playerBodyGoList = new List< GameObject>();

    CanvasGroup selfCanvasGroup; //自己的canvasGroup组件
    CanvasGroup otherCanvasGroup; //创建界面的canvas组件

	// Use this for initialization

    void Awake()
    {
        selfCanvasGroup = GetComponent<CanvasGroup>();

        btn_Enter = transform.FindChild("Btn_Enter").GetComponent<Button>();
        btn_Enter.onClick.AddListener(OnEnter);

        btn_Create = transform.FindChild("Btn_Create").GetComponent<Button>();
        btn_Create.onClick.AddListener(OnCreate);

        btn_Delete = transform.FindChild("Btn_Delete").GetComponent<Button>();
        btn_Delete.onClick.AddListener(OnDelete);

        //加载进度背景
        processBg = transform.FindChild("ProcessBg").GetComponent<Image>();
        processBg.enabled = false;

        //加载进度条
        process = transform.FindChild("Process").GetComponent<Slider>();
        process.gameObject.SetActive(false);

    }
	void Start () {
        GameObject createView = GameObject.Find("CreateView");
        otherCanvasGroup = createView.GetComponent<CanvasGroup>();
        otherCanvasGroup.alpha = 0;
       
        headImg = PlayerInfoModel.Instance.headImg;
        playerBody = PlayerInfoModel.Instance.playerBody;

        occupationName = PlayerInfoModel.Instance.occupationName;

        //角色模型显示位置
        stage = GameObject.Find("Stage").transform;

        //加载已创建角色
        Init();
        LoadCreatedCharacter();
        
        MyEventSystem.AddListener(EventsNames.updateCharacter, UpdateCharacter);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void  Init()
    {
        characterCount = PlayerInfoModel.Instance.characterInfoList.Count;
        //实例化已创建角色
        for (int i = 0; i < characterCount;i++ )
        {
            Player info = PlayerInfoModel.Instance.characterInfoList[i];
            GameObject playerBodyGo = Instantiate(playerBody[info.occupation, info.sex]);
            playerBodyGo.transform.position = stage.position + Vector3.up * 0.3f;
            playerBodyGo.transform.SetParent(stage);
            //设置数据
            Transform headBar = playerBodyGo.transform.FindChild("PlayerHeadBar");
            PlayerHeadBar script = headBar.GetComponent<PlayerHeadBar>();
            script.SetData(info);
          
            playerBodyGoList.Add(playerBodyGo);
            playerBodyGo.SetActive(false);
        }
    }

    void UpdateCharacter()
    {
        characterCount = PlayerInfoModel.Instance.characterInfoList.Count;

        Player info = PlayerInfoModel.Instance.characterInfoList[characterCount-1];
        GameObject playerBodyGo = Instantiate(playerBody[info.occupation, info.sex]);
        playerBodyGo.transform.position = stage.position + Vector3.up * 0.3f;
        playerBodyGo.transform.SetParent(stage);

        //设置数据
        Transform headBar = playerBodyGo.transform.FindChild("PlayerHeadBar");
        PlayerHeadBar script = headBar.GetComponent<PlayerHeadBar>();
        script.SetData(info);

        playerBodyGoList.Add(playerBodyGo);
        playerBodyGo.SetActive(false);

        LoadCreatedCharacter();
    }
    void LoadCreatedCharacter()
    {
        characterCount=PlayerInfoModel.Instance.characterInfoList.Count;

        //根据已创建角色数量更新按钮状态
        btn_Enter.interactable=characterCount>0;
        btn_Delete.interactable = characterCount > 0;
        btn_Create.interactable = characterCount < maxNum;
   
        //找到三个头像图片格子
        for (int i = 0; i < maxNum; i++)
        {
            Transform grid = transform.FindChild("CharacterGrid" + i);
            Image lockImg = grid.GetChild(0).GetComponent<Image>();
            headImgGrid[i] = grid.GetChild(1).GetComponent<Toggle>();
           
            //锁图激活
            lockImg.gameObject.SetActive(true);

            //更新格子图片和信息
            Image backGround = headImgGrid[i].transform.FindChild("Background").GetComponent<Image>();
            Text label = headImgGrid[i].transform.FindChild("Label").GetComponent<Text>();

            //图片和文字初始状态为禁用
            headImgGrid[i].gameObject.SetActive(false);
           
            //已有角色创建
            if (i<characterCount)
            {
                //获取角色信息
                string name = PlayerInfoModel.Instance.characterInfoList[i].name;
                int level = PlayerInfoModel.Instance.characterInfoList[i].level;
                int occupation = PlayerInfoModel.Instance.characterInfoList[i].occupation;
                int server = PlayerInfoModel.Instance.characterInfoList[i].server;
                int sex = PlayerInfoModel.Instance.characterInfoList[i].sex;

                //激活显示图片
                headImgGrid[i].gameObject.SetActive(true);

                backGround.sprite = headImg[occupation, sex];

                Image checkMark = backGround.transform.GetChild(0).GetComponent<Image>();
                checkMark.sprite = headImg[occupation, sex];

                label.text = "昵称：" + name + "\n" + "等级：LV" + level + "\n" + "职业：" + occupationName[occupation]
                    + "\n" + "电信" + server + "区";

                //默认第一张被选中
                if (i == 0)
                {
                    headImgGrid[i].isOn = true;
                    IsOn(true);
                }

                else
                {
                    headImgGrid[i].isOn = false;
                }

                headImgGrid[i].onValueChanged.AddListener(IsOn);

                lockImg.gameObject.SetActive(false); //禁用锁图
            }

            else
            {
                headImgGrid[i].isOn = false;
            }
        }

    }

    void IsOn(bool isOn)
    {
        characterCount=PlayerInfoModel.Instance.characterInfoList.Count;
      
        for (int i = 0; i < characterCount;i++)
        {
            if (headImgGrid[i].isOn)
            {
                index = i;

                //在角色显示台显示目前选中的角色模型
                //之前显示对象模型禁用

                if (0 <= lastIndex && lastIndex < playerBodyGoList.Count)
                {
                    playerBodyGoList[lastIndex].SetActive(false);
                }

                //已经实例化，（激活）
                playerBodyGoList[index].SetActive(true);
                //更新摄像机目标
                GameObject mainCamera=Camera.main.gameObject;
                ThirdPersonCamera script = mainCamera.GetComponent<ThirdPersonCamera>();
                script.target = playerBodyGoList[index].transform;

                lastIndex = index;

                break;
            }
        }
    }

    void OnEnter()
    {
        //记录选择的角色信息
        PlayerInfoModel.Instance.SelectedIndex = index;
        PlayerInfoModel.Instance.SelectedPlayer = PlayerInfoModel.Instance.characterInfoList[index];
        //打开进度背景图和进度条
        processBg.enabled = true;
        process.gameObject.SetActive(true);
        process.value =SceneManager.LoadSceneAsync(SceneNames.birthPlaceView).progress;

    }

    void OnCreate()
    {
        
        //打开创建界面
        otherCanvasGroup.alpha = 1;
        otherCanvasGroup.blocksRaycasts = true;
        //关闭选择界面
        selfCanvasGroup.alpha = 0;
        selfCanvasGroup.blocksRaycasts = false;

        //禁用当前选中对象模型
        if (playerBodyGoList.Count>0)
        {
            playerBodyGoList[index].SetActive(false);
        }
    }

    void OnDelete()
    {
        PlayerInfoModel.Instance.RemoveCharacter(index);

        Destroy(playerBodyGoList[index]);
        playerBodyGoList[index] = null;
        playerBodyGoList.RemoveAt(index);

        //重新加载角色信息
        LoadCreatedCharacter();
    }
}
