using Photon.Pun;
using UnityEngine;

public class InteractionsObjects : MonoBehaviour
{
    private int weaponThis;

    private void Start()
    {
        RandomWeapons();
    }

    private void RandomWeapons()
    {
        weaponThis = Random.Range(0, 8);
    }

    [PunRPC]
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
            collision.gameObject.GetComponent<PlayerController>().InteractionsOn(weaponThis);
    }

    [PunRPC]
    public void OpenObject()
    {
        SoundManager.inst.FindWeapons();
        Destroy(gameObject, GameMeaning.destroyGameInteractions);
    }
}
