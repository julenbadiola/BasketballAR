using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using System.Linq;

public class ScoreMethods : MonoBehaviour
{
    [SerializeField]
    private Transform _scoresPanelList;
    [SerializeField]
    private ScoreListing _scoreListing;
    private Dictionary<string, ScoreListing> _listings = new Dictionary<string, ScoreListing>();
    //They have to be primitive since they are going to be sent
    private Dictionary<string, int> _scoreBoard = new Dictionary<string, int>();
    private Dictionary<string, DragAndShoot> _playerBalls = new Dictionary<string, DragAndShoot>();
    
    public void AddScoreListing(Player player)
    {
        ScoreListing listing = Instantiate (_scoreListing, _scoresPanelList);
        if(listing != null)
        {
            listing.SetInitialInfo(player);
            _scoreBoard.Add(player.NickName, 0); 
            _listings.Add(player.NickName, listing);
        }
    }
    public void ScorePanelUpdate(Dictionary<string, int> scores){
        foreach (KeyValuePair<string, int> row in scores)
        {   
            _listings[row.Key].SetScore(row.Value);
        }
    }

    public void AddPlayerBallToList(PhotonView view)
    {
        string nickname = view.Owner.NickName;
        DragAndShoot script = view.gameObject.GetComponent<DragAndShoot>();
        _playerBalls.Add(nickname, script);
    }

    public void SendScoreToMaster()
    {
        //Send I scored to master
        PhotonNetwork.RaiseEvent(
            MasterManager.SCORE_UPDATE, 
            PhotonNetwork.LocalPlayer.NickName, 
            RaiseEventOptions.Default,
            SendOptions.SendReliable
        );
    }
    
    public void AddScore(string nickname)
    {
        //ONLY MASTER
        
        //Get score of nickname and update it
        int score = _scoreBoard[nickname] + 1;
        _scoreBoard[nickname] = score;

        if(score < 10)
        {
            //Send scores to all players
            PhotonNetwork.RaiseEvent(
                MasterManager.SCORE_NORMALIZATION, 
                _scoreBoard, 
                RaiseEventOptions.Default,
                SendOptions.SendReliable
            );
            //Update my own scoreboard (master)
            ScorePanelUpdate(_scoreBoard);
        }
        //If someone reaches 10, send data and quitRoom
        else
        {
            //Sort by score
            var sortedScoreBoard = _scoreBoard.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            _scoreBoard = sortedScoreBoard.ToDictionary(pair => pair.Key, pair => pair.Value);

            Dictionary<string, Vector2> finalData = new Dictionary<string, Vector2>();
            foreach(var item in _scoreBoard)
            {
                string nick = item.Key;

                //Get throws and score of each player and make Vector2
                int thr = _playerBalls[nick].throws;
                int scr = item.Value;
                finalData[nick] = new Vector2 (thr, scr);
            }

            Debug.Log("SENDING SCORE FINALIZATION " + finalData.ToString());
            PhotonNetwork.RaiseEvent(
                MasterManager.SCORE_REACHED, 
                finalData, 
                RaiseEventOptions.Default,
                SendOptions.SendReliable
            );

            MasterManager.SetFinalScoreScene(finalData);
        }
    }

    
}
