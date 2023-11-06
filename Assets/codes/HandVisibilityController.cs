using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HandVisibilityController : MonoBehaviour
{
    public XRBaseInteractor leftHandInteractor;
    public XRBaseInteractor rightHandInteractor;
    public GameObject leftHandModel;
    public GameObject rightHandModel;

    private void OnEnable()
    {
        // Register event handlers
        leftHandInteractor.selectEntered.AddListener(HideLeftHand);
        leftHandInteractor.selectExited.AddListener(ShowLeftHand);

        rightHandInteractor.selectEntered.AddListener(HideRightHand);
        rightHandInteractor.selectExited.AddListener(ShowRightHand);
    }

    private void OnDisable()
    {
        // Unregister event handlers
        leftHandInteractor.selectEntered.RemoveListener(HideLeftHand);
        leftHandInteractor.selectExited.RemoveListener(ShowLeftHand);

        rightHandInteractor.selectEntered.RemoveListener(HideRightHand);
        rightHandInteractor.selectExited.RemoveListener(ShowRightHand);
    }

    private void HideLeftHand(SelectEnterEventArgs arg)
    {
        leftHandModel.SetActive(false);
    }

    private void ShowLeftHand(SelectExitEventArgs arg)
    {
        leftHandModel.SetActive(true);
    }

    private void HideRightHand(SelectEnterEventArgs arg)
    {
        rightHandModel.SetActive(false);
    }

    private void ShowRightHand(SelectExitEventArgs arg)
    {
        rightHandModel.SetActive(true);
    }
}
