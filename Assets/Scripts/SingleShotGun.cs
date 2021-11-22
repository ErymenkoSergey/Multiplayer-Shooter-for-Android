using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class SingleShotGun : Gun
{
	[SerializeField]
	private Camera cameraMain;

	[SerializeField]
	private Camera camPricel;

	PhotonView PV;

	private void Awake()
	{
		PV = GetComponent<PhotonView>();
	}

    #region Shoot
    public override void Use()
	{
		Shoot();
	}

    public override void InstMines()
    {
		ShootMines();
	}

    public override void UsePricel()
    {
		ShootPricel();
	}

    private void Shoot()
	{
		Ray ray = cameraMain.ViewportPointToRay(new Vector3(0.5f, 0.5f));
		ray.origin = cameraMain.transform.position;
		Raycast(ray, 3);
	}

	private void ShootMines()
	{
		Ray ray = cameraMain.ViewportPointToRay(new Vector3(0.5f, 0.5f));
		ray.origin = cameraMain.transform.position;
		Raycast(ray, 2);
	}

	private void ShootPricel()
	{
		Ray ray = camPricel.ViewportPointToRay(new Vector3(0.5f, 0.5f));
		ray.origin = camPricel.transform.position;
		Raycast(ray, 1);
	}

	private void Raycast(Ray ray, int shootType)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            switch (shootType)
            {
                case 1:
					hit.collider.gameObject.GetComponent<IDamageable>()?.Identifications(((GunInfo)itemInfo).id);
					PV.RPC("RPC_ShootID", RpcTarget.All, hit.point, hit.normal);
					hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
                    PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
                    break;
                case 2:
                    hit.collider.gameObject.GetComponent<IDamageable>()?.MineActives();
                    PV.RPC("RPC_Mine", RpcTarget.All, hit.point, hit.normal);
                    break;
                case 3:
					hit.collider.gameObject.GetComponent<IDamageable>()?.Identifications(((GunInfo)itemInfo).id);
					PV.RPC("RPC_ShootID", RpcTarget.All, hit.point, hit.normal);
					hit.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(((GunInfo)itemInfo).damage);
					PV.RPC("RPC_Shoot", RpcTarget.All, hit.point, hit.normal);
					break;
            }
        }
    }
    #endregion

    #region RPC
    [PunRPC]
	private void RPC_Shoot (Vector3 hitPosition, Vector3 hitNormal)
	{
		Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);

		if(colliders.Length != 0)
		{
			GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.001f, 
			Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
			Destroy(bulletImpactObj, GameMeaning.BULLETDRAWINGTIME);
			bulletImpactObj.transform.SetParent(colliders[0].transform);
			SoundManager.inst?.ShootPistolSound();
		}
	}

	[PunRPC]
	private void RPC_Mine(Vector3 hitPosition, Vector3 hitNormal)
	{
        GameObject mineImpactObj = Instantiate(mineJump, new Vector3(transform.position.x +1f, transform.position.y -1f, transform.position.z), transform.rotation);
    }

	[PunRPC]
	private void RPC_ShootID(Vector3 hitPosition, Vector3 hitNormal)
    {
		Collider[] colliders = Physics.OverlapSphere(hitPosition, 0.3f);

		if (colliders.Length != 0)
		{
			GameObject bulletImpactObj = Instantiate(bulletImpactPrefab, hitPosition + hitNormal * 0.001f,
			Quaternion.LookRotation(hitNormal, Vector3.up) * bulletImpactPrefab.transform.rotation);
			Destroy(bulletImpactObj, GameMeaning.BULLETDRAWINGTIME);
			bulletImpactObj.transform.SetParent(colliders[0].transform);
			SoundManager.inst?.ShootPistolSound();
		}
	}
    #endregion
}
