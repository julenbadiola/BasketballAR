using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class PlayerListing : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_Text _text;
    public Player Player {get; private set;}
    public bool Ready = false;

    public void SetPlayerInfo(Player player)
    {
        Player = player;
        SetPlayerText(player);
    }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(target, changedProps);
        if(target != null && target == Player)
        {
            if(changedProps.ContainsKey("RandomNumber")){
                SetPlayerText(target);
            }
        }
    }

    private void SetPlayerText(Player player)
    {
        int result = -1;
        if(player.CustomProperties.ContainsKey("RandomNumber")){
            result = (int) player.CustomProperties["RandomNumber"];
        }
        string playerInfoText = result.ToString() + ", " + player.NickName;
        if(player == PhotonNetwork.LocalPlayer){
            playerInfoText += " (you)";
        }
        _text.text = playerInfoText;
    }
}
