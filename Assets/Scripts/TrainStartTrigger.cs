using UnityEngine;

public class TrainStartTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            TrainControllers.inst.PlayGudokTraine();
            TrainControllers.inst.EngineOn();
            Destroy(gameObject);
        }
    }
}
