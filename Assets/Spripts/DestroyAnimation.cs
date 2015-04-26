using UnityEngine;
using System.Collections;

public class DestroyAnimation : MonoBehaviour {

	// Use this for initialization
    public bool active = false;
    LoadImageController gameControl;
	void Start () {
        GameObject game = GameObject.Find("Canvas");
        gameControl = game.GetComponentInChildren<LoadImageController>();

	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void Destroy()
    {
        Destroy(gameObject);
        gameControl.desGem = true;
    }
}
