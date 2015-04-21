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

    Gem gem;

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
                if (i == 3 && j == 5)
                {
                    map[3][5] = 1;
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
        CacCucRoiXuong();
        if(Input.GetMouseButtonDown(1))
        {
            VetCan(1, 1);
        }
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
        
        gem = a.GetComponent<Gem>();
        gem.SetProfile(cot, hang, index);


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
                if (arrGem[i][j] == null)
                {
                    if (arrGem[i][j + 1] != null && arrGem[i][j + 1].tag == "Rock")
                    {

                        int max = i - 1 > 0 ? i - 1 : i + 1;

                        if (arrGem[max][j + 1] == null)
                        {
                            return;
                        }
                        else
                        {
                            arrGem[max][j + 1].transform.position = new Vector3(arrGem[i][j + 1].transform.position.x, arrGem[i][j + 1].transform.position.y - (float)(0.8f), arrGem[i][j + 1].transform.position.z);
                            arrGem[i][j] = arrGem[max][j + 1];
                            arrGem[max][j + 1] = null;
                        }
                    }
                    if (arrGem[i][j + 1] != null && arrGem[i][j + 1].tag != "Rock")
                    {
                        arrGem[i][j + 1].transform.position = new Vector3(arrGem[i][j + 1].transform.position.x, arrGem[i][j + 1].transform.position.y - (float)(0.8f), arrGem[i][j + 1].transform.position.z);
                        arrGem[i][j] = arrGem[i][j + 1];
                        arrGem[i][j + 1] = null;
                    }
                }
            }
        }

    }


    void VetCan(int i, int j)
    {

        Debug.Log(arrGem[i][j]);
        
        for (int b = j - 1; b <= j + 1; b++)
        {
            for (int a = i - 1; a <= i + 1; a++)
            {
                if (a >= 0 && b >= 0 && a <= 6 && b <= 7)
                {
                    if (arrGem[i][j].tag == arrGem[a][b].tag && arrGem[a][b].GetComponent<Gem>().check == false)
                    {
                        arrGem[i][j].GetComponent<Gem>().check = true;
                        arrGem[a][b].GetComponent<Gem>().check = true;
                        arrGem[i][j].GetComponent<Gem>().ChangleColor();
                        arrGem[a][b].GetComponent<Gem>().ChangleColor();
                        VetCan(a, b);
                    }
                }
                
            }
        }
    }
    void ThuatToanVetCan(int i, int j)
    {
        Debug.Log(arrGem[i][j]);
        //1
        if (arrGem[i][j].tag == arrGem[i -1][j - 1].tag && arrGem[i -1][j - 1].GetComponent<Gem>().check == false)
        {
            Debug.Log("1");
            arrGem[i][j].GetComponent<Gem>().check = true;
            arrGem[i - 1][j - 1].GetComponent<Gem>().check = true;
            if (i - 1 < 0)
            {
                i = 1;
            }
            if (j - 1 < 0)
            {
                j = 1;
            }
            ThuatToanVetCan(i -1, j - 1);
        }
        //2
        if (arrGem[i][j].tag == arrGem[i][j - 1].tag && arrGem[i][j - 1].GetComponent<Gem>().check == false)
        {
            Debug.Log("2");
            arrGem[i][j].GetComponent<Gem>().check = true;
            arrGem[i][j - 1].GetComponent<Gem>().check = true;
            if (j - 1 < 0)
            {
                j = 1;
            }
            ThuatToanVetCan(i , j - 1);
        }
        //3
        if (arrGem[i][j].tag == arrGem[i + 1][j - 1].tag && arrGem[i + 1][j - 1].GetComponent<Gem>().check == false)
        {
            Debug.Log("3");
            arrGem[i][j].GetComponent<Gem>().check = true;
            arrGem[i + 1][j - 1].GetComponent<Gem>().check = true;
            if (i + 1 > 8)
            {
                i = 6;
            }
            if (j - 1 < 0)
            {
                j = 1;
            }
            
            ThuatToanVetCan(i + 1, j - 1);
        }
        //4
        if (arrGem[i][j].tag == arrGem[i - 1][j].tag && arrGem[i - 1][j ].GetComponent<Gem>().check == false)
        {
            Debug.Log("4");
            arrGem[i][j].GetComponent<Gem>().check = true;
            arrGem[i - 1][j].GetComponent<Gem>().check = true;
            if (i - 1 < 0)
            {
                i = 1;
            }
            ThuatToanVetCan(i -1, j );
        }
        //5
        if (arrGem[i][j].tag == arrGem[i + 1][j].tag && arrGem[i + 1][j].GetComponent<Gem>().check == false)
        {
            Debug.Log("5");
            arrGem[i][j].GetComponent<Gem>().check = true;
            arrGem[i + 1][j].GetComponent<Gem>().check = true;
            if (i + 1 > 8)
            {
                i = 6;
            }
            ThuatToanVetCan(i + 1, j);
        }
        //6
        if (arrGem[i][j].tag == arrGem[i - 1][j + 1].tag && arrGem[i -1][j + 1].GetComponent<Gem>().check == false)
        {
            Debug.Log("6");
            arrGem[i][j].GetComponent<Gem>().check = true;
            arrGem[i - 1][j + 1].GetComponent<Gem>().check = true;
            if (i - 1 < 0)
            {
                i = 1;
            }
            if (j + 1 > 7)
            {
                j = 5;
            }
            ThuatToanVetCan(i - 1, j + 1);
        }
        //7
        if (arrGem[i][j].tag == arrGem[i][j + 1].tag && arrGem[i][j + 1].GetComponent<Gem>().check == false)
        {
            Debug.Log("7");
            arrGem[i][j].GetComponent<Gem>().check = true;
            arrGem[i][j + 1].GetComponent<Gem>().check = true;
            if (j + 1 > 7)
            {
                j = 5;
            }
            ThuatToanVetCan(i, j + 1);
        }
        //8
        if (arrGem[i][j].tag == arrGem[i + 1][j + 1].tag && arrGem[i + 1][j + 1].GetComponent<Gem>().check == false)
        {
            Debug.Log("8");
            arrGem[i][j].GetComponent<Gem>().check = true;
            arrGem[i + 1][j + 1].GetComponent<Gem>().check = true;
            if (i + 1 > 8)
            {
                i = 6;
            }
            if (j + 1 > 7)
            {
                j = 5;
            }
            ThuatToanVetCan(i + 1, j + 1);
        }

    }
  
}
