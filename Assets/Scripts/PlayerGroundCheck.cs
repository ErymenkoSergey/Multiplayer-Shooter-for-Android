using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
	PlayerController playerController;

	private void Awake()
	{
		playerController = GetComponentInParent<PlayerController>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (PlayerController.inst.isJump == true)
        {
			if (other.gameObject == playerController.gameObject)
				return;

			playerController.SetGroundedState(true);
			playerController.JumpStop();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if(other.gameObject == playerController.gameObject)
			return;

		playerController.SetGroundedState(false);
	}

	private void OnTriggerStay(Collider other)
	{
		if(other.gameObject == playerController.gameObject)
			return;

		playerController.SetGroundedState(true);
	}
}