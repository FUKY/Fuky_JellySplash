using UnityEngine;
using System.Collections;
using System.Text;
using System.IO;  

public class LoadMap : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //string text = System.IO.File.ReadAllText("D:\\SVN\\Fuky_JellySplash\\trunk\\Assets\\Map\\myfile.txt");
        //Debug.Log(text);
        TextAsset mytxtData = Resources.Load("Map/myfile") as TextAsset;
        Debug.Log(mytxtData);
        //string txt = mytxtData.text;
        //Debug.Log(txt);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
