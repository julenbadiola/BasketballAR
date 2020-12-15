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
    [SerializeField]
    private Transform imageTarget;

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
        Player player = PhotonNetwork.CurrentRoom.GetPlayer(obj.Sender);

        if(obj.Code == MasterManager.BALL_THROW_EVENT)
        {
            ReceivedOponentThrow(player, (object[]) obj.CustomData);
        }
    }

    private void ReceivedOponentThrow(Player player, object[] data){
        //Instantiate as a child of ImageTarget object
        GameObject oponentBall = Instantiate(oponentBallPrefab, Vector3.zero, Quaternion.identity, imageTarget);

        //Get data and set shoot info
        oponentBall.GetComponent<OponentBallScript>().SetShootInfo(
            player,
            (Quaternion) data[0], 
            (Vector3) data[1], 
            (Vector3) data[2]
        );
    }
}
