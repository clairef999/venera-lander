using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
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

    [Header("Lighting")]
    public GameObject topLight;
    public GameObject bottomLight;
    public bool isLit;


    private bool isUp = true;
    private bool isMoving = false;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Start()
    {
        // Start in the up position
        switchHandle.rotation = upSwitch.rotation;
        isUp = true;


        if (isLit)
        {
            topLight.SetActive(true);
            bottomLight.SetActive(false);
        }
    }

    public void Interact()
    {
        if (!isMoving)
        {
            StartCoroutine(FlickSwitch());
        }
    }

    private IEnumerator FlickSwitch()
    {
        isMoving = true;

        Quaternion startRotation = switchHandle.rotation;
        Quaternion targetRotation = isUp ? downSwitch.rotation : upSwitch.rotation;

        float elapsed = 0f;

        while (elapsed < switchTime)
        {
            elapsed += Time.deltaTime;

            float time = elapsed / switchTime;
            switchHandle.rotation = Quaternion.Lerp(startRotation, targetRotation,time);

            yield return null;
        }

        switchHandle.rotation = targetRotation;

        // play sound effect
        if (audioSource != null)
        {
            if (isUp)
                audioSource.PlayOneShot(switchDownSound);
            else
                audioSource.PlayOneShot(switchUpSound);
        }


        if (isLit) // for lit switches, toggles active light
        {
            if (isUp) 
            {
                topLight.SetActive(false);
                bottomLight.SetActive(true);
            }
            else 
            {
                topLight.SetActive(true);
                bottomLight.SetActive(false);
            }
        }


        isUp = !isUp;
        isMoving = false;
    }
}