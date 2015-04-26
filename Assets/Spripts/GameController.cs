using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour {

    private int index;//so thu tu cac Prefabs
    public GameObject[] listGem;//list cac gem_Prefabs
    public GameObject clock;
    public GameObject animationDestroy;
    private GameObject[][] arrGem;//list Game Object hien ra man hinh
    public float x, y;//vi tri camera
    public int soHang;//so hang cua mang
    public int soCot;//so cot cua mang

    Gem gem;

    private RaycastHit2D rayHit;

    //List<Object> listVetCan = new List<Object>();
    List<List<GameObject>> list = new List<List<GameObject>>();

    private int[][] map;

    List<GameObject> ListDelete = new List<GameObject>();

    float delay = 0;
    bool active = false;
    int m,  n;
    public bool hehe = false;
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
                map[i][j] = 0;
            }
        }
        RandomImage();
        CheckListInvalid();

    }
    [ContextMenu("Test1")]
    void Test1()
    {
        m = Random.Range(0, 5);
        n = Random.Range(0, 6);
        ChangleColor(m, n);
    }
    void Update()
    {
        DestroyButtonMouse();

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

                Instantiate(animationDestroy, rayHit.collider.transform.position, Quaternion.identity);
                active = true;
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
        //CheckListInvalid();

    }

    
    public void TestVetCan() 
    {

        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < 7; i++)
            {
                if (arrGem[i][j].tag != "Rock")
                {
                    List<GameObject> listVetCan = new List<GameObject>();
                    VetCan(listVetCan, i, j);
                    if (listVetCan.Count >= 3)
                    {
                        list.Add(listVetCan);
                        Debug.Log(list.Count);
                    }
                }           
                
            }
        }

    }
    void ResetCheckGem()
    {
        for(int i = 0; i < 7 ; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (arrGem[i][j].tag != "Rock")
                    arrGem[i][j].GetComponent<Gem>().check = false;
            }
        }
 
    }
    void CheckListInvalid() 
    {
        list.Clear();
        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < 7; i++)
            {
                if (arrGem[i][j].tag != "Rock")
                {
                    List<GameObject> listVetCan = new List<GameObject>();
                    VetCan(listVetCan, i, j);
                    if (listVetCan.Count >= 3)
                    {
                        list.Add(listVetCan);
                        Debug.Log(list.Count);
                        
                    }
                }
            }
        }
        ResetCheckGem();
    }
    //private List<Object> listVetCan = new List<Object>();
    List<GameObject> VetCan(List<GameObject> listVetCan, int i, int j)
    {
        
        for (int b = j - 1; b <= j + 1; b++)
        {
            for (int a = i - 1; a <= i + 1; a++)
            {
                if (a >= 0 && b >= 0 && a <= 6 && b <= 7)
                {
                    if (arrGem[i][j].tag == arrGem[a][b].tag && arrGem[a][b].GetComponent<Gem>().check == false)
                    {
                        //Debug.Log(arrGem[i][j]);
                        arrGem[i][j].GetComponent<Gem>().check = true;
                        arrGem[a][b].GetComponent<Gem>().check = true;

                        if (!listVetCan.Contains(arrGem[i][j]))
                        {
                            listVetCan.Add(arrGem[i][j]);                            
                        }
                        if (!listVetCan.Contains(arrGem[a][b]))
                        {
                            listVetCan.Add(arrGem[a][b]);
                        }
                        VetCan(listVetCan, a, b);                       
                    }
                }
                
            }
        }
        return listVetCan;
    }

    [ContextMenu("Test")]
    void Test(int i, int j)
    {
        if (arrGem[i][j] == null)
            NoTheoChieuDoc(j);
    }
    void ChangleColor(int i, int j)
    {
        arrGem[i][j].GetComponent<Image>().color = Color.Lerp(arrGem[i][j].GetComponent<Image>().color, Color.black, 0.5f);
    }
    void NoTheoChieuNgang(int vitri)
    {
        for (int i = 0; i < 7; i++)
        {
            ListDelete.Add(arrGem[i][vitri]);
        }
        for (int i = 0; i < ListDelete.Count; i++)
        {
            Destroy(ListDelete[i]);
        }
        ListDelete.Clear();
    }
    void NoTheoChieuDoc(int vitri)
    {
        for (int i = 0; i < 8; i++)
        {
            ListDelete.Add(arrGem[vitri][i]);
        }
        for (int i = 0; i < ListDelete.Count; i++)
        {
            arrGem[vitri][i] = null;
            Destroy(ListDelete[i]);            
        }
        ListDelete.Clear();
    }
    void ScaleGem()
    {
        for (int i = 0; i < list[0].Count; i++)
        {
            list[0][i].transform.localScale = new Vector3(localScale, localScale, 1);
        }
    }

    [ContextMenu("cucdacbiet1")]
    void cucdacbiet1()
    {
        CucDacBiet(1, 1);
    }
    void CucDacBiet(int i,int j)
    {
        for (int m = i - 1; m <= i + 1; m++)
        {
            for (int n = j - 1; n <= j + 1; n++)
            {
                if (m >= 0 && n >= 0 && arrGem[m][n] != arrGem[i][j])
                {
                    arrGem[m][n].tag = arrGem[i][j].tag;
                    arrGem[m][n].GetComponent<Image>().sprite = arrGem[i][j].GetComponent<Image>().sprite;
                }
            }
        }
    }
    float scale = 0.01f;
    float localScale = 1;

    bool activeHelp;
    void FixedUpdate()
    {
        
        localScale += scale;
        if (localScale > 1.2)
        {
            scale = -0.01f;
        }
        if (localScale < 0.8)
        {
            scale = 0.01f;
        }
        if (activeHelp == true)
            ScaleGem();
    }
}
