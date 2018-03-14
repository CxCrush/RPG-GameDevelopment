using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MonsterGenerater : MonoBehaviour {

    public MonsterID id;  //生成的怪物类型ID

    public int num=5;  //怪物数量
    public float radius = 10;  //区域半径
    List<GameObject>  monsterList; //怪物容器
    GameObject monsterManager;  //怪物管理

    GameObject player;  //目标玩家


    bool monsterClear=true;  //怪物是否完全清除
	// Use this for initialization
	void Start () {

        monsterManager = new GameObject();
        monsterManager.name = "[Monsters]";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && monsterClear)
        {
            player = other.gameObject;
            monsterClear = false;

            GenerateMonster();
        }
    }

    void Check()
    {
        int count = 0;

        for (int i = 0; i < monsterList.Count;i++ )
        {
            if (monsterList[i]==null)
            {
                count++;
            }
        }

        if (count == monsterList.Count)
        {
            monsterList.Clear();
            monsterClear = true;
        }
    }
   
    void GenerateMonster()
    {
        //生成5个怪
        for (int i = 0; i < num;i++)
        {
            //水平位置
            Vector3 dir  = new Vector3(Random.Range(-1f, 1f),0,Random.Range(-1f, 1f));
            float rate = Random.Range(0, 1f);
            Vector3 horizontalPos = dir * radius * rate;

            //竖直位置
            Vector3 verticalPos = horizontalPos + Vector3.up * 100;
            Ray ray = new Ray(verticalPos, Vector3.down);

            Vector3 desPos = horizontalPos;

            RaycastHit hit;

            if (Physics.Raycast(ray,out hit,150))
            {
                if (hit.collider.tag=="Ground")
                {
                    desPos = hit.point;
                }
            }

            //实例化怪物
            int index=(int)id;
            GameObject monster = Instantiate(MonsterModel.Instance.monsterPrefabList[index]);
            monster.transform.position = desPos;
            monster.transform.SetParent(monsterManager.transform);

            //设置数据
            MonsterController script = monster.GetComponent<MonsterController>();
            Monster monsterData=MonsterModel.Instance.monsterList[index].Clone();
            script.SetData(monsterData);
        }
    }
}
