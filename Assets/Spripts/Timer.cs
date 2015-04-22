using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Timer : MonoBehaviour {

	// Use this for initialization
    public Image timeImage;
    public Text textTime;
    public float timeS;
    float timeDelay = 1;
    float a = 100;
    float delay;

    public GameObject canVas;
    public GameObject Over;
	void Start () {
 
	}
	
	// Update is called once per frame
	void Update () {
        if (delay > timeDelay)
        {
            a -= 1;
            timeS--;
            string t = timeS.ToString();
            textTime.text = t;
            delay = 0;
            timeImage.fillAmount -= 0.01666666f;
        }
        if (timeS <= 0)
        {
            timeS = 60;
            timeImage.fillAmount = 1;
        }
        delay += Time.deltaTime;
	}

}
