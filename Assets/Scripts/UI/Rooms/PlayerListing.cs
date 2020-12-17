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
    [SerializeField]
    private RawImage _icon;
    [SerializeField]
    private Texture notReadyIcon;
    [SerializeField]
    private Texture readyIcon;

    public Player Player { get; private set; }
    public bool Ready = false;

    public void SetPlayerInfo(Player player)
    {
        Player = player;
        SetPlayerText(player);
    }


    public void UpdateIcon()
    {
        if (Ready && (_icon.texture != readyIcon))
        {
            _icon.texture = readyIcon;
        }
        else if (!Ready && (_icon.texture != notReadyIcon))
        {
            _icon.texture = notReadyIcon;
        }

    }

    public override void OnPlayerPropertiesUpdate(Player target, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(target, changedProps);
        if (target != null && target == Player)
        {
            if (changedProps.ContainsKey("Color"))
            {
                SetPlayerText(target);
            }
        }
    }

    private void SetPlayerText(Player player)
    {
        int result = -1;
        //Buscar color que nadie tenga
        if (player.CustomProperties.ContainsKey("Color"))
        {
            result = (int)player.CustomProperties["Color"];
        }
        string playerInfoText = player.NickName;
        if (player == PhotonNetwork.LocalPlayer)
        {
            playerInfoText += " (tú)";
        }

        //If is master, change the icon to ready
        if (PhotonNetwork.IsMasterClient)
        {
            if (player == PhotonNetwork.LocalPlayer)
            {
                _icon.texture = readyIcon;
            }
        }
        //If is not, dont show icons
        else
        {
            _icon.gameObject.SetActive(false);
        }

        _color.color = MasterManager.getColorByIndex(result);
        _text.text = playerInfoText;
    }
}
