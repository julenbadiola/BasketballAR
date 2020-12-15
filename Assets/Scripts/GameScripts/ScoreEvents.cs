using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class ScoreEvents : MonoBehaviourPun
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private ScoreListing _scoreListing;
    [SerializeField]
    private GameObject _scorePanel;
    private bool isMaster;
    private Dictionary<string, ScoreListing> _listings = new Dictionary<string, ScoreListing>();
    private Dictionary<string, int> _scoreBoard = new Dictionary<string, int>();
    
    void Start(){
        isMaster = PhotonNetwork.IsMasterClient;
        foreach (KeyValuePair<int, Photon.Realtime.Player> row in PhotonNetwork.CurrentRoom.Players)
        {   
            AddScoreListing(row.Value);            
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

    private void NetworkingClient_EventReceived(EventData obj)
    {
        
        if(obj.Code == MasterManager.SCORE_UPDATE)
        {
            if(isMaster){
                //Get nickname of thrower
                string nickname = (string) obj.CustomData;

                //Get his score and update it
                int score = _scoreBoard[nickname] + 1;
                _scoreBoard[nickname] = score;

                //Update the score panel
                ScorePanelUpdate(_scoreBoard);
                
                //Send scores to all players
                PhotonNetwork.RaiseEvent(
                    MasterManager.SCORE_NORMALIZATION, 
                    _scoreBoard, 
                    RaiseEventOptions.Default,
                    SendOptions.SendReliable
                );
            }
        }

        if(obj.Code == MasterManager.SCORE_NORMALIZATION)
        {   
            if(!isMaster){
                ScorePanelUpdate((Dictionary<string, int>) obj.CustomData);
            }
        }
    }

    private void ScorePanelUpdate(Dictionary<string, int> scores){
        Debug.Log(scores);
        
        foreach (KeyValuePair<string, int> row in scores)
        {   
            Debug.Log(row.Key);
            Debug.Log(row.Value);

            _listings[row.Key].SetScore(row.Value);
        }
    }

    private void AddScoreListing(Player player)
    {
        ScoreListing listing = Instantiate (_scoreListing, _content);
        if(listing != null)
        {
            listing.SetInitialInfo(player);
            _scoreBoard.Add(player.NickName, 0); 
            _listings.Add(player.NickName, listing);
        }
    }
}
