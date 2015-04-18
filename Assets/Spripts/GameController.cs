using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour {

    private int index;//so thu tu cac Prefabs
    public GameObject[] listGem;//list cac gem_Prefabs
    public GameObject clock;
    private GameObject[][] arrGem;//list Game Object hien ra man hinh
    public float x, y;//vi tri camera
    public int soHang;//so hang cua mang
    public int soCot;//so cot cua mang

    private RaycastHit2D rayHit;

    private int[][] map;

    void Start()
    {
        map = new int[soCot][];
        for (int i = 0; i < soCot; i++)
        {
            map[i] = new int[soHang];
        }
        for (int i = 0; i < soCot; i++)
        {
            for (int j = 0; j < soHang; j++)
            {
                if (i == 4 && j == 5)
                {
                    map[4][5] = 1;
                }
                else
                    map[i][j] = 0;
            }
        }
        RandomImage();
    }
    void Update()
    {
        DestroyButtonMouse();
        if (Input.GetMouseButtonDown(1))
            CacCucRoiXuong();
        CacCucRoiXuong();
    }
    void RandomImage()
    {
        arrGem = new GameObject[soCot][];

        for (int i = 0; i < soCot; i++)
        {
            arrGem[i] = new GameObject[soHang];
        }
        for (int i = 0; i < soCot; i++)
        {
            for (int j = 0; j < soHang; j++)
            {
                //InstantiateGem(i, j);
                if (map[i][j] == 0)
                {
                    InstantiateGem(i, j);//in ra cac Object o vi tri PosIT
                }
                else
                {
                    InstantiateBlock(i, j);
                }
            }
        }
    }
    void InstantiateGem(int hang, int cot)
    {
        index = Random.Range(0, 5);
        GameObject a = Instantiate(listGem[index], new Vector3(hang * 0.8f - x, cot * 0.8f - y, 0), Quaternion.identity) as GameObject;
        arrGem[hang][cot] = a;
        //add vao Canvas
        a.transform.parent = transform;
        a.transform.localScale = Vector3.one;

    }
    void InstantiateBlock(int hang, int cot)
    {
        GameObject a = Instantiate(clock, new Vector3(hang * 0.8f - x, cot * 0.8f - y, 0), Quaternion.identity) as GameObject;
        arrGem[hang][cot] = a;
        //add vao Canvas
        a.transform.parent = transform;
        a.transform.localScale = Vector3.one;
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
                Destroy(rayHit.collider.gameObject);

            }

        }
    }

    [ContextMenu("CacCucRoiXuong")]
    void CacCucRoiXuong()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (arrGem[i][7] == null)
                {
                    InstantiateGem(i, 7);
                }
                if (arrGem[i][j] == null) //kiem tra xem cac de roi xong
                {

                    if (arrGem[i][j + 1].tag == "Rock")
                    {
                        arrGem[i - 1][j + 1].transform.position = new Vector3(arrGem[i][j + 1].transform.position.x, arrGem[i][j + 1].transform.position.y - (float)(0.8f), arrGem[i][j + 1].transform.position.z);
                        arrGem[i][j] = arrGem[i -1][j + 1];
                        arrGem[i -1][j + 1] = null;
                    }

                    if (arrGem[i][j + 1].tag != "Rock")
                    {
                        arrGem[i][j + 1].transform.position = new Vector3(arrGem[i][j + 1].transform.position.x, arrGem[i][j + 1].transform.position.y - (float)(0.8f), arrGem[i][j + 1].transform.position.z);
                        arrGem[i][j] = arrGem[i][j + 1];
                        arrGem[i][j + 1] = null;
                    }                   

                }
            }
        }

    }
  
}
