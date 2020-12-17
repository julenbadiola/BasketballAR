using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

[RequireComponent(typeof(ScoreMethods))]
public class OnlineEvents : MonoBehaviourPun
{
    [SerializeField]
    private ScoreArea _scoreArea;

    private bool isMaster;
    private ScoreMethods _scoreMethods;
    
    void Start()
    {
        
        isMaster = PhotonNetwork.IsMasterClient;
        _scoreMethods = GetComponent<ScoreMethods>();
        foreach (KeyValuePair<int, Photon.Realtime.Player> row in PhotonNetwork.CurrentRoom.Players)
        {   
            _scoreMethods.AddScoreListing(row.Value);            
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    public void AddScore()
    {
        if(isMaster)
        {
            _scoreMethods.AddScore(PhotonNetwork.LocalPlayer.ActorNumber);
        }
        else
        {
            MasterManager.SendNotificationToMaster(MasterManager.SCORE_UPDATE);
        }
    }

    public void AddThrow()
    {
        if(isMaster)
        {
            _scoreMethods.AddThrow(PhotonNetwork.LocalPlayer.ActorNumber);
        }
        else
        {
            MasterManager.SendNotificationToMaster(MasterManager.BALL_THROW);
        }
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
        Player sender = PhotonNetwork.CurrentRoom.GetPlayer(obj.Sender);

        if(obj.Code == MasterManager.BALL_THROW)
        {
            if(isMaster)
            {
                //Add throw to the id of thrower
                _scoreMethods.AddThrow((int) obj.CustomData);
            }
            Debug.Log("Throw de " + sender.NickName);
        }

        if(obj.Code == MasterManager.SCORE_UPDATE)
        {
            if(isMaster)
            {
                //Add score to the id of thrower
                _scoreMethods.AddScore((int) obj.CustomData);
            }
            
            Color color = MasterManager.GetColorOfPlayer(sender);
            Debug.Log("Canasta de " + sender.NickName);
            _scoreArea.playWinEffect(color);
        }        

        if(obj.Code == MasterManager.SCORE_NORMALIZATION)
        {   
            if(!isMaster)
            {
                _scoreMethods.ScorePanelUpdate((Dictionary<int, int>) obj.CustomData);
            }
        }

        if(obj.Code == MasterManager.SCORE_REACHED)
        {   
            if(!isMaster)
            {
                Debug.Log("SCORE FINALIZATION RECEIVED");
                MasterManager.sceneSwapper.SetFinalScoreScene((Dictionary<string, int[]>) obj.CustomData);
            }
        }
    }

    
    
}
