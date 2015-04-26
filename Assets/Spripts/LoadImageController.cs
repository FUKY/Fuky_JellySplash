using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class LoadImageController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

    //Public
    public GameObject[] listGem;//list cac gem_Prefabs
    public GameObject destroyGem;
    public GameObject conect;
    //UI
    public Text textScore;//text diem
    public Text textDiemCanDat;//text diem can dat

    public float x, y;//vi tri camera
    public float iTwenPos;//vi tri in ra luc dau
    public int soHang;//so hang cua mang
    public int soCot;//so cot cua mang
    public int score = 0;//diem
    
    public int[] diemDatDuoc;//cac level trong game(se thay bang load file txt)
    public int level;//level trong game
    public bool desGem = false;//kiem tra cac cuc Gem da xoa chua
    public float localScale = 1;//kich thuc gem
    public bool activeTime = false;
    public float timeHelp;   


	// -----------   
    private GameObject[][] arrGem;//list Game Object hien ra man hinh
    private RaycastHit2D rayHit;   
    private List<GameObject> ListDelete = new List<GameObject>();//list Object de xoa
    private List<GameObject> listConect = new List<GameObject>();//tao lien ket cho cac cuc(sau nay thanh thanh Animation)
    private List<List<GameObject>> listLoangDau = new List<List<GameObject>>();//kiem tra con duong nao de an khong
    private List<GameObject> listMouse = new List<GameObject>();//luu tat cac cac gem khi duoc keo
    private int[][] map;//map Game(load tu file txt)
    private Gem gem;

    private int index;//so thu tu cac Prefabs   
    private bool activeHelp;
    private float scale = 0.01f;
    private float help;
    private bool boolScale = false;
    private int m, n;


    //test
    private bool cucdacbiet;
    private bool cucdacbiet1;

    public GameObject[] dacBiet = new GameObject[4];
	void Start () {
        level = 0;
        diemDatDuoc = new int[5] { 5000, 9000, 12000, 15000, 18000 };
        activeTime = true;
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

        RandomImage(iTwenPos);// random hinh anh khi moi dua vao game o vi tri ItweenPos
        //update vi tri vao man hinh
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                UpdateViTri(arrGem[i][j], i, j);
            }
        }
        CheckListInvalid();
        
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
        CheckListInvalid();
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
            //kiem tra xem cac cuc dac biet co o trong listDelete khong
            for (int i = 0; i < soCot; i++)
            {
                for (int j = 0; j < soHang; j++)
                {
                    if (arrGem[i][j].GetComponent<Gem>().destroyCollum == true && ListDelete.Contains(arrGem[i][j]))
                    {
                        NoTheoChieuNgang(i);
                    }
                    if (arrGem[i][j].GetComponent<Gem>().destroyRow == true && ListDelete.Contains(arrGem[i][j]))
                    {
                        NoTheoChieuDoc(j);
                    }
                    if (arrGem[i][j].GetComponent<Gem>().destroyColRow == true && ListDelete.Contains(arrGem[i][j]))
                    {
                        NoTheoHaiChieu(i, j);
                    }
                }
            }
            //xoa cac Gem trong listDelete
            for (int i = 0; i < ListDelete.Count; i++)
            {
                
                Destroy(ListDelete[i]);
                Instantiate(destroyGem, ListDelete[i].transform.position, Quaternion.identity);
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
        listMouse.Clear();
        
    }

    //Bat dau nhan chuot va Add vao list Delete(xoa)
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        activeTime = false;
        desGem = false;
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
                listMouse.Add(rayHit.collider.gameObject);
            }
        }
        
    }

    //ket thuc nhan thi se xoa cac cuc trrong listDelete
    public void OnEndDrag(PointerEventData eventData)
    {
        
        Xoa();
        
        activeTime = true;
        cucdacbiet1 = true;
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
            if (!listMouse.Contains(rayHit.collider.gameObject))
            {
                listMouse.Add(rayHit.collider.gameObject);
            }
            if (!ListDelete.Contains(rayHit.collider.gameObject) && ListDelete.Count >= 1)//kiem tra xem doi tuong chon da co trong ListDelete chua
            {
                InstantiateConect(rayHit.collider.gameObject, ListDelete[ListDelete.Count - 1]);//xuat ket noi ra man hinh
                ListDelete.Add(rayHit.collider.gameObject);

                if (ListDelete.Count == 5)
                {
                    cucdacbiet = true;                    
                }
                if (ListDelete.Contains(rayHit.collider.gameObject) && rayHit.collider.gameObject.GetComponent<Gem>().cucDacBiet == true)
                {
                    CucDacBiet(TimViTriX(rayHit.collider.gameObject), TimViTriY(rayHit.collider.gameObject));
                }


            }
            if (ListDelete.Count >= 2)
            {
                if (rayHit.collider.gameObject == ListDelete[ListDelete.Count - 2] && listConect.Count >= 1)//neu nguoi choi quay lai cuc phia trc co
                {
                    
                    ListDelete.RemoveAt(ListDelete.Count - 1);
                    Destroy(listConect[listConect.Count - 1]);
                    listConect.RemoveAt(listConect.Count - 1);
                    GameObject a = new GameObject();
                    for (int i = 0; i < listMouse.Count; i++)
                    {
                        if (listMouse[i].GetComponent<Gem>().cucDacBiet == true)
                        {
                            a = listMouse[i];
                        }
                    }
                    if (!ListDelete.Contains(a))
                    {
                        ResetCucDacBiet(TimViTriX(a), TimViTriY(a));
                        listMouse.RemoveAt(listMouse.Count - 1);
                    }
                    
                        
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

    //no cac cuc theo chieu doc
    void NoTheoChieuNgang(int vitri)
    {
        for (int i = 0; i < 8; i++)
        {
            ListDelete.Add(arrGem[vitri][i]);
        }
    }
    //no cac cuc theo chieu ngang
    void NoTheoChieuDoc(int vitri)
    {
        for (int i = 0; i < 7; i++)
        {
            ListDelete.Add(arrGem[i][vitri]);
        }
    }

    //no cac cuc theo hai huong
    void NoTheoHaiChieu(int vitriX, int vitriY)
    {
        for (int i = 0; i < 7; i++)
        {
            ListDelete.Add(arrGem[i][vitriY]);
        }
        for (int i = 0; i < 8; i++)
        {
            ListDelete.Add(arrGem[vitriX][i]);
        }

    }
    [ContextMenu("Test")]
    void Test()
    {
        ResetCucDacBiet(m, n);
    }
    //reset cuc dac biet
    void ResetCucDacBiet(int i, int j)
    {
        for (int m = i - 1; m <= i + 1; m++)
        {
            for (int n = j - 1; n <= j + 1; n++)
            {
                if (m >= 0 && n >= 0)
                {
                    arrGem[m][n].tag = listGem[arrGem[m][n].GetComponent<Gem>().inDex].tag;
                    arrGem[m][n].GetComponent<Image>().sprite = listGem[arrGem[m][n].GetComponent<Gem>().inDex].GetComponent<Image>().sprite;

                }
            }
        }
    }
    //cuc dac biet thu nhat lam cac cuc xung quanh giong nhu no    
    void CucDacBiet(int i, int j)
    {
        for (int m = i - 1; m <= i + 1; m++)
        {
            for (int n = j - 1; n <= j + 1; n++)
            {
                if (m >= 0 && n >= 0 && arrGem[m][n] != arrGem[i][j])
                {
                    if (arrGem[m][n].GetComponent<Gem>().cucDacBiet == false)
                    {
                        arrGem[m][n].tag = arrGem[i][j].tag;
                        arrGem[m][n].GetComponent<Image>().sprite = arrGem[i][j].GetComponent<Image>().sprite;
                    }
                }
            }
        }
    }

    //sau nay xoa
    void CheckDestroy()
    {
        m = Random.Range(0, 5);
        n = Random.Range(0, 6);
        if (arrGem[m][n] != null && arrGem[m][n].GetComponent<Gem>().cucDacBiet != true)
        {
            //arrGem[m][n].GetComponent<Image>().color = Color.Lerp(arrGem[m][n].GetComponent<Image>().color, Color.black, 0.5f);
            int a = Random.Range(3, 4);
            if (a == 0)
            {
                arrGem[m][n].GetComponent<Gem>().destroyCollum = true;
                GameObject meomeo = Instantiate(dacBiet[0], arrGem[m][n].transform.position, Quaternion.identity) as GameObject;
                meomeo.transform.parent = arrGem[m][n].transform;
            }
            if (a == 1)
            {
                arrGem[m][n].GetComponent<Gem>().destroyRow = true;
                GameObject meomeo = Instantiate(dacBiet[1], arrGem[m][n].transform.position, Quaternion.identity) as GameObject;
                meomeo.transform.parent = arrGem[m][n].transform;
            }
            if (a == 2)
            {
                arrGem[m][n].GetComponent<Gem>().destroyColRow = true;
                GameObject meomeo = Instantiate(dacBiet[2], arrGem[m][n].transform.position, Quaternion.identity) as GameObject;
                meomeo.transform.parent = arrGem[m][n].transform;
            }
            if (a == 3)
            {
                arrGem[m][n].GetComponent<Gem>().cucDacBiet = true;
                GameObject meomeo = Instantiate(dacBiet[3], arrGem[m][n].transform.position, Quaternion.identity) as GameObject;
                meomeo.transform.parent = arrGem[m][n].transform;
            }
            
            cucdacbiet = false;
            cucdacbiet1 = false;
        }

    }

    //kiem tra xem con duong de an khong
    List<GameObject> LoangDau(List<GameObject> list, int i, int j)
    {

        for (int b = j - 1; b <= j + 1; b++)
        {
            for (int a = i - 1; a <= i + 1; a++)
            {
                if (a >= 0 && b >= 0 && a <= 6 && b <= 7)
                {
                    if (arrGem[i][j].tag != null && arrGem[a][b] != null)
                    {
                        if (arrGem[i][j].tag == arrGem[a][b].tag && arrGem[a][b].GetComponent<Gem>().check == false)
                        {
                            //Debug.Log(arrGem[i][j]);
                            arrGem[i][j].GetComponent<Gem>().check = true;
                            arrGem[a][b].GetComponent<Gem>().check = true;

                            if (!list.Contains(arrGem[i][j]))
                            {
                                list.Add(arrGem[i][j]);
                            }
                            if (!list.Contains(arrGem[a][b]))
                            {
                                list.Add(arrGem[a][b]);
                            }
                            LoangDau(list, a, b);
                        }
                    }
                }

            }
        }
        return list;
    }
    //dua cac cac dung de an vao list
    void CheckListInvalid()
    {
        listLoangDau.Clear();
        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < 7; i++)
            {                
                if (arrGem[i][j] != null && arrGem[i][j].tag != "Rock")
                {
                    List<GameObject> list = new List<GameObject>();
                    LoangDau(list, i, j);
                    if (list.Count >= 3)
                    {
                        listLoangDau.Add(list);
                    }
                }
            }
        }
        ResetCheckGem();
    }    
    void ResetCheckGem()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (arrGem[i][j] != null  && arrGem[i][j].tag != "Rock")
                    arrGem[i][j].GetComponent<Gem>().check = false;
            }
        }

    }

    //phong to va nho  kich thuoc cac cuc Gem
    void ScaleGem()
    {
        for (int i = 0; i < listLoangDau[0].Count; i++)
        {
            if (listLoangDau[0][i] != null)
                listLoangDau[0][i].transform.localScale = new Vector3(localScale, localScale, 1);
        }
        boolScale = true;
    }
    //reset kich thuoc cuc Gem ve ban dau
    void ResetScaleGem()
    {
        if (boolScale == true)
        {
            for (int i = 0; i < listLoangDau[0].Count; i++)
            {
                if (listLoangDau[0][i] != null)
                {
                    listLoangDau[0][i].transform.localScale = new Vector3(1, 1, 1);
                }
            }
            boolScale = false;
        }
    }

   //Ran dom lai Map
    public void RandomMap()
    {
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 8; j++)
            {                
                Destroy(arrGem[i][j]);
            }
        }
        listConect.Clear();
        ListDelete.Clear();
        listLoangDau.Clear();
    }


    void Update()
    {
        
    }
    void FixedUpdate()
    {

        textDiemCanDat.text = diemDatDuoc[level].ToString();//chuyen diem tu kieu in sang String
        textScore.text = "Score: " + score;
        if (cucdacbiet1 == true && cucdacbiet == true && desGem == true)
        {
            CheckDestroy();
        }
        if (desGem == true)
        {
            
            CacCucRoiXuong();
            
        }
       
        //sao 5s goi y duong cho nguoi choi
        if (activeTime == true)
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

            if (help > timeHelp)
            {
                activeHelp = true;
            }
            help += Time.deltaTime;
        }
        if (activeTime == false)
        {
            activeHelp = false;
            help = 0;
        }

        
        //phong to, nho kich thuoc cuc Gem cho nguoi choi biet
        if (activeHelp == true)
        {
            ScaleGem();
        }
        if (activeHelp == false)
        {
            ResetScaleGem(); 
        }
        if (listLoangDau.Count == 0)//neu k con duong nao de an Ramdom lai map
        {
            RandomMap();
        }
        
    }
}
