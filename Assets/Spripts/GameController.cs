using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour {

    public GameObject[] gameObjectColor = new GameObject[5];
    public float x, y;

    private int[][] listInt = new int[10][];
    private GameObject[] listGameObject = new GameObject[100];

    GameObject a;
    private List<GameObject> b = new List<GameObject>(); 
    private List<GameObject> list = new List<GameObject>();
    RaycastHit2D hit;

    int count = 0;
    int posX, posY;
    void Start()
    {
        RandomList();
    }
    void Update()
    {
        DestroyMouse();

    }

    void RandomList()
    {
        for (int i = 0; i < 10; i++)
        {
            listInt[i] = new int[10];
        }
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                listInt[i][j] = Random.Range(0, 5);
                listGameObject[count] = gameObjectColor[listInt[i][j]];
                Instantiate(listGameObject[count], new Vector3(i * 1 - x, j * 1 - y, 0), Quaternion.identity);
                //a = (GameObject)Instantiate(listGameObject[count], new Vector3(i * 1 - x, j * 1 - y, 0), Quaternion.identity) as GameObject;
                //list.Add(a);
                count++;
            }
        }
    }
    void DestroyMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

            if (hit.collider == null)
            {
                return;
            }
            else
            {
                TimViTri();
                
                //Destroy(hit.collider.gameObject);
                //Destroy(list[posX + posY*10]);                
                //list[posX + posY * 10 + 1].transform.position = new Vector3(list[posX + posY * 10 + 1].transform.position.x, list[posX + posY * 10 + 1].transform.position.y - 1);
            }
            
        }
    }
    void TimViTri()
    {

        posY = (int)(hit.transform.position.x + x );
        posX = (int)(hit.transform.position.y + y);

        Debug.Log(posX + "\t" + posY);

    }
    void ResetVitri()
    {
        posX = 0;
        posY = 0;
    }

    [ContextMenu("ReloadList")]
    public void ReloadList()
    {
        listGameObject[0].gameObject.active = false;
        
    }

}
