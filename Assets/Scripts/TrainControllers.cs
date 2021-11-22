using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class TrainControllers : MonoBehaviour
{
    public static TrainControllers inst;

    [SerializeField]
    private AudioClip gudokTraine, engineTraine, engineTraineGo, moveTraine;

    private float speeForce = 0f;

    private float speedAddTiming = 2f;

    private bool isMoveTrain = false;

    private bool volumeSounds;

    private void Awake()
    {
        inst = this;
        volumeSounds = true;
    }

    private void PlaySound(AudioClip clip, float vol)
    { // add Check Soud Game Setting - on?
        if(volumeSounds)
           AudioSource.PlayClipAtPoint(clip, transform.position, vol);
    }

    private void Update()
    {
        if (isMoveTrain)
            transform.position += new Vector3(speeForce * Time.deltaTime, 0f, 0f);
    }

    internal void PlayGudokTraine()
    {
        PlaySound(gudokTraine, 1f);
    }
    private void PlayEngineTraine(float vol)
    {
        PlaySound(engineTraine, vol);
    }

    private void PlayEngineGoTraine(float vol)
    {
        PlaySound(engineTraineGo, 1f);
    }

    protected void PlayMoveTraine()
    {
        //PlaySound(moveTraine, 1f);
    }

    internal async void EngineOn()
    {
        PlayEngineTraine(0.2f);
        await Task.Delay(17000);
        PlayEngineTraine(1f);
        await Task.Delay(60000);
        MoveTrain();
        await Task.Delay(16000);
        StopAddSpeed();
        await Task.Delay(8000);
        speedAddTiming = 1f;
        RunAddsSpeed();
    }

    private void MoveTrain()
    {
        EngineTrain.inst.EngineGo();
        PlayEngineGoTraine(1f);
        isMoveTrain = true;
        RunAddsSpeed();
    }

    #region Controler Spee Train
    IEnumerator runnerSpeedAddForce;

    internal void RunAddsSpeed()
    {
        runnerSpeedAddForce = IERunAddSpeed();
        StartCoroutine(runnerSpeedAddForce);
    }

    internal void StopAddSpeed()
    {
        if (runnerSpeedAddForce != null)
            StopCoroutine(runnerSpeedAddForce);
    }

    IEnumerator IERunAddSpeed()
    {
        while (true)
        {
            speeForce++;
            yield return new WaitForSeconds(speedAddTiming);
        }
    }
    #endregion
}
