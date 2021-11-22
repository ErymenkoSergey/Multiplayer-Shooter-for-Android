using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CheckingJointsRails : MonoBehaviour
{
    AudioSource audioSource;

    [SerializeField]
    private AudioClip knockRail;

    [SerializeField]
    private GameObject raicastFront;

    private RaycastHit hit;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Physics.Raycast(raicastFront.transform.position, -Vector3.up, out hit, 2))
            if (hit.transform.tag == "RailwayJunction")
                audioSource.PlayOneShot(knockRail, 0.3f);
    }
}
