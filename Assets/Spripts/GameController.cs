using UnityEngine;
using System.Collections;


public class GameController : MonoBehaviour {

	// Use this for initialization
    //thay doi mau
    public Sprite[] sprite_Color = new Sprite[5];
    public GameObject[] gameObjectColor = new GameObject[5];

    public float x, y;

    //mang hai chieu chua cac sprite
    private int [][] list = new int[10][];


	void Start () { 
        RandomList();
        
	}
	
	// Update is called once per frame
	void Update () {
          
	
	}
    void RandomList()
    {
        int randDom;
        for (int i = 0; i < 10; i++)
        {
            list[i] = new int[10];
        }
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                randDom = Random.Range(0, 5);
                list[i][j] = randDom;
                Debug.Log(list[i][j]);
                Instantiate(gameObjectColor[list[i][j]], new Vector3(i * 1 + x, j * 1 + y, 0), Quaternion.identity);
             }
        }
        

    }

}
