using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Controller : MonoBehaviour {

	// Use this for initialization
    private LoadImageController loadImageControll;
    private Timer time;
    public int[] diemDatDuoc;

    public Text nextLevel;
    public GameObject panel;

    float timeDelay = 3;
    float delay;
    int level = 0;

    void Awake()
    {
        diemDatDuoc = new int[5] { 5000, 9000, 12000, 15000, 18000 };
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

	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        NextLevel();
        GameOver();
	}
    void GameOver()
    {
        if (time.timeS <= 0 && loadImageControll.score < diemDatDuoc[level])
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
                level = 0;
                delay = 0;
            }
            delay += Time.deltaTime;
        }
    }
    void NextLevel()
    {
        if (loadImageControll.score > diemDatDuoc[level])
        {
            if (level < 4)
            {
                nextLevel.text = " Next Level";
            }
            if (level == 4)
            {
                nextLevel.text = " WIN CMNR";
                
            }
            panel.active = true;
            loadImageControll.activeTime = false;
            if (delay > timeDelay)
            {
                level++;
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
