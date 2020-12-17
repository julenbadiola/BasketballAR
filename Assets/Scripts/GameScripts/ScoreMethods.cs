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
    private Dictionary<int, DragAndShoot> _playerBalls = new Dictionary<int, DragAndShoot>();

    public void AddScoreListing(Player player)
    {
        ScoreListing listing = Instantiate(_scoreListing, _scoresPanelList);
        if (listing != null)
        {
            listing.SetInitialInfo(player);
            _scoreBoard.Add(player.ActorNumber, 0);
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

    public void AddPlayerBallToList(PhotonView view)
    {
        int id = view.Owner.ActorNumber;
        DragAndShoot script = view.gameObject.GetComponent<DragAndShoot>();
        _playerBalls.Add(id, script);
    }

    public void SendScoreToMaster()
    {
        //Send I scored to master
        PhotonNetwork.RaiseEvent(
            MasterManager.SCORE_UPDATE,
            PhotonNetwork.LocalPlayer.ActorNumber,
            RaiseEventOptions.Default,
            SendOptions.SendReliable
        );
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
            //Sort by score
            var sortedScoreBoard = _scoreBoard.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            _scoreBoard = sortedScoreBoard.ToDictionary(pair => pair.Key, pair => pair.Value);

            Dictionary<string, int[]> finalData = new Dictionary<string, int[]>();
            foreach (var item in _scoreBoard)
            {
                int player_id = item.Key;
                Player player = PhotonNetwork.LocalPlayer.Get(player_id);

                //Get throws and score of each player and make Vector2
                int col = MasterManager.GetColorIndexOfPlayer(player);
                int thr = _playerBalls[player_id].throws;
                int scr = item.Value;
                finalData[player.NickName] = new int[] { col, thr, scr };
            }

            Debug.Log("SENDING SCORE FINALIZATION " + finalData.ToString());
            PhotonNetwork.RaiseEvent(
                MasterManager.SCORE_REACHED,
                finalData,
                RaiseEventOptions.Default,
                SendOptions.SendReliable
            );

            MasterManager.sceneSwapper.SetFinalScoreScene(finalData);
        }
    }

    


}
