using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LoadImageController : MonoBehaviour {

	// Use this for initialization
    private  int index;
    public GameObject[] listGem;
    private int[][] listImage;
    public float x, y;
    private RaycastHit2D rayHit;
    private int posX, posY;
    private List<GameObject> listGameObject = new List<GameObject>();

    private List<GameObject> listDestroy = new List<GameObject>();
	void Start () {
        RandomImage();
	}
	
	// Update is called once per frame
	void Update () {
        DestroyButtonMouse();
        //DestroyGameObject();
	}
    void RandomImage()
    {
        listImage = new int[7][];
        for(int i=0; i<7; i++)
        {
            listImage[i] = new int[8];
        }
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                index = Random.Range(0, 6);

                GameObject a = Instantiate(listGem[index], new Vector3(i * 0.8f - x, j * 0.8f - y, 0), Quaternion.identity) as GameObject;
                a.transform.parent = transform;
                a.transform.localScale = Vector3.one;
                listGameObject.Add(a);
            }
        }
    }

    void DestroyButtonMouse()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rayHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            
            if (rayHit.collider == null)
            {
                return;
            }
            else
            {
                TimViTri();
                int indexVitri = posX * 8 + posY;
                //Destroy(rayHit.collider.gameObject);
                Destroy(listGameObject[indexVitri]);              
                Debug.Log(listGameObject[indexVitri]);
                if (listGameObject[indexVitri] == null)
                {
                    Debug.Log("adsadasd");
                }
                
            }
 
        }
    }
    void TimViTri()
    {

        posX = (int)(rayHit.transform.position.x/0.8 + x + 0.8);
        posY = (int)(rayHit.transform.position.y/0.8 + y + 0.8 );

        Debug.Log(posX + "\t" + posY);

    }

    [ContextMenu("SwapObject")]
    void SwapObject()
    {
        GameObject swap = new GameObject();
        swap = listGameObject[0];
        listGameObject[0] = listGameObject[1];
        listGameObject[1] = swap;
        Debug.Log(listGameObject[0]);
        Debug.Log(swap);
        Debug.Log(listGameObject[1]);
        GameObject b;
        b = Instantiate(listGameObject[0], listGameObject[1].transform.position, Quaternion.identity) as GameObject;
        b.transform.parent = transform;
        b.transform.localScale = Vector3.one;
        b = Instantiate(listGameObject[1], listGameObject[0].transform.position, Quaternion.identity) as GameObject;
        b.transform.parent = transform;
        b.transform.localScale = Vector3.one;
    }
    [ContextMenu("DestroyGameObject")]
    void DestroyGameObject()
    {
        for (int i = 0; i < listGameObject.Count; i++)
        {
            if (listGameObject[i] == null)
            {
                listDestroy.Add(listGameObject[i]);
                listDestroy.Add(listGameObject[i + 1]);
                GameObject b;

                //if (i != 6)
                //{
                //    b = Instantiate(listDestroy[1], new Vector3(listDestroy[1].transform.position.x, listDestroy[1].transform.position.y - 0.8f, 0), Quaternion.identity) as GameObject;
                //    b.transform.parent = transform;
                //    b.transform.localScale = Vector3.one;
                //}
                b = Instantiate(listDestroy[1], new Vector3(listDestroy[1].transform.position.x, listDestroy[1].transform.position.y - 0.8f, 0), Quaternion.identity) as GameObject;
                b.transform.parent = transform;
                b.transform.localScale = Vector3.one;

                Destroy(listGameObject[i + 1]);                
                listGameObject.RemoveAt(i + 1);

                Debug.Log(i);
                listDestroy.Clear();

            }
        }
        
        
    }
}
