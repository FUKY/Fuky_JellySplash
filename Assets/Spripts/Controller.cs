using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Controller : MonoBehaviour {
	
    private LoadImageController loadImageControll;
    private Timer time;
    

    public Text nextLevel;
    public GameObject panel;

    private float timeDelay = 3;
    private float delay;
    

    void Awake()
    {
        
        //diemDatDuoc = new int[5] { 1000, 2000, 3000, 4000, 5000 };
        
        loadImageControll = gameObject.GetComponent<LoadImageController>();
        time = GameObject.Find("Time").GetComponent<Timer>();
        if (time == null)
        {
            Debug.Log("k tim thay Timer");
        }
        if (loadImageControll == null)
        {
            Debug.Log("k tim thay LoadImage");
        }
    }
    // Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        NextLevel();
        GameOver();
	}
    //reset lai level va random lai map
    void GameOver()
    {
        if (time.timeS <= 0 && loadImageControll.score < loadImageControll.diemDatDuoc[loadImageControll.level])
        {
            nextLevel.text = "Game Over";
            panel.active = true;
            loadImageControll.activeTime = false;
            if (delay > timeDelay)
            {
                loadImageControll.RandomMap();
                nextLevel.text = "";
                loadImageControll.score = 0;
                time.timeImage.fillAmount = 1;
                time.timeS = 60;
                loadImageControll.activeTime = true;
                panel.active = false;
                loadImageControll.level = 0;
                delay = 0;
            }
            delay += Time.deltaTime;
        }
    }
    //khi nguoi choi du dieu kien thi qua man
    void NextLevel()
    {
        if (loadImageControll.score > loadImageControll.diemDatDuoc[loadImageControll.level])
        {
            if (loadImageControll.level < 4)
            {
                nextLevel.text = " Next Level";
            }
            if (loadImageControll.level == 4)
            {
                nextLevel.text = " WIN CMNR";
                delay = 0;
            }
            panel.active = true;
            loadImageControll.activeTime = false;
            if (delay > timeDelay)
            {
                loadImageControll.level++;
                loadImageControll.RandomMap();                
                nextLevel.text = "";
                loadImageControll.score = 0;
                time.timeImage.fillAmount = 1;
                time.timeS = 60;                
                loadImageControll.activeTime = true;
                panel.active = false;
                
                delay = 0;                
            }
            delay += Time.deltaTime;
        }
    }
}
