using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager inst;

    [SerializeField]
    private AudioClip gameSoundClip, buttonClip, 
        gameOverClip, appleHitClip,knifeFireClip, 
        appleRewardClip, wheelClip, knifeClip, 
        unlockedClip, fightStartClip, fightendClip, 
        stepClip, runClip, jumpClip, shootPistolClip, 
        shootAKClip, openDoorClip, teleportOneClip,
        teleportTwoClip, teleportThreeClip,
        findWeapons, dieClip;


    private void Awake()
    {
        inst = this;
    }

    private void PlaySound(AudioClip clip, float vol = 1)
    {
        if (GameManager.inst.SoundSettings)
            if (Camera.main != null)
                AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, vol);
    }

    #region SoundMenuAndSettings
    public void PlayGameSound()
    {
        PlaySound(gameSoundClip);
    }
    public void PlayButton()
    {
        PlaySound(buttonClip);
    }
    public void PlayAppleHit()
    {
        PlaySound(appleHitClip);
    }
    public void PlayWheelHit()
    {
        PlaySound(wheelClip);
    }
    public void PlayKnifeHit()
    {
        PlaySound(knifeClip);
    }
    public void PlayUnlock()
    {
        PlaySound(unlockedClip);
    }
    public void PlayKnifeFire()
    {
        PlaySound(knifeFireClip);
    }
    public void PlayBossStartFight()
    {
        PlaySound(fightStartClip);
    }
    public void PlayBossEndFight()
    {
        PlaySound(fightendClip);
    }
    public void PlayGameOver()
    {
        PlaySound(gameOverClip);
    }
    public void PlayAppleReward()
    {
        PlaySound(appleRewardClip);
    }

    #endregion

    #region GameSound

    public void StepSound()
    {
        PlaySound(stepClip);
    }

    public void RunSound()
    {
        PlaySound(runClip);
    }

    public void JumpSound()
    {
        PlaySound(jumpClip);
    }

    public void ShootPistolSound()
    {
        PlaySound(shootPistolClip);
    }

    public void ShootAKSound()
    {
        PlaySound(shootAKClip);
    }

    public void OpenDoorSound()
    {
        PlaySound(openDoorClip);
    }

    public void TeleportOneSound()
    {
        PlaySound(teleportOneClip);
    }

    public void TeleportTwoSound()
    {
        PlaySound(teleportTwoClip);
    }

    public void TeleportThreeSound()
    {
        PlaySound(teleportThreeClip);
    }

    public void FindWeapons()
    {
        PlaySound(findWeapons);
    }

    public void Die()
    {
        PlaySound(dieClip);
    }
    #endregion
}
