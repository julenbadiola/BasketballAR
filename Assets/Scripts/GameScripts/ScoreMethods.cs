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
    private int FINAL_SCORE = 2;
    [SerializeField]
    private Transform _scoresPanelList;
    [SerializeField]
    private ScoreListing _scoreListing;
    private Dictionary<int, ScoreListing> _listings = new Dictionary<int, ScoreListing>();
    //They have to be primitive since they are going to be sent
    private Dictionary<int, int> _scoreBoard = new Dictionary<int, int>();
    private Dictionary<int, int> _throwsBoard = new Dictionary<int, int>();
    public void AddScoreListing(Player player)
    {
        ScoreListing listing = Instantiate(_scoreListing, _scoresPanelList);
        if (listing != null)
        {
            listing.SetInitialInfo(player);
            _scoreBoard.Add(player.ActorNumber, 0);
            _throwsBoard.Add(player.ActorNumber, 0);
            _listings.Add(player.ActorNumber, listing);
        }
    }
    public void ScorePanelUpdate(Dictionary<int, int> scores)
    {
        foreach (KeyValuePair<int, int> row in scores)
        {
            _listings[row.Key].SetScore(row.Value);
        }
    }

    public void AddThrow(int id)
    {
        int throws = _throwsBoard[id] + 1;
        _throwsBoard[id] = throws;
    }

    public void AddScore(int id)
    {
        //ONLY MASTER

        //Get score of player and update it
        int score = _scoreBoard[id] + 1;
        _scoreBoard[id] = score;

        if (score < FINAL_SCORE)
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
            ProcessFinalResults();
        }
    }

    private void ProcessFinalResults()
    {
        Dictionary<string, int[]> finalData = new Dictionary<string, int[]>();
        //Get players in room
        foreach (KeyValuePair<int, Photon.Realtime.Player> row in PhotonNetwork.CurrentRoom.Players)
        {
            Player player = row.Value;

            //index 0
            int col = MasterManager.GetColorIndexOfPlayer(player);
            //index 1
            int thr = _throwsBoard[player.ActorNumber];
            //index 2
            int scr = _scoreBoard[player.ActorNumber];

            Debug.Log("BEFORE SENDING " + player.NickName + " " + col + " " + thr + " " + scr);
            finalData[player.NickName] = new int[] { col, thr, scr };
        }
        //Sort by score
        var sortedDict = finalData.OrderBy(x => x.Value[2]).ToDictionary(x => x.Key, x => x.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

        Debug.Log("SENDING SCORE FINALIZATION");

        PhotonNetwork.RaiseEvent(
            MasterManager.SCORE_REACHED,
            sortedDict,
            RaiseEventOptions.Default,
            SendOptions.SendReliable
        );

        MasterManager.sceneSwapper.SetFinalScoreScene(sortedDict);
    }




}
