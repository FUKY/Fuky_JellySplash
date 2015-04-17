using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class LoadImageController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

	// Use this for initialization
    private  int index;
    public GameObject[] listGem;

    private int[][] listImage;


    private GameObject[][] arrGem;
    public float x, y;
    public int soHang;
    public int soCot;

    public int score = 0;

    private RaycastHit2D rayHit;
    

    private List<GameObject> ListDelete = new List<GameObject>();
   
	void Start () {
        RandomImage();
        //ListDelete.Contains()
	}
	
	// Update is called once per frame
	void Update () {
        //DestroyButtonMouse();
        DestroyGameObject();        
	}
    void RandomImage()
    {
        listImage = new int[soCot][];
        arrGem = new GameObject[soCot][];
        for(int i=0; i<soCot; i++)
        {
            listImage[i] = new int[soHang];
            arrGem[i] = new GameObject[soHang];
        }
        for (int i = 0; i < soCot; i++)
        {
            for (int j = 0; j < soHang; j++)
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
    int TimViTriX(GameObject vitrClick)
    {
        int posX;
        posX = (int)(vitrClick.transform.position.x / 0.8 + x + 0.8);
        
        return posX;
    }
    int TimViTriY(GameObject vitrClick)
    {
        int posY;
        posY = (int)(vitrClick.transform.position.y / 0.8 + y + 0.8);
        return posY;
    }
    void DestroyGameObject()
    {
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
        index = Random.Range(0, 5);
        GameObject a = Instantiate(listGem[index], new Vector3(hang * 0.8f - x, cot * 0.8f - y, 0), Quaternion.identity) as GameObject;
        arrGem[hang][cot] = a;
        a.transform.parent = transform;
        a.transform.localScale = Vector3.one;
    }

    void Xoa()
    {

        if (ListDelete.Count >= 3)
        {
            for (int i = 0; i < ListDelete.Count; i++)
            {
                Destroy(ListDelete[i]);
                score += 100;
            }            
        }
        ListDelete.Clear();
    }


    public void OnBeginDrag(PointerEventData eventData)
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
        }
        
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        Xoa();

    }
    public void OnDrag(PointerEventData eventData)
    {
        rayHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (rayHit.collider == null)
        {
            return;
        }
        else
        {
            if (ListDelete[0] == null)
            {
                return;
            }
            if (rayHit.collider.gameObject.tag == ListDelete[0].tag && KiemTraKhoangCach(rayHit.collider.gameObject,ListDelete[ListDelete.Count - 1]) <= 1.5f)
            {
                if (!ListDelete.Contains(rayHit.collider.gameObject))
                    ListDelete.Add(rayHit.collider.gameObject);
            }
        }
        Debug.Log(ListDelete.Count);
        
    }

    float KiemTraKhoangCach(GameObject a, GameObject b)
    {
        float khoangCach = Vector3.Distance(a.transform.position, b.transform.position);
        return khoangCach;
    }

    void KiemTraViTri(GameObject vitriClick)
    {
        int posX = TimViTriX(vitriClick);
        int posY = TimViTriY(vitriClick);

        for (int i = posX - 1; i <= posX + 1; i++)
        {
            for (int j = posY - 1; j <= posY + 1; j++)
            { }
        }

    }

}
