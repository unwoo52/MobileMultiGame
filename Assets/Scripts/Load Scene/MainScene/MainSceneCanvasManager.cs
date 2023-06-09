using System.Collections.Generic;
using UnityEngine;

namespace MyNamespace
{
    public interface ISetActive_CreateCanvas
    {
        void SetActive_CreateCanvas(bool param);
    }
    public class MainSceneCanvasManager : MonoBehaviour, ISetActive_CreateCanvas
    {
        #region singleton
        private static MainSceneCanvasManager _instance = null;

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(this.gameObject);
            }
            GoToDefaultScene();
        }

        public static MainSceneCanvasManager Instance
        {
            get
            {
                if (null == _instance)
                {
                    return null;
                }
                return _instance;
            }
        }
        #endregion
        [SerializeField] private List<GameObject> scenes;
        [SerializeField] private GameObject _createCanvas;
        [SerializeField] private GameObject _joinOtherGameCanvas;
        public GameObject JoinOtherGameCanvas { get { return _joinOtherGameCanvas;} }
        public void SetActive_CreateCanvas(bool isActive)
        {
            _createCanvas.SetActive(isActive);
        }



        public void GoToGameStartScene()
        {
            ChangeScene("Game Start Menu Canvs");
        }

        public void GoToStoreScene()
        {
            ChangeScene("Store Canvas");
        }

        public void GoToOptionScene()
        {
            ChangeScene("Option Canvas");
        }

        public void GoToQuitGameScene()
        {
            ChangeScene("Quit Canvas");
        }

        public void GoToDefaultScene()
        {
            ChangeScene("Main Menu Canvas");
        }
        public void BackButton()
        {
            GoToDefaultScene();
        }

        private void ChangeScene(string sceneName)
        {
            foreach (GameObject sceneObject in scenes)
            {
                bool isActive = sceneObject.name == sceneName;
                sceneObject.SetActive(isActive);
            }
        }

    }
}