using System.Collections;
using UnityEngine;

public class SwitchBoard : MonoBehaviour
{
    [Header("Switches")]
    public Switch[] switches;

    [Header("Cooling")]
    public float degreesCooled;

    [Header("Lighting")]
    public GameObject boardLight;

    public bool boardActive;
    public bool boardLocked;

    private Coroutine blinkCoroutine;

    public void ActivateBoard()
    {
        boardActive = true;
        StartBlinking();
    }

    public void DeactivateBoard()
    {
        boardActive = false;
        StopBlinking();
    }

    public void StartBlinking()
    {
        if (blinkCoroutine == null)
        {
            blinkCoroutine = StartCoroutine(BlinkRoutine());
        }
    }

    public void StopBlinking()
    {
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }

        boardLight.SetActive(false);
    }

    public bool SwitchFlipped(int switchIndex)
    {
        if (!boardActive) return false;
        if (boardLocked) return false;
        if (!TempManager.instance.canProgress) return false;

        int correctSwitch = TempManager.instance.currentSequence[TempManager.instance.currentIndex];

        if (correctSwitch == switchIndex)
        {

            TempManager.instance.currentIndex++;
            TempManager.instance.Progress();

            return true;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator BlinkRoutine()
    {
        while (true)
        {
            boardLight.SetActive(!boardLight.activeSelf);
            yield return new WaitForSeconds(0.25f);
        }
    }
}