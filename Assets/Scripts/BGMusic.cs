using UnityEngine;

public class BGMusic : MonoBehaviour
{
    public static BGMusic inst;

    private void Awake()
    {
        inst = this;
    }

    internal void DestroyGame()
    {
        Destroy(gameObject);
    }
}
