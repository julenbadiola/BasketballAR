using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class OnlineEvents : MonoBehaviourPun
{
    [SerializeField]
    private GameObject oponentBallPrefab;

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
            ReceivedOponentThrow((object[]) obj.CustomData);
        }
    }

    private void ReceivedOponentThrow(object[] data){
        GameObject ball = Instantiate(oponentBallPrefab, Vector3.zero, Quaternion.identity);
        ball.GetComponent<OponentBallScript>().SetShootInfo(data[0].ToString(), (Vector3) data[1], (Vector3) data[2]);
    }
}
