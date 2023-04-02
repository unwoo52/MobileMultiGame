using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyNamespace
{
    public interface ISetMapdata
    {
        void SetMapdata(string gamename, string mapname);
    }

    public class SavedGame : MonoBehaviour, ISetMapdata
    {
        private string _gameName;
        private string _mapName;
        public void SetMapdata(string gamename, string mapname)
        {
            transform.GetChild(0).GetComponent<TMP_Text>().text = gamename;
            _mapName = mapname;
            _gameName = gamename;
            Button button = GetComponent<Button>();
            if (button != null)
            {
                // On Click 이벤트에 대한 설정을 합니다.
                button.onClick.AddListener(Connet);
            }
        }
        public void Connet()
        {
            MainSceneCanvasManager.Instance.Connect(_gameName, _mapName);
        }
    }
}