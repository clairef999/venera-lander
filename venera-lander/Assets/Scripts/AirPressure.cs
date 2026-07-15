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
