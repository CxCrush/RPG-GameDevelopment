using UnityEngine;
using System.Collections;

public enum MonsterState
{
    idle = 1, walk, chase, attack1, attack2, attack3,hit,die,back
}
public class MonsterController : MonoBehaviour {

    public float patrollRadius = 10;//巡逻半径
    public float sightRadius = 10;//视野半径
    public float listenRadius = 15;//听觉半径
    public float deadZone = 0.5f;
    public float halfSightAngle = 60;//视角半角
    public float attackRadius = 2; //攻击半径
    public float patrollSpeed = 2f;
    public float chaseSpeed = 3f;
    public MonsterID id;   //怪物id

    GameObject[] players; //当前场景所有玩家

    Transform targetPlayer;//玩家目标

    public Transform TargetPlayer
    {
        get
        {
            GameObject player = FindTargetPlayer(patrollRadius);
            if (player != null)
            {
                targetPlayer = player.transform;
            }
          
            return targetPlayer; 
        }

        set 
        {
            targetPlayer = value; 
        }
    }

    //怪物数据模型脚本引用
    Monster selfMonster;

    MonsterState selfState;
    Vector3 originalPos;
    NavMeshAgent agent;
    Animator animator;

    float timer = 0;
    public float waitTime = 3;
    
	// Use this for initialization
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
	void Start () {
        selfState = MonsterState.walk;

        originalPos = transform.position;
        newDestination();
        SetData(null);
        MyEventSystem.AddListener(EventsNames.updateMonsterHp, UpdateMonsterHp);
	}
	
	// Update is called once per frame
	void Update () {

        Check();

        switch (selfState)
        {
            case MonsterState.idle:
                Idle();
                break;
            case MonsterState.walk:
                Walk();
                break;
            case MonsterState.chase:
                Chase();
                break;
            case MonsterState.back:
                Back();
                break;
            case MonsterState.attack1:
            case MonsterState.attack2:
            case MonsterState.attack3:
                Attack();
                break;
            case MonsterState.hit:
                break;
            case MonsterState.die:
                break;
            default:
                break;
        }

        if (selfState==MonsterState.back)
        {
            animator.SetInteger(AnimatorHashIds.monsterState, (int)MonsterState.walk);
        }
        else
        animator.SetInteger(AnimatorHashIds.monsterState, (int)selfState);

	}

    //设置数据
    public void SetData(Monster monster)
    {
        //设置怪物属性模型
        //selfMonster = monster.Clone();
        selfMonster = MonsterModel.Instance.monsterList[(int)id].Clone();
        Transform hpbar = transform.FindChild("MonsterHeadBar");
        MonsterHeadBar script = hpbar.GetComponent<MonsterHeadBar>();
        script.SetData(selfMonster);
    }
    void Check()
    {
        //死亡
        if (selfMonster!=null&&selfMonster.hp<=0)
        {
            selfState = MonsterState.die;
            return;
        }

        if (selfState==MonsterState.chase||selfState==MonsterState.back||
            selfState == MonsterState.attack1 || selfState == MonsterState.attack2
            || selfState == MonsterState.attack3)
        {
            return;
        }
       
        if (TargetPlayer!=null)
        {
            //看见目标玩家

            //在视野范围内
            Vector3 offset = TargetPlayer.position - transform.position;
            if (offset.magnitude<=sightRadius)
            {
                float angle = Vector3.Angle(transform.forward, offset);

                //在视角内
                if (angle<halfSightAngle)
                {
                    //中间没有障碍物
                    Ray ray = new Ray(transform.position+Vector3.up, offset);
                    RaycastHit hit;
                    if (Physics.Raycast(ray,out hit,sightRadius))
                    {
                        if (hit.collider.tag=="Player")
                        {
                            //状态改为追击玩家
                            selfState = MonsterState.chase;
                            return;
                        }
                       
                    }
                }
            }
            
            //听到玩家(计算路径长度是否小于听觉半径)
            NavMeshPath path = new NavMeshPath();
            agent.CalculatePath(targetPlayer.position, path);

            Vector3[] allPoints = new Vector3[path.corners.Length + 2];
            allPoints[0] = transform.position;
            allPoints[allPoints.Length - 1] = targetPlayer.position;

            for (int i = 1; i < allPoints.Length - 1; i++)
            {
                allPoints[i] = path.corners[i - 1];
            }

            float length = 0;
            for (int i = 0; i < allPoints.Length-1;i++ )
            {
                length += Vector3.Distance(allPoints[i], allPoints[i + 1]);
            }

            if (length < listenRadius)
            {
                //状态改为追击玩家
                selfState = MonsterState.chase;
            }
        }
    }

    void Walk()
    {
        if (agent.remainingDistance<=deadZone)
        {
            selfState = MonsterState.idle;
        }
    }

    void Chase()
    {
        agent.destination = TargetPlayer.position;
        agent.speed = chaseSpeed;

        //超出巡逻范围,返回
        if (Vector3.Distance(originalPos,transform.position)>=patrollRadius)
        {
            selfState = MonsterState.back;
            TargetPlayer = null;

            agent.SetDestination(originalPos);
            agent.speed = patrollSpeed;
         
            return;
        }

        if (agent.remainingDistance <= attackRadius)
        {
            selfState = MonsterState.attack1;
            agent.Stop();
        }
    }
    
    void Back()
    {
        if (agent.remainingDistance<=deadZone)
        {
            selfState = MonsterState.idle;
        }
    }
    void Attack()
    {
        //agent.Stop();  //停止导航
        transform.LookAt(targetPlayer);

        if (TargetPlayer != null)
        {
            if (Vector3.Distance(TargetPlayer.position, transform.position) >= attackRadius)
            {
                agent.Resume();
                print("重新追击!");
                selfState = MonsterState.chase;
            }
        }
    }

    public void DoDamageToPlayer()
    {
        if (TargetPlayer!=null&&selfMonster!=null)
        {
            PlayerController script = TargetPlayer.GetComponent<PlayerController>();
            MyEventSystem.Dispatch(EventsNames.updatePlayerHp, -selfMonster.atk);
        }
    }
    void Idle()
    {
        timer += Time.deltaTime;
        if (timer>=waitTime)
        {
            timer = 0;
            newDestination();
            selfState = MonsterState.walk;
        }

    }

    public void DealWithDeath()
    {
        if(selfMonster==null)
        {
            return;
        }

        MyEventSystem.Dispatch(EventsNames.updatePlayerExp, selfMonster.maxHp);
        MyEventSystem.Dispatch(EventsNames.completeTask);

        Destroy(gameObject);
    }
    void UpdateMonsterHp(System.Object obj)
    {
        if (selfMonster==null||selfState!=MonsterState.attack1)
        {
            return;
        }

        int damage = (int)obj;

        selfMonster.hp -= damage;
        selfState = MonsterState.hit;

        if (selfMonster.hp <= 0)
        {
            selfState = MonsterState.die;
        }
    }
    Vector3 newDestination()
    {
        Vector3 randDir = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        float rate = Random.Range(0f, 1f);
        Vector3 newDestPos =originalPos+ patrollRadius * rate * randDir;

        Vector3 topPos=newDestPos+Vector3.up*1000;

        Ray ray=new Ray(newDestPos,Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray,out hit,1100,1<<8))
        {
            if (hit.collider.tag=="Ground")
            {
                newDestPos = hit.point;
            }
        }

        agent.destination = newDestPos;
        return newDestPos;
    }

    GameObject FindTargetPlayer(float radius)
    {
        if (players==null)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            if (players==null)
            {
                return null;
            }
        }

        GameObject player = null;

        float minDistance = radius;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i]!=null)
            {
                float distance=Vector3.Distance(players[i].transform.position,transform.position);
                if (distance<minDistance)
                {
                    minDistance = distance;
                    player = players[i];
                }
            }
        }

        return player;
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(originalPos, patrollRadius);
//         Gizmos.color = Color.cyan;
//         Gizmos.DR(transform.position,);
    }


}
