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
            _scoreMethods.SendScoreToMaster();
        }
    }
    private void NetworkingClient_EventReceived(EventData obj)
    {
        if(obj.Code == MasterManager.PLAYER_INSTANTIATION)
        {   
            if(isMaster)
            {
                int viewId = (int) obj.CustomData;
                PhotonView view = PhotonView.Find(viewId);

                if (view != null )
                {
                    _scoreMethods.AddPlayerBallToList(view);
                    Debug.Log("ADDED BALL " + view.gameObject.ToString());
                }
                else
                {
                    Debug.LogError("No object found with viewid " + viewId);
                }
            }
        }
        if(obj.Code == MasterManager.SCORE_UPDATE)
        {
            if(isMaster)
            {
                //Add score to the id of thrower
                _scoreMethods.AddScore((int) obj.CustomData);
            }
            
            Player sender = PhotonNetwork.CurrentRoom.GetPlayer(obj.Sender);
            Color color = MasterManager.GetColorOfPlayer(sender);
            Debug.Log("Canasta de " + sender.NickName + " - Color: " + color);
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
