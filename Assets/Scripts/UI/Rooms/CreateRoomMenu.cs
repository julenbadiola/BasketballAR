using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

public class CreateRoomMenu : MonoBehaviourPunCallbacks
{
    
    [SerializeField]
    private Text _roomName;

    private RoomsCanvases _roomsCanvases;

    public void FirstInitialize(RoomsCanvases canvases){
        _roomsCanvases = canvases;
    }

    public void OnClick_CreateRoom()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        RoomOptions options = new RoomOptions();
        options.BroadcastPropsChangeToAll = true;
        options.PublishUserId = true;
        options.MaxPlayers = 4;

        PhotonNetwork.JoinOrCreateRoom(_roomName.text, options, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Created room succesfully");
        _roomsCanvases.CurrentRoomCanvas.Show();

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    { 
        Debug.Log("Failed to create room." + message);
    }
}
