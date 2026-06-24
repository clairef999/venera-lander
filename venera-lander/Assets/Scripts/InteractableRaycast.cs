using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

public class InteractableRaycast : MonoBehaviour
{

    public bool interactOnMouseHeld = false;

    public bool debugMode = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            CheckInteractable();
        }
        else if (interactOnMouseHeld && Input.GetKey(KeyCode.Mouse0))
        {
            CheckInteractable();
        }
    }
    public void CheckInteractable()
    {
        if (debugMode)Debug.Log("Checking interactables...");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);

        if (debugMode) Debug.Log("Hits: " + hits.Length);

        foreach (RaycastHit hit in hits)
        {
            if (hit.transform.gameObject.GetComponent<Interactable>() != null)
            {
                if (debugMode) Debug.Log("Found interactable: " + hit.transform.gameObject.name);
                hit.transform.gameObject.GetComponent<Interactable>().Interact();
            }
        }
    }
}
