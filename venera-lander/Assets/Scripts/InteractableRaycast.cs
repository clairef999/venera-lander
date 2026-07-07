using UnityEngine;

public class InteractableRaycast : MonoBehaviour
{
    public bool interactOnMouseHeld = false;
    public bool debugMode = false;

    private Switch lastInteractedSwitch;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            lastInteractedSwitch = null;
        }

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
        if (debugMode) Debug.Log("Checking interactables...");

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);

        if (debugMode) Debug.Log("Hits: " + hits.Length);

        foreach (RaycastHit hit in hits)
        {
            Switch hitSwitch = hit.transform.GetComponent<Switch>();

            if (hitSwitch != null)
            {
                if (hitSwitch == lastInteractedSwitch)
                {
                    return;
                }

                if (debugMode)
                    Debug.Log("Found interactable: " + hit.transform.gameObject.name);

                hitSwitch.Interact();
                lastInteractedSwitch = hitSwitch;

                return;
            }
        }
    }
}