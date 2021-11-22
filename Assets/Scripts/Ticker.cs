using System;
using System.Collections;
using UnityEngine;

public class Ticker : MonoBehaviour
{
    public static Ticker inst;

    public static Action OnTick;

    private void Start()
    {
        inst = this;
        RunTicks();
    }

    IEnumerator runnerTicks;

    private void RunTicks()
    {
        runnerTicks = IERunTicks();
        StartCoroutine(runnerTicks);
    }

    private void StopTicks()
    {
        if (runnerTicks != null)
            StopCoroutine(runnerTicks);
    }

    IEnumerator IERunTicks()
    {
        while (true)
        {
            OnTick?.Invoke();
            yield return new WaitForSeconds(1f);
        }
    }
}
