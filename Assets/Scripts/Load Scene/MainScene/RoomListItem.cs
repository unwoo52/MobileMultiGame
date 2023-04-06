using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MyNamespace
{
    public class RoomListItem : MonoBehaviour
    {
        [SerializeField] private TMP_Text _roomNameText;
        [SerializeField] private TMP_Text _roomPlayerCountText;
        [SerializeField] private Button _joinButton;

        private RoomInfo _roomInfo;

        public void SetRoomInfo(RoomInfo roomInfo)
        {
            _roomInfo = roomInfo;
            _roomNameText.text = roomInfo.Name;
            _roomPlayerCountText.text = $"{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";

            if (roomInfo.PlayerCount == roomInfo.MaxPlayers)
            {
                _joinButton.interactable = false;
            }
            else
            {
                _joinButton.interactable = true;
            }
        }

        public void OnJoinButtonClick()
        {
            FindObjectOfType<JoinOhterGame>().myJoinRoom(_roomInfo);
        }
    }

}
