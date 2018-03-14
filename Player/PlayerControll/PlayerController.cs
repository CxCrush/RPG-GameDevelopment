using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

//玩家状态
public enum PlayerState
{
    Idle=1,Walk,Run,Attack1,Attack2,Attack3,Attack4,Hit,Dodge,Defence,Die,AutoWalk
}
public class PlayerController : MonoBehaviour
{

    public float speed = 2;
    public float deadZone = 0.5f;
    public float angularSpeed = 20;
    public float attackRadius = 10;

    PlayerState selfState;
    Player selfPlayer;

    Animator animator;
    NavMeshAgent agent;

    Vector3 birthPos;  //出生点
    Vector3 desPos;//鼠标点击目标点位置
    GameObject target;//目标人物或攻击对象

    Text targetWarning;  //目标不明确提示
    Text distanceWarning;  //目标距离太远提示

    bool isPlayerSkillPlaying = false;
    //技能队列
    Queue skillQueue=new Queue();
    float timer = 0;  //计时器,获取技能释放动画时间,以使技能能够顺利释放完成

    
    //生成技能效果预制物
    GameObject effectGo = null;
    GameObject skillEffectManager;//技能 效果对象管控
    //点击效果预制物
    GameObject clickEffectGo;


	// Use this for initialization
    void Awake()
    {
        animator = transform.GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        birthPos = transform.position;
       
    }
	void Start () 
    {
        //初始状态为待机状态
        selfState = PlayerState.Idle;
        selfPlayer = PlayerInfoModel.Instance.SelectedPlayer;
        //更新玩家数据模型中的位置信息
        selfPlayer.pos = transform.position;

        targetWarning = GameObject.Find("Canvas").transform.FindChild("TargetWarning").GetComponent<Text>();
        targetWarning.enabled = false;

        distanceWarning = GameObject.Find("Canvas").transform.FindChild("DistanceWarning").GetComponent<Text>();
        distanceWarning.enabled = false;


        MyEventSystem.AddListener(EventsNames.addPlayerSkillTrigger, AddPlayerSkillTrigger);
        MyEventSystem.AddListener(EventsNames.setPlayerAttackTarget, SetPlayerAttackTarget);
        MyEventSystem.AddListener(EventsNames.updatePlayerHp, UpdatePlayerHp);
	}
	
	// Update is called once per frame
	void Update () {

        //更新玩家数据模型中的位置信息
        selfPlayer.pos = transform.position;

        //释放技能
        PlayerSkillTrigger();

        if (Input.GetMouseButtonDown(0))
        {
            //射线和UI碰撞
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.tag == "Ground")
                {
                    desPos = hit.point;

                    //生成点击效果预制物
                    if (clickEffectGo==null)
                    {
                        GameObject go = ResourceManager.Instance.Load("Effect_Prefeb/Efx_Click_Red");
                        clickEffectGo = Instantiate(go);
                    }
                 
                    clickEffectGo.transform.position = hit.point;
                    
                    agent.Resume();
                    agent.destination = hit.point;
                    //停止技能释放
                    //skillQueue.Clear();
                    selfState = PlayerState.AutoWalk;
                    MyEventSystem.Dispatch(EventsNames.cannotRotatePlayer);
                }

                else if (hit.collider.tag=="NPC01")
                {
                    UIManager.Instance.LoadUI(UIPanelType.chat);
                }

                else if (hit.collider.tag == "Monster")
                {
                    target = hit.collider.gameObject;
                }
            }
        }

        else 
        {
          
            Check();

        }

        if (selfState==PlayerState.AutoWalk)
        {
            animator.SetInteger(AnimatorHashIds.playserState, (int)PlayerState.Run);
            AutoMove(speed * 1.5f);
        }
        else
        animator.SetInteger(AnimatorHashIds.playserState, (int)selfState);

        //重置状态
        ResetState();
           
	}

    void LateUpdate()
    {
        if (effectGo!=null&&target!=null)
        {
            if (Vector3.Distance(target.transform.position,effectGo.transform.position)>=0.5f)
            {
                effectGo.transform.position += effectGo.transform.forward * speed * 5.0f * Time.deltaTime;
            }
            
        }

    }
    void Check()
    {

        #region 行走

        if (Input.GetKey(KeyCode.W))
        {
            //停止自动寻路和技能释放
            agent.Stop();
            transform.position += transform.forward * speed * Time.deltaTime;
            selfState = PlayerState.Walk;
            MyEventSystem.Dispatch(EventsNames.canRotatePlayer);

        }

        else if (Input.GetKey(KeyCode.S))
        {
            //停止自动寻路和技能释放
            agent.Stop(); 

            transform.position -= transform.forward * speed * Time.deltaTime;
            selfState = PlayerState.Walk;
            MyEventSystem.Dispatch(EventsNames.canRotatePlayer);
        }

        else if (Input.GetKey(KeyCode.A))
        {
            //停止自动寻路和技能释放
            agent.Stop();
            transform.position -= transform.right * speed * Time.deltaTime;
            transform.Rotate(-Vector3.up * angularSpeed * Time.deltaTime);
            selfState = PlayerState.Walk;
            MyEventSystem.Dispatch(EventsNames.canRotatePlayer);
        }

        else if (Input.GetKey(KeyCode.D))
        {
            //停止自动寻路和技能释放
            agent.Stop();
            transform.position += transform.right * speed * Time.deltaTime;
            selfState = PlayerState.Walk;
            MyEventSystem.Dispatch(EventsNames.canRotatePlayer);
        }

        #endregion
      
        #region 奔跑
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.LeftShift))
        {
            //停止自动寻路和技能释放
            agent.Stop();
            transform.position += transform.forward * speed * 1.5f * Time.deltaTime;
            selfState = PlayerState.Run;
            MyEventSystem.Dispatch(EventsNames.canRotatePlayer);
        }

        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.LeftShift))
        {
            //停止自动寻路和技能释放
            agent.Stop();
            transform.position -= transform.forward * speed * 1.5f * Time.deltaTime;
            selfState = PlayerState.Run;
            MyEventSystem.Dispatch(EventsNames.canRotatePlayer);
        }

        #endregion

    }

    //受伤
    void UpdatePlayerHp(System.Object obj)
    {
        if (selfState==PlayerState.Attack1||
            selfState == PlayerState.Attack2||
            selfState == PlayerState.Attack3||
            selfState == PlayerState.Attack4)
        {
            return;
        }

        selfState = PlayerState.Hit;

        int hpChange = (int)obj;

        if (selfPlayer==null)
        {
            selfPlayer = PlayerInfoModel.Instance.SelectedPlayer;
        }

        selfPlayer.Hp += hpChange;

        if (selfPlayer.Hp<=0)
        {
            selfState = PlayerState.Die;
        }
    }

    //死亡
    public void DealWithDeath()
    {
        //回到出生点
        transform.position = birthPos;
        selfPlayer.Hp = selfPlayer.MaxHp;
        selfPlayer.Mp = selfPlayer.MaxMp;

        selfState = PlayerState.Idle;
        if (agent!=null)
        {
            agent.destination = birthPos;
        }
    

    }
    //添加技能进入技能队列
    void AddPlayerSkillTrigger(System.Object obj)
    {
        //停止自动寻路*-
        if (agent!=null)
        {
            agent.Stop();
        }
      
        if (target != null)
        {
            //距离太远,打不到
            if (Vector3.Distance(target.transform.position,transform.position)>attackRadius)
            {
                distanceWarning.enabled = true;
                return;
            }
            else
            {
                transform.LookAt(target.transform);
                targetWarning.enabled = false;
                distanceWarning.enabled = false;
            }
          
        }

        else if (targetWarning!=null)
        {
            targetWarning.enabled = true;
        }
            

        PlayerState state = (PlayerState)obj;
        if (skillQueue==null)
        {
            skillQueue = new Queue();
        }

        skillQueue.Enqueue(state);
    }
 
    //播放技能
    void PlayerSkillTrigger()
    {
        if (!isPlayerSkillPlaying)
        {
            if (skillQueue.Count>0)
            {
                PlayerState state = (PlayerState)skillQueue.Dequeue();
                selfState = state;
            }
        }
    }

    public void CreatePlayerSkillEffect()
    {
        switch (selfState)
        {
            case PlayerState.Attack1:
                effectGo=PlayerSkillModel.Instance.CreateEffectGo(SkillID.Skill1);
                break;
            case PlayerState.Attack2:
                effectGo=PlayerSkillModel.Instance.CreateEffectGo(SkillID.Skill2);
                break;
            case PlayerState.Attack3:
                effectGo=PlayerSkillModel.Instance.CreateEffectGo(SkillID.Skill3);
                break;
            case PlayerState.Attack4:
                effectGo=PlayerSkillModel.Instance.CreateEffectGo(SkillID.Skill4);
                break;
            default:
                break;
        }

        SetPlayerSkillEffectData(effectGo);
    }

    //设置技能效果对象属性
    void SetPlayerSkillEffectData(GameObject skillEffecGo)
    {
        if (skillEffecGo!=null)
        {

            //怪物掉血
            int i = (int)selfState;
            if (0 <= i - 4 && i - 4 < PlayerSkillModel.Instance.skillList.Count)
            {
                MyEventSystem.Dispatch(EventsNames.updateMonsterHp, PlayerSkillModel.Instance.skillList[i - 4].Damage);
            }

            if (target != null)
            {
                skillEffecGo.transform.position = target.transform.position + Vector3.up;
                skillEffecGo.transform.LookAt(target.transform);
            }
            else
                skillEffecGo.transform.position = transform.position + transform.forward;

            if (skillEffectManager == null)
            {
                skillEffectManager = new GameObject();
                skillEffectManager.name = "[PlayerSkillTriggerEffects]";
            }

            if (skillEffecGo.transform.parent != skillEffectManager.transform)
            {
                skillEffecGo.transform.SetParent(skillEffectManager.transform);
            }
        }
    }

    void SetPlayerAttackTarget(System.Object obj)
    {
        target = obj as GameObject;
    }
    void AutoMove(float _speed)
    {
        if (Vector3.Distance(desPos,transform.position) <= deadZone)
        {
            selfState = PlayerState.Idle;

            if (clickEffectGo!=null)
            {
                Destroy(clickEffectGo);
            }
        }
        
        else
        {
            agent.destination = desPos;
        }
    }

   
    void ResetState()
    {
        
        switch (selfState)
        {
            case PlayerState.Idle:
                break;
            case PlayerState.Walk:
            case PlayerState.Run:
                isPlayerSkillPlaying = false;
                timer += Time.deltaTime;
            if (timer >= animator.GetCurrentAnimatorStateInfo(0).length/4)
            {
                timer = 0;
                selfState = PlayerState.Idle;
            }
                break;
            case PlayerState.Attack1:
            case PlayerState.Attack2:
            case PlayerState.Attack3:
            case PlayerState.Attack4:
                 timer += Time.deltaTime;
                 if (timer >= animator.GetCurrentAnimatorStateInfo(0).length)
                 {
                    timer = 0;
                    isPlayerSkillPlaying = false;
                    if (skillQueue.Count == 0)
                    {
                        selfState = PlayerState.Idle;
                        animator.CrossFade(selfState.ToString(), 0.2f, 0);
                    }
                 }
                 else
                     isPlayerSkillPlaying = true;
                break;
            case PlayerState.Hit:
            case PlayerState.Dodge:
            case PlayerState.Defence:
                isPlayerSkillPlaying = false;
                timer += Time.deltaTime;
            if (timer >= animator.GetCurrentAnimatorStateInfo(0).length)
            {
                timer = 0;
                selfState = PlayerState.Idle;
            }
                break;
            case PlayerState.Die:
                break;
            case PlayerState.AutoWalk:
                isPlayerSkillPlaying = false;
                return;
            default:
                break;
        }

        if (selfState == PlayerState.Idle)
        {
            MyEventSystem.Dispatch(EventsNames.cannotRotatePlayer);
        }
    }

    public void OnDestroy()
    {
        if (clickEffectGo!=null)
        {
            Destroy(clickEffectGo);
        }
    
    }
 
}
