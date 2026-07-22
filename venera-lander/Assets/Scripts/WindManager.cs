using UnityEngine;

public class WindManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private RectTransform needle;

    [Header("Tilt")]
    [SerializeField] private float currentTilt = 0f;
    [SerializeField] private float tiltAxis = 0f;
    [SerializeField] private float maxDegreesPerSecond = 15f;

    [Header("Thrusters")]
    [SerializeField] private float thrusterStrength = 0.75f;

    [Header("Imbalance")]
    [SerializeField] private float dangerZoneStart = 15f;
    [SerializeField] private float dangerZoneEnd = 90f;
    [SerializeField] private float imbalanceStrength = 0.25f;

    [Header("Stabilization")]
    [SerializeField] private float stableZoneLimit = 15f;
    [SerializeField] private float stabilizationStrength = 0.15f;

    private void Update()
    {
        UpdateTiltAxis();
        UpdateTilt();
        UpdateNeedle();
    }

    private void UpdateTiltAxis() //this whole method calculates the adjustment we make to the tilt axis each frame
    {
        float axisAdjustment = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            axisAdjustment += thrusterStrength;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            axisAdjustment -= thrusterStrength;
        }
        
        float absoluteTilt = Mathf.Abs(currentTilt);
        bool isInStableZone = absoluteTilt <= stableZoneLimit;
        bool isInDangerZone = absoluteTilt >= dangerZoneStart && absoluteTilt < dangerZoneEnd;

        if (isInStableZone) //bring the tilt back to center
        {
            axisAdjustment -= Mathf.Sign(currentTilt) * stabilizationStrength;
        }
        else if (isInDangerZone) //push further away from center
        {
            axisAdjustment += Mathf.Sign(currentTilt) * imbalanceStrength;
        }
        //mathf.sign returns 1 if positive and -1 if negative, which can simplify this section because
        //both stabilization and imbalance adjustments are based on the direction of the current tilt

        tiltAxis += axisAdjustment * Time.deltaTime;
        tiltAxis = Mathf.Clamp(tiltAxis, -1f, 1f);
    }

    private void UpdateTilt() //this applies adjustments based on the previous method
    {
        float degreesThisFrame = tiltAxis * maxDegreesPerSecond * Time.deltaTime;

        currentTilt += degreesThisFrame;
        currentTilt = Mathf.DeltaAngle(0f, currentTilt);
    }

    private void UpdateNeedle() //this adjusts the actual UI element
    {
        needle.localRotation =
            Quaternion.Euler(0f, 0f, -currentTilt);
    }
}