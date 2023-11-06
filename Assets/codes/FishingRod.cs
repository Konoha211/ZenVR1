using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.XR.Interaction.Toolkit;

public class FishingRod : MonoBehaviour
{
    public float angleThreshold = 15.0f;
    public float checkDuration = 5f;
    public float checkInterval = 60f;
    public int totalChecks = 5;
    public PlayableDirector gameSuccessTimeline;
    public PlayableDirector gameFailureTimeline;

    private XRBaseInteractable interactable;
    private bool isGrabbed = false;
    private Quaternion initialRotation;
    private Vector3 initialDirection;
    private int successCount = 0;
    private int failureCount = 0;
    private int currentCheck = 0;

    void Start()
    {
        interactable = GetComponent<XRBaseInteractable>();

        // Let's add a null check for safety
        if(interactable == null)
        {
            Debug.LogError("XRBaseInteractable component is missing on the GameObject.");
            return;
        }

        interactable.selectEntered.AddListener(Grabbed);
        interactable.selectExited.AddListener(Released);

    }

    private void Grabbed(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        initialRotation = transform.rotation;
        initialDirection = transform.up; // Considering the rod swings up and down primarily
        Debug.Log("Rod Grabbed, Initial Rotation: " + initialRotation.eulerAngles);
        StartCoroutine(CheckSwing());
    }

    private void Released(SelectExitEventArgs args)
    {
        isGrabbed = false;
       
        StopAllCoroutines();
        Debug.Log("Rod Released.");
    }

    private IEnumerator CheckSwing()
    {
        while (isGrabbed && currentCheck < totalChecks)
        {
            yield return new WaitForSeconds(checkInterval - checkDuration);

            float maxAngleDifference = 0f;
            float checkEndTime = Time.time + checkDuration;
            Debug.Log("Check starting...");

            while (Time.time < checkEndTime)
            {
                Vector3 currentDirection = transform.up;
                float angleDifference = Vector3.Angle(initialDirection, currentDirection);

                Debug.Log("Current Angle Difference: " + angleDifference);

                maxAngleDifference = Mathf.Max(maxAngleDifference, angleDifference);
                yield return null;
            }

            Debug.Log("Check ended. Max Angle: " + maxAngleDifference);

            if (maxAngleDifference > angleThreshold)
            {
                failureCount++;
                Debug.Log("Swing too large - Check failed!");
            }
            else
            {
                successCount++;
                Debug.Log("Swing within range - Check successful!");
            }

            currentCheck++;
            initialDirection = transform.up;
            yield return new WaitForSeconds(2);
            
        }

        DetermineOutcome();
    }

   private void DetermineOutcome()
    {
        if (successCount > 3)
        {
            Debug.Log("Game Success! Success count: " + successCount);
            PlayTimeline(gameSuccessTimeline);
        }
        else if (failureCount > 3)
        {
            Debug.Log("Game Failure! Failure count: " + failureCount);
            PlayTimeline(gameFailureTimeline);
        }

        successCount = 0;
        failureCount = 0;
        currentCheck = 0;
    }

    private void PlayTimeline(PlayableDirector timeline)
    {
        if(timeline != null)
        {
            timeline.Play();
        }
        else
        {
            Debug.LogError("PlayableDirector for the timeline is not assigned.");
        }
    }

    void OnDestroy()
    {
        if(interactable != null) // Safety check in case it was missing and listeners weren't added
        {
            interactable.selectEntered.RemoveListener(Grabbed);
            interactable.selectExited.RemoveListener(Released);
        }
    }
}
