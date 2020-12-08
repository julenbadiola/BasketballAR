using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class TestConnect : MonoBehaviourPunCallbacks
{
    void Start()
    {
        print("Connecting to server.");
        //AuthenticationValues.authValues = new AuthenticationValues("0");
        //PhotonNetwork.AuthValues = authValues;
        PhotonNetwork.SendRate = 20;
        PhotonNetwork.SerializationRate = 5;
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster (){
        print("Connected to server.");
        print(PhotonNetwork.LocalPlayer.NickName);
        if(!PhotonNetwork.InLobby){
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnDisconnected(Photon.Realtime.DisconnectCause cause){
        print("Disconnected from server. Cause: " + cause.ToString());
    }

    public override void OnJoinedLobby(){
        print("Connected to lobby.");
    }
}
