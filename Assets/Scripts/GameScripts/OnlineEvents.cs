using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class OnlineEvents : MonoBehaviourPun
{
    private eventcodes eventcodes;

    void Awake()
    {
        eventcodes = GameObject.Find("online").GetComponent<eventcodes>();
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
        if(obj.Code == eventcodes.BALL_THROW_EVENT)
        {
            object[] datas = (object[]) obj.CustomData;
        }
        Debug.Log("RECEIVEEEED");
    }
}
