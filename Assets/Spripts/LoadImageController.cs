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


	void Start () {
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
        CacCucRoiXuong();//kiem tra va cho roi cac cuc 

        textScore.text = "Score: " + score;
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
                InstantiateGem(i, j, posIT);//in ra cac Object o vi tri PosIT
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
                if (arrGem[i][j] == null && arrGem[i][j + 1] != null) //kiem tra xem cac de roi xong
                {
                    //update to do moi
                    arrGem[i][j + 1].transform.position = new Vector3(arrGem[i][j + 1].transform.position.x, arrGem[i][j + 1].transform.position.y - (float)(0.8f), arrGem[i][j + 1].transform.position.z);
                    arrGem[i][j] = arrGem[i][j + 1];
                    arrGem[i][j + 1] = null;
                }    
                      
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
        if (ListDelete[0] == null)
        {
            return;
        }
        else
        if (rayHit.collider == null)
        {
            return;
        }
        else
        {
            if (rayHit.collider.gameObject.tag == ListDelete[0].tag && KiemTraKhoangCach(rayHit.collider.gameObject,ListDelete[ListDelete.Count - 1]) <= 1.5f)//kiem tra de dua vao listDelete
            {
                if (!ListDelete.Contains(rayHit.collider.gameObject) && ListDelete.Count >= 1)//kiem tra xem doi tuong chon da co trong ListDelete chua
                {
                    InstantiateConect(rayHit.collider.gameObject, ListDelete[ListDelete.Count - 1]);//xuat ket noi ra man hinh
                    ListDelete.Add(rayHit.collider.gameObject);
                    
                }
                if (rayHit.collider.gameObject == ListDelete[ListDelete.Count - 2] && ListDelete.Count >= 2 && listConect.Count >= 1)//neu nguoi choi quay lai cuc phia trc co
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

    void ChangleColor(GameObject gameObj)
    {
        color = gameObj.GetComponent<Image>();
        color.color = Color.red;        
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
}
