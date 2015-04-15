using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class LoadImageController : MonoBehaviour {

	// Use this for initialization
    private  int index;
    public GameObject[] listGem;
    private int[][] listImage;

    private GameObject[][] arrGem;
    public float x, y;

    private RaycastHit2D rayHit;
    private int posX, posY;

    private List<GameObject> ListDelete = new List<GameObject>();

	void Start () {
        RandomImage();
	}
	
	// Update is called once per frame
	void Update () {
        DestroyButtonMouse();
        if (Input.GetMouseButtonDown(1))
        {
            DestroyGameObject();
        }
	}
    void RandomImage()
    {
        listImage = new int[7][];
        arrGem = new GameObject[7][];
        for(int i=0; i<7; i++)
        {
            listImage[i] = new int[8];
            arrGem[i] = new GameObject[8];
        }
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 8; j++)
            {                
                InstantiateGem(i, j);
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
                if (ListDelete.Count == 0)
                {
                    ListDelete.Add(rayHit.collider.gameObject);
                }
                if (ListDelete.Count != 0)
                {
                    if (rayHit.collider.gameObject.tag == ListDelete[0].tag)
                    {
                        ListDelete.Add(rayHit.collider.gameObject);
                    }
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
    void DestroyGameObject()
    {
        int countNull = 1;
        for (int i = 0; i < 7; i++) 
        {
            for (int j = 0; j < 8; j++) 
            {

                if (arrGem[i][j] == null && arrGem[i][j + 1] != null) 
                {
                    arrGem[i][j + 1].transform.position = new Vector3(arrGem[i][j + 1].transform.position.x, arrGem[i][j + 1].transform.position.y - (float)(0.8f), arrGem[i][j + 1].transform.position.z);
                    arrGem[i][j] = arrGem[i][j + 1];
                    arrGem[i][j + 1] = null;
                    if (arrGem[i][7] == null)
                        InstantiateGem(i, 7);
                }
                if (arrGem[i][7] == null)
                {
                    InstantiateGem(i, 7);
                }
                      
            }
        }

    }
    
    void InstantiateGem( int hang, int cot)
    {
        index = Random.Range(0, 6);
        GameObject a = Instantiate(listGem[index], new Vector3(hang * 0.8f - x, cot * 0.8f - y, 0), Quaternion.identity) as GameObject;
        arrGem[hang][cot] = a;
        a.transform.parent = transform;
        a.transform.localScale = Vector3.one;
    }
    [ContextMenu("Xoa")]
    void Xoa()
    {
        Debug.Log(ListDelete.Count);
        for (int i = 0; i < ListDelete.Count; i++)
        {
            Destroy(ListDelete[i]);
            
        }
        ListDelete.Clear();
    }
}
