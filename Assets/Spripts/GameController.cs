using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameController : MonoBehaviour {

    public GameObject gameObj1;
    public GameObject gameObj2;
    public GameObject conect;
    void Start()
    {

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            XoayRotation();
        }
        if (Input.GetMouseButtonDown(1))
        {
            ResetRotation();
        }
    }

    [ContextMenu("XoayRotation")]
    void XoayRotation()
    {
        float x, y;
        x = (gameObj1.transform.position.x + gameObj2.transform.position.x) / 2;
        y = (gameObj1.transform.position.y + gameObj2.transform.position.y) / 2;

        conect.transform.position = new Vector3(x, y, 0);
        Vector3 relative = gameObj1.transform.InverseTransformPoint(gameObj2.transform.position);
        float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
        conect.transform.Rotate(0, 0,-angle);
    }
    [ContextMenu("ResetRotation")]

    void ResetRotation()
    {
        conect.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
  
}
