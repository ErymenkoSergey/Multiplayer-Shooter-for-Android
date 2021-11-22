using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineTrain : MonoBehaviour
{
    public static EngineTrain inst;

    AudioSource audioSource;

    private void Start()
    {
        inst = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void EngineGo()
    {
        audioSource.Play();
    }
}
