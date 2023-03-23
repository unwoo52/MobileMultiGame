using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Realtime;

namespace Photon.Pun.Demo.PunBasics
{
	public class GameManager : MonoBehaviourPunCallbacks
    {
        [Tooltip("The prefab to use for representing the player")]
        [SerializeField]
        private GameObject playerPrefab;

        static public GameManager Instance;
		void Start()
		{
			Instance = this;
			if (playerPrefab == null) { // #Tip Never assume public properties of Components are filled up properly, always check and inform the developer of it.

				Debug.LogError("<Color=Red><b>Missing</b></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
			} 
			else 
			{
				if (PlayerManager.LocalPlayerInstance==null)
				{
				    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
					// we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
					PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f,5f,0f), Quaternion.identity, 0);
				}
				else
				{
					Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
				}
			}
		}
		public override void OnLeftRoom()
		{
			SceneManager.LoadScene(0);
		}
		public bool LeaveRoom()
		{
			return PhotonNetwork.LeaveRoom();
		}
	}
}