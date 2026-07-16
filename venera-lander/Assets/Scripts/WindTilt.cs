using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WindTilt : MonoBehaviour
{
    public GameObject vessel;
    private int tilt;
    private int timer;
    public float tiltfactor;
    public bool windOn = true;
    // Start is called before the first frame update
    void Start()
    {
        //if (windOn)
        //{
        //    WindTilting();
        //}
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            //vessel.transform.DOLocalRotate(new Vector3(vessel.transform.localRotation.x + tiltfactor, -90, 0), 0.1f);
            //vessel.transform.Rotate(new Vector3(vessel.transform.localRotation.x + tiltfactor, -90, 0), Space.Self);
            //vessel.transform.localRotation = Quaternion.Euler(vessel.transform.localRotation.x + tiltfactor, -90, 0);
        }
        if (Input.GetKey(KeyCode.D))
        {
           // vessel.transform.Rotate(new Vector3(vessel.transform.localRotation.x - tiltfactor, -90, 0), Space.Self);
            //  vessel.transform.DOLocalRotate(new Vector3(vessel.transform.localRotation.x - tiltfactor, -90, 0), 0.1f);
            // vessel.transform.localRotation = Quaternion.Euler(vessel.transform.localRotation.x - tiltfactor, -90, 0);
        }
    }

    public void WindTilting()
    {
        tilt = Random.Range(-200, 60);
        timer = Random.Range(1, 10);
        StartCoroutine(WindTimer());
    }

    IEnumerator WindTimer()
    {
        vessel.transform.DOLocalRotate(new Vector3(tilt, -90, 0), timer);
        yield return new WaitForSeconds(timer);
        WindTilting();
    }
}
