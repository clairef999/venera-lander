using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AirPressure : MonoBehaviour
{
    [System.NonSerialized] public float currentAirPressure;
    public float pressureIncreasePerSec = 1;
    public float pressureReleaseValue;
    public TMP_Text pressureTxt;
    public float releaseCooldownTime = 5;
    private bool coolingDown = false;
    private float timer = 0;
    private bool dangerousLvl = false;
    public float dangerousPressureValue;
    public float killPressureValue;
    public GameObject wheel;
    private float startMouseX;
    private float dragDelta = 0;
    // Start is called before the first frame update
    void Start()
    {
        pressureTxt.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        currentAirPressure += pressureIncreasePerSec * Time.deltaTime;
        pressureTxt.text = currentAirPressure.ToString();
        if (coolingDown)
        {
            timer += Time.deltaTime;
            if(timer >= releaseCooldownTime)
            {
                timer = 0;
                coolingDown = false;
            }
        }

        if(currentAirPressure >= dangerousPressureValue)
        {
            pressureTxt.color = Color.red;
            Debug.Log("pipe burst");
            dangerousLvl = true;
        }
        else if (currentAirPressure > dangerousPressureValue)
        {
            dangerousLvl = false;
        }

        if (currentAirPressure >= killPressureValue)
        {
            pressureTxt.text = "ur dead";
        }

        if (Input.GetMouseButtonDown(0))
        {

            // Vector3 t = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Vector3 t = Input.mousePosition;
            startMouseX = t.x;
        }
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.CompareTag("Wheel"))
                {
                    RotateWheel();
                    Debug.Log("wheel turn");
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            dragDelta = 0;
            startMouseX = 0;
        }
    }

    private Vector3 currentAngles;
    public void RotateWheel()
    {
        Vector3 currentMousePos = Input.mousePosition;
        dragDelta = currentMousePos.x - startMouseX;
        /*Vector3*/ currentAngles = new Vector3(0, 0, wheel.transform.localEulerAngles.z);
        currentAngles.z += (dragDelta / 200);
        wheel.transform.localEulerAngles = currentAngles;

        Debug.Log("startmouseX: " + startMouseX);
        Debug.Log("dragDelta.z: " + dragDelta);
        Debug.Log("currentMousePosX: " + currentMousePos.x);
        Debug.Log("currentAngles.z: " + currentAngles.z);

    }

    public void ReleasePressure()
    {
        if (!coolingDown && !dangerousLvl)
        {
            coolingDown = true;
            currentAirPressure -= pressureReleaseValue;

            if (currentAirPressure < 0)
            {
                currentAirPressure = 0;
            }
        }
    }

    public void FixBurst()
    {
        if (dangerousLvl)
        {
            dangerousLvl = false;
            currentAirPressure = currentAirPressure / 2;
            pressureTxt.color = Color.white;
            Debug.Log("pipe fixed");
        }

    }


}
