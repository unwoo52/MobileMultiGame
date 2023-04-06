using MyNamespace;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace MyNamespace
{
    

    public class JoinOhterGame : MonoBehaviourPunCallbacks
    {
        [SerializeField] private Transform _roomListContent;
        [SerializeField] private GameObject _roomListItemPrefab;
        private List<RoomInfo> _roomList = new List<RoomInfo>();

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            _roomList = roomList;
            RefreshRoomList();
        }
        private void RefreshRoomList()
        {
            foreach (Transform child in _roomListContent)
            {
                Destroy(child.gameObject);
            }

            foreach (RoomInfo roomInfo in _roomList)
            {
                GameObject roomListItem = Instantiate(_roomListItemPrefab, _roomListContent);
                RoomListItem item = roomListItem.GetComponent<RoomListItem>();
                item.SetRoomInfo(roomInfo);
            }
        }
        public void JoinRoom(RoomInfo roomInfo)
        {
            PhotonNetwork.JoinRoom(roomInfo.Name);
        }
        public void myJoinRoom(RoomInfo roomInfo)
        {
            PhotonNetwork.JoinRoom(roomInfo.Name);
        }
        /*9
        public Dropdown dropdown;

        private void OnEnable()
        {
            UpdateDropdown();
        }

        public void UpdateDropdown(List<RoomInfo> roomList)
        {
            dropdown.ClearOptions(); // 기존 드롭다운 옵션 삭제

            List<string> options = new List<string>();
            foreach (RoomInfo roomInfo in roomList)
            {
                options.Add(roomInfo.Name);
            }

            dropdown.AddOptions(options);
        }
        */

        string _gamename;
        public void ActiveJoinCanvas()
        {
            MainSceneCanvasManager.Instance.JoinOtherGameCanvas.SetActive(true);
        }

        public void InActiveJoinCanvas()
        {
            MainSceneCanvasManager.Instance.JoinOtherGameCanvas.SetActive(false);
        }


        public void SetGameName(string name)
        {
            _gamename = name;
        }
    }
}

