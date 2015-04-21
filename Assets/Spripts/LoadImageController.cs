using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class LoadImageController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

	// Use this for initialization
    private  int index;//so thu tu cac Prefabs
    public GameObject[] listGem;//list cac gem_Prefabs
    private GameObject[][] arrGem;//list Game Object hien ra man hinh
    public float x, y;//vi tri camera
    public float iTwenPos;//vi tri in ra luc dau
    public int soHang;//so hang cua mang
    public int soCot;//so cot cua mang
    private int score = 0;//diem
    private RaycastHit2D rayHit;   
    private List<GameObject> ListDelete = new List<GameObject>();//list Object de xoa

    public Text textScore;

    private Image color;

    public GameObject conect;

    private List<GameObject> listConect = new List<GameObject>();

    private int[][] map;

    Gem gem;

	void Start () {
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
        map[3][4] = 1;
        map[0][4] = 1;
        RandomImage(iTwenPos);// random hinh anh khi moi dua vao game o vi tri ItweenPos
        //update vi tri vao man hinh
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                UpdateViTri(arrGem[i][j], i, j);
            }
        }
        
	}
	
	// Update is called once per frame
	void Update () {
        //DestroyButtonMouse();
        CacCucRoiXuong();//kiem tra va cho roi cac cuc 

        textScore.text = "Score: " + score;
        if (Input.GetMouseButtonDown(1))
        {
            LoangDau(1, 1);
        }
	}

    //random ra cac mau o vi tri khac nhau
    // tham so truyen vao posIT de o vi tri khac, va su dung Itween de dua ve vi tri trong man hinh
    void RandomImage(float posIT)
    {
        arrGem = new GameObject[soCot][];

        for(int i=0; i<soCot; i++)
        {
            arrGem[i] = new GameObject[soHang];

        }
        for (int i = 0; i < soCot; i++)
        {
            for (int j = 0; j < soHang; j++)
            {
                if (map[i][j] == 0)
                {
                    InstantiateGem(i, j, posIT);//in ra cac Object o vi tri PosIT
                }
                else
                {
                    InstantiateBlock(i, j);
                }
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
                //Destroy(rayHit.collider.gameObject);      
                Debug.Log(rayHit.collider.gameObject);

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

    //cho cac cuc roi xuong
    //
    void CacCucRoiXuong()
    {
        for (int i = 0; i < 7; i++) 
        {
            for (int j = 0; j < 8; j++)
            {
                if (arrGem[i][7] == null)
                {
                    InstantiateGem(i, 7, 0);
                }
                DiChuyenCacCuc(i, j);
            }
        }

    }

    void DiChuyenCacCuc(int m,int n)
    {
        if (arrGem[m][n] == null)
        {
            if (arrGem[m][n + 1] != null && arrGem[m][n + 1].tag == "Rock")
            {

                int max = m - 1 > 0 ? m - 1 :  m + 1;

                if (arrGem[max][n + 1] == null)
                {
                    return;
                }
                else
                {
                    arrGem[max][n + 1].transform.position = new Vector3(arrGem[m][n + 1].transform.position.x, arrGem[m][n + 1].transform.position.y - (float)(0.8f), arrGem[m][n + 1].transform.position.z);
                    arrGem[m][n] = arrGem[max][n + 1];
                    arrGem[max][n + 1] = null;
                }
            }
            if (arrGem[m][n + 1] != null && arrGem[m][n + 1].tag != "Rock")
            {
                arrGem[m][n + 1].transform.position = new Vector3(arrGem[m][n + 1].transform.position.x, arrGem[m][n + 1].transform.position.y - (float)(0.8f), arrGem[m][n + 1].transform.position.z);
                arrGem[m][n] = arrGem[m][n + 1];
                arrGem[m][n + 1] = null;
            }
        }
    }

    
    //In ra man hinh o vi tri "hang", "cot"
    //cong them posIT de su dung Itwen
    void InstantiateGem( int hang, int cot, float posIT)
    {
        index = Random.Range(0, 5);
        GameObject a = Instantiate(listGem[index], new Vector3(hang * 0.8f - x, cot * 0.8f - y + posIT, 0), Quaternion.identity) as GameObject;
        arrGem[hang][cot] = a;
        //add vao Canvas
        a.transform.SetParent(transform);
        a.transform.localScale = Vector3.one;

        gem = a.GetComponent<Gem>();
        gem.SetProfile(cot, hang, index);
    }
    void InstantiateBlock(int hang, int cot)
    {
        GameObject a = Instantiate(listGem[listGem.Length -1], new Vector3(hang * 0.8f - x, cot * 0.8f - y, 0), Quaternion.identity) as GameObject;
        arrGem[hang][cot] = a;
        //add vao Canvas
        a.transform.parent = transform;
        a.transform.localScale = Vector3.one;
    }

    //xoa cac  Object co trong listDelete
    void Xoa()
    {
        //xoa cac cuc
        if (ListDelete.Count >= 3)
        {
            for (int i = 0; i < ListDelete.Count; i++)
            {
                Destroy(ListDelete[i]);
                score += 100;
            }            
        }
        //xoa listConect
        for (int i = 0; i < listConect.Count; i++ )
        {
            Destroy(listConect[i]);
        }
        ListDelete.Clear();
        listConect.Clear();
        
    }

    //Bat dau nhan chuot va Add vao list Delete(xoa)
    
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

    //ket thuc nhan thi se xoa cac cuc trrong listDelete
    public void OnEndDrag(PointerEventData eventData)
    {
        Xoa();

    }
   
    //add cac cuc khi keo
    public void OnDrag(PointerEventData eventData)
    {
        rayHit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (ListDelete == null)
        {
            return;
        }

        if (rayHit.collider == null)
        {
            return;
        }

        if (ListDelete.Count <= 0 || ListDelete[0] == null) 
        {
            return;
        }

        if (rayHit.collider.gameObject.tag == ListDelete[0].tag && KiemTraKhoangCach(rayHit.collider.gameObject, ListDelete[ListDelete.Count - 1]) <= 1.5f)//kiem tra de dua vao listDelete
        {
            if (!ListDelete.Contains(rayHit.collider.gameObject) && ListDelete.Count >= 1)//kiem tra xem doi tuong chon da co trong ListDelete chua
            {
                InstantiateConect(rayHit.collider.gameObject, ListDelete[ListDelete.Count - 1]);//xuat ket noi ra man hinh
                ListDelete.Add(rayHit.collider.gameObject);

            }
            if (ListDelete.Count >= 2)
            {
                if (rayHit.collider.gameObject == ListDelete[ListDelete.Count - 2] && listConect.Count >= 1)//neu nguoi choi quay lai cuc phia trc co
                {
                    ListDelete.RemoveAt(ListDelete.Count - 1);
                    Destroy(listConect[listConect.Count - 1]);
                    listConect.RemoveAt(listConect.Count - 1);

                }
            }

        }
    }

    //kiem tra cac cuc gan nhau moi cho vao listDelete
    //ham tra ve khoang cac giua hai cuc
    float KiemTraKhoangCach(GameObject a, GameObject b)
    {
        float khoangCach = Vector3.Distance(a.transform.position, b.transform.position);
        return khoangCach;
    }

    //ssu dung Itween de update vi tri vao man hinh
    void UpdateViTri(GameObject gameObject, int hang, int cot)
    {
        iTween.MoveTo(gameObject, iTween.Hash(
            iT.MoveTo.position, new Vector3(hang * 0.8f - x, cot * 0.8f - y),//toi vi tri cuoi
            iT.MoveTo.time, 2.0f,//thoi gian
            iT.MoveTo.easetype, iTween.EaseType.easeOutBack));//hieu ung di chuyen
    }

    //In conect ra giua 2 cuc dc keo
    void InstantiateConect(GameObject gameObj1, GameObject gameObj2)
    {
        float x, y;
        x = (gameObj1.transform.position.x + gameObj2.transform.position.x) / 2;
        y = (gameObj1.transform.position.y + gameObj2.transform.position.y) / 2;

        GameObject a = Instantiate(conect, new Vector3(x, y, 0), Quaternion.identity) as GameObject;
        a.transform.parent = transform;
        listConect.Add(a);
        Vector3 relative = gameObj1.transform.InverseTransformPoint(gameObj2.transform.position);
        float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
        a.transform.Rotate(0, 0, -angle);
    }

    [ContextMenu("NoTheoChieuDoc")]
    void NoTheoChieuDoc()
    {
        for (int i = 0; i < 8; i++)
        {
            ListDelete.Add(arrGem[0][i]);
        }
        Xoa();
    }
    [ContextMenu("NoTheoChieuNgang")]
    void NoTheoChieuNgang()
    {
        for (int i = 0; i < 7; i++)
        {
            ListDelete.Add(arrGem[i][0]);
        }
        Xoa();
    }

    void LoangDau(int i, int j)
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
                        LoangDau(a, b);
                    }
                }
            }
        }
    }
}
