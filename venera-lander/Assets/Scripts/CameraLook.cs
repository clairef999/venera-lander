using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float rotateSpeed = 10f;
    private float direct = 0;
    private float streng = 0;

    public bool debug = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, (direct) * rotateSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.A))
        {
            direct = -1.5f;

        }
        else if (Input.GetKey(KeyCode.D))
        {
            direct = 1.5f;
        }
        else
        {
            direct = 0;
            streng = 0;
        }
        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        //RaycastHit hit;

        //if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        //{
        //    if (debug) Debug.Log(hit.transform.gameObject.name);
        //    if(hit.transform.gameObject.GetComponent<LookRegion>() != null)
        //    {
        //        var lookDir = hit.transform.gameObject.GetComponent<LookRegion>().direction;
        //        var lookFact = hit.transform.gameObject.GetComponent<LookRegion>().lookStrength;
        //        if(lookDir == LookRegion.dir.Left)
        //        {
        //            direct = -1f;
        //        }
        //        if (lookDir == LookRegion.dir.Right)
        //        {
        //            direct = 1f;
        //        }
        //        if(lookFact == LookRegion.strength.Min)
        //        {
        //            streng = 1f;
        //        }
        //        if (lookFact == LookRegion.strength.Max)
        //        {
        //            streng = 2f;
        //        }
        //    }
        //    else
        //    {
        //        direct = 0;
        //        streng = 0;
        //    }

        //}

    }
}
