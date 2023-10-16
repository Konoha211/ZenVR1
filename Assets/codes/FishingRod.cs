using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class FishingRod : MonoBehaviour
{
    public GameObject redSpot;
    public GameObject greenSpot;
    public float movementThreshold = 0.1f; // Set the threshold value based on your requirement
    public float successHoldDuration = 5f; // Duration in seconds to hold within threshold for success
    private bool isGrabbed = false;
    private Vector3 initialPosition;
    private float holdTimer;

    private XRBaseInteractable interactable;

    void Start()
    {
        // Get the XRBaseInteractable component
        interactable = GetComponent<XRBaseInteractable>();

        // Subscribe to the select events (grabbing)
        interactable.selectEntered.AddListener(Grabbed);
        interactable.selectExited.AddListener(Released);

        redSpot.SetActive(false);
        greenSpot.SetActive(false);
    }

    private void Grabbed(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        initialPosition = transform.position;
        holdTimer = 0f;
    }

    private void Released(SelectExitEventArgs args)
    {
        isGrabbed = false;
        redSpot.SetActive(false);
        greenSpot.SetActive(false);
    }

    void Update()
    {
        if (isGrabbed)
        {
            float distance = Vector3.Distance(initialPosition, transform.position);

            if (distance > movementThreshold)
            {
                // Failed condition
                Fail();
            }
            else
            {
                // If within threshold, start accumulating time
                holdTimer += Time.deltaTime;
                if (holdTimer >= successHoldDuration)
                {
                    // Success condition
                    Success();
                }
            }
        }
    }

    private void Fail()
    {
        redSpot.SetActive(true);
        greenSpot.SetActive(false);
        holdTimer = 0f; // Reset timer
    }

    private void Success()
    {
        redSpot.SetActive(false);
        greenSpot.SetActive(true);
        // Optionally, you may want to release the rod automatically or reset the challenge
    }

    void OnDestroy()
    {
        // Unsubscribe from the select events (grabbing)
        interactable.selectEntered.RemoveListener(Grabbed);
        interactable.selectExited.RemoveListener(Released);
    }
}
