using UnityEngine;

public class ObjectSound : MonoBehaviour
{
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();
    }

    // This function is called when the object collides with another
   void OnTriggerEnter(Collider other)
{
    if (other.gameObject.CompareTag("stick"))
    {
        audioSource.Play();
    }
}

}
