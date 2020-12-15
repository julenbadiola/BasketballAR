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
    [SerializeField]
    private Image _color;
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
            if(changedProps.ContainsKey("Color")){
                SetPlayerText(target);
            }
        }
    }

    private void SetPlayerText(Player player)
    {
        int result = -1;
        //Buscar color que nadie tenga
        if(player.CustomProperties.ContainsKey("Color")){
            result = (int) player.CustomProperties["Color"];
        }
        string playerInfoText = player.NickName;
        if(player == PhotonNetwork.LocalPlayer){
            playerInfoText += " (tú)";
        }
        _color.color = MasterManager.getColorByIndex(result);
        _text.text = playerInfoText;
    }
}
