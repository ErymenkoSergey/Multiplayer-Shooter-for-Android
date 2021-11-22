using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Mine : MonoBehaviour
{
    [SerializeField]
    private AudioClip mineInstalled, mineActivated;

    private void PlaySound(AudioClip clip, float vol = 1)
    {
        AudioSource.PlayClipAtPoint(clip, transform.position, vol);
    }

    private void MineInstalled()
    {
        PlaySound(mineInstalled);
    }
    private void MineActivated()
    {
        PlaySound(mineActivated);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ground")
            MineInstalled();
        if (collision.gameObject.tag == "Metal")
            MineInstalled();
        if (collision.gameObject.tag == "Beton")
            MineInstalled();
        if (collision.gameObject.tag == "Wood")
            MineInstalled();

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1111, 0));
            MineActivated();
            Destroy(gameObject);
        }
    }
}