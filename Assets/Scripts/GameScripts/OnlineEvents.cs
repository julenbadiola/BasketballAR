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
        Debug.Log("RECEIVEEEED");
    }

    private void ReceivedOponentThrow(object[] data){
        Debug.Log("THROWER" + data[0]);
        Vector3 throwPosition = (Vector3) data[1];
        Vector3 force = (Vector3) data[2];

        Debug.Log("THROW POS" + throwPosition);
        Debug.Log("FORCE " + force);

        GameObject ball = Instantiate(oponentBallPrefab, throwPosition, Quaternion.identity);
        ball.name = data[0].ToString() + " OPONENT ball";
        ball.GetComponent<Rigidbody>().AddForce(force);
    }
}
