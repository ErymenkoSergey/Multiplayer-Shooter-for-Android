using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
	public static RoomManager Instance;

	void Awake()
	{
		if (Instance)
		{
			Destroy(gameObject);
			return;
		}
		DontDestroyOnLoad(gameObject);
		Instance = this;
	}

	public override void OnEnable()
	{
		base.OnEnable();
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public override void OnDisable()
	{
		base.OnDisable();
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
	{
		if (scene.buildIndex == GameMeaning.SCENEFIRST) // We're in the game scene
		{
			PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
		}
	}
}

//using UnityEngine;
//using Photon.Pun;
//using UnityEngine.SceneManagement;
//using System.IO;

//public class RoomManager : MonoBehaviourPunCallbacks
//{
//	public static RoomManager Instance;

//	public void Awake()
//	{
//        if (Instance)
//        {
//            Destroy(gameObject);
//            return;
//        }

//        DontDestroyOnLoad(gameObject);
//        Instance = this;
//	}

//	public override void OnEnable()
//	{
//		base.OnEnable();
//		SceneManager.sceneLoaded += OnSceneLoaded;
//	}

//	public override void OnDisable()
//	{
//		base.OnDisable();
//		SceneManager.sceneLoaded -= OnSceneLoaded;
//	}

//	private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
//	{
//		if(scene.buildIndex == GameMeaning.SCENEFIRST)
//			PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerManager"), Vector3.zero, Quaternion.identity);
//	}
//}