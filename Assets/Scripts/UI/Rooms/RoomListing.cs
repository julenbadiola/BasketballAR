using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class RoomListing : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text;
    public RoomInfo RoomInfo;

    public void SetRoomInfo(RoomInfo roomInfo)
    {
        _text.text = roomInfo.MaxPlayers + ", " + roomInfo.Name;
        RoomInfo = roomInfo;
    }

    public void OnClick_Button(){
        PhotonNetwork.JoinRoom(RoomInfo.Name);
    }
}
