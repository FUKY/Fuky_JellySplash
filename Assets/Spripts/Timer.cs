using UnityEngine;
using System.Collections;

public class Timer : MonoBehaviour {

	// Use this for initialization
    private SpriteRenderer time;
    int t = 100;
	void Start () {
        time = gameObject.GetComponent<SpriteRenderer>();
	
	}
	
	// Update is called once per frame
	void Update () {
	    t--;
        Debug.Log(t);
        UpdateTime();
	}
    public void UpdateTime()
    {
        // Set the health bar's colour to proportion of the way between green and red based on the player's health.
        time.material.color = Color.Lerp(Color.green, Color.red, 1 - t * 0.01f);

        // Set the scale of the health bar to be proportional to the player's health.
        time.transform.localScale = new Vector3(t * 0.01f, 1, 1);
    }
}
