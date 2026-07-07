using System.Collections;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [Header("Switch Objects")]
    public Transform switchHandle;
    public Transform upSwitch;
    public Transform downSwitch;

    [Header("Settings")]
    public float switchTime = 0.25f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip switchUpSound;
    public AudioClip switchDownSound;
    public AudioClip incorrectSound;

    [Header("Lighting")]
    public GameObject topLight;
    public GameObject bottomLight;
    public bool isLit;

    [Header("Index and Switchboard")]
    public int switchIndex;
    public SwitchBoard switchBoard;

    private bool isUp = true;
    private bool isMoving = false;
    private bool resetting = false;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Start()
    {
        switchHandle.rotation = upSwitch.rotation;
        isUp = true;

        UpdateLights();
    }

    public void Interact()
    {
        if (switchBoard == null) return;
        if (!switchBoard.boardActive) return;
        if (switchBoard.boardLocked) return;
        if (isMoving) return;

        bool wasCorrect = switchBoard.SwitchFlipped(switchIndex);

        if (!wasCorrect)
        {
            switchBoard.boardLocked = true;
        }

        StartCoroutine(FlickSwitch(wasCorrect));
    }

    private void UpdateLights()
    {
        if (!isLit) return;
        if (topLight == null || bottomLight == null) return;

        topLight.SetActive(isUp);
        bottomLight.SetActive(!isUp);
    }

    private IEnumerator FlickSwitch(bool wasCorrect)
    {
        isMoving = true;

        Quaternion startRotation = switchHandle.rotation;
        Quaternion targetRotation = isUp ? downSwitch.rotation : upSwitch.rotation;

        float elapsed = 0f;

        while (elapsed < switchTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / switchTime;

            switchHandle.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

            yield return null;
        }

        switchHandle.rotation = targetRotation;

        if (audioSource != null)
        {
            if (isUp && switchDownSound != null)
                audioSource.PlayOneShot(switchDownSound);
            else if (!isUp && switchUpSound != null)
                audioSource.PlayOneShot(switchUpSound);
        }

        isUp = !isUp;
        UpdateLights();

        isMoving = false;

        if (!wasCorrect)
        {
            yield return StartCoroutine(Incorrect());
        }
    }

    private IEnumerator Incorrect()
    {

        if (audioSource != null && incorrectSound != null)
        {
            audioSource.PlayOneShot(incorrectSound);
            yield return new WaitForSeconds(incorrectSound.length);
        }

        yield return StartCoroutine(ResetSwitch());

        switchBoard.boardLocked = false;
    }

    private IEnumerator ResetSwitch()
    {
        isMoving = true;

        Quaternion startRotation = switchHandle.rotation;
        Quaternion targetRotation = isUp ? downSwitch.rotation : upSwitch.rotation;

        float elapsed = 0f;

        while (elapsed < switchTime)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / switchTime;

            switchHandle.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

            yield return null;
        }

        switchHandle.rotation = targetRotation;

        isUp = !isUp;
        UpdateLights();

        isMoving = false;

        if (audioSource != null)
        {
            if (isUp && switchDownSound != null)
                audioSource.PlayOneShot(switchDownSound);
            else if (!isUp && switchUpSound != null)
                audioSource.PlayOneShot(switchUpSound);
        }
    }

}