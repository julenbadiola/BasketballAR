using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;

using System.Linq;

public class FinalScoreScene : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private FinalScoreListing _finalScoreListing;
    private Dictionary<Player, bool> _players = new Dictionary<Player, bool> ();
    public void SetFinalResults(Dictionary<string, int[]> data)
    {
        foreach (KeyValuePair<int, Photon.Realtime.Player> row in PhotonNetwork.CurrentRoom.Players)
        {
            if(row.Value != PhotonNetwork.LocalPlayer)
            {
                _players.Add(row.Value, false);
            }
        }

        if (PhotonNetwork.IsMasterClient)
        {
            //Show play again button
            StartCoroutine(checkPlayers());
        }
        
        //Receives dict where key is the nickname of the player and value an int array with throws and score info
        foreach (var item in data)
        {
            AddFinalScoreListing(item.Key, item.Value);
        }
    }
    public void AddFinalScoreListing(string nickname, int[] dataOfPlayer)
    {
        FinalScoreListing listing = Instantiate(_finalScoreListing, _content);
        int color = dataOfPlayer[0];
        int throws = dataOfPlayer[1];
        int score = dataOfPlayer[2];

        if (listing != null)
        {
            listing.SetData(nickname, color, throws, score);
        }
    }

    [PunRPC]
    private void RPC_ChangeReadyState(Player player, bool ready)
    {
        _players[player] = ready;
    }

    IEnumerator checkPlayers()
    {
        while (true)
        {
            bool res = checkReadyPlayers();
            startButton.interactable = res;
            yield return new WaitForSeconds(1);
        }
    }

    private bool checkReadyPlayers()
    {
        bool allReady = true;
        foreach (KeyValuePair<Player, bool> row in _players)
        {
            if (row.Key != PhotonNetwork.LocalPlayer)
            {
                if (!row.Value)
                {
                    allReady = false;
                }
            }
        }
        return allReady;
    }

    public void OnClick_PlayAgain()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (checkReadyPlayers())
            {
                PhotonNetwork.LoadLevel(1);
            }
        }
        else    
        {
            base.photonView.RPC("RPC_ChangeReadyState", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer, true);            
            startButton.interactable = false;
        }
    }
    public void OnClick_Exit()
    {
        MasterManager.sceneSwapper.ReturnRoomsScene();
    }
}