using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ChuanSongMen1 : MonoBehaviour
{
    //进度条
    GameObject canvas;
    Slider process;
	// Use this for initialization
	void Start () {
	
        canvas=GameObject.Find("Canvas");
        process = canvas.transform.FindChild("Process").GetComponent<Slider>();
        process.wholeNumbers = true;
        process.value = 0;

        process.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag=="Player")
        {
            Invoke("ChuansSong", 2);
        }
    }

    void ChuansSong()
    {
        //process.gameObject.SetActive(true);
        //process.value=SceneManager.LoadSceneAsync(SceneNames.gameScene1).progress;

        DontDestroyOnLoad(canvas);
        GameObject skillEffects = GameObject.Find("[PlayerSkillTriggerEffects]");
        DontDestroyOnLoad(skillEffects);
        //StartCoroutine(Loading());
    }
    
    IEnumerator Loading()
    {
        while (true)
        {
            //if(process.value>=1.0f)
            //{
            //    process.gameObject.SetActive(false);
            //    break;
            //}
            yield return null;
        }
    }
}
