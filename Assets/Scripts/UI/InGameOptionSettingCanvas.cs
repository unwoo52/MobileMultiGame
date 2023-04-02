using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyNamespace
{
    public class InGameOptionSettingCanvas : MonoBehaviour
    {
        public void SaveGame()
        {            
            GameDataManager.Instance.AllGameDataSave();
        }

        public void SaveAndQuitGame()
        {

            GameDataManager.Instance.AllGameDataSave();
            InGameManager.Instance.LeaveRoom();

        }
    }
}
