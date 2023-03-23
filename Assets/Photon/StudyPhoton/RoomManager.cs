using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public TMP_Text playerCountText;
    public TMP_Text roomNameText;

    void Start()
    {
        if (PhotonNetwork.InRoom)
        {
            UpdateRoomInfo();
        }
    }

    public override void OnJoinedRoom()
    {
        UpdateRoomInfo();
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        UpdateRoomInfo();
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        UpdateRoomInfo();
    }

    private void UpdateRoomInfo()
    {
        if (PhotonNetwork.InRoom)
        {
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            playerCountText.text =playerCount.ToString();

            string roomName = PhotonNetwork.CurrentRoom.Name;
            roomNameText.text = roomName;
        }
    }
}