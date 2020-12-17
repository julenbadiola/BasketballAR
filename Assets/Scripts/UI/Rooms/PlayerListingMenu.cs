using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class PlayerListingMenu : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject readyButton;
    [SerializeField]
    private GameObject startButton;
    [SerializeField]
    private Transform _content;
    [SerializeField]
    private PlayerListing _playerListing;
    [SerializeField]
    private TMP_Text _readyUpText;
    [SerializeField]
    private RawImage _icon;
    [SerializeField]
    private Texture notReadyIcon;
    [SerializeField]
    private Texture readyIcon;

    private List<PlayerListing> _listings = new List<PlayerListing>();
    private RoomsCanvases _roomsCanvases;
    private bool _ready = false;
    [SerializeField]
    private int WAIT_TO_REFRESH_PLAYERS_STATUS = 1;
    private void Awake()
    {
        GetCurrentRoomPlayers();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        bool isMaster = PhotonNetwork.IsMasterClient;
        if (!isMaster)
        {
            startButton.SetActive(false);
            readyButton.SetActive(true);
        }
        else
        {
            readyButton.SetActive(false);
        }
        
        SetReadyUp(false);
        StartCoroutine(checkPlayers());
    }

    IEnumerator checkPlayers()
    {
        while (true)
        {
            bool res = checkReadyPlayers();
            startButton.GetComponent<Button>().interactable = res;
            yield return new WaitForSeconds(WAIT_TO_REFRESH_PLAYERS_STATUS);
        }
    }

    private void SetReadyUp(bool state)
    {
        _ready = state;
        string hex = "";
        if (state)
        {
            _readyUpText.text = "¡ESTOY LISTO!";
            _icon.texture = readyIcon;
            hex = "D4FFAC";
        }
        else
        {
            _readyUpText.text = "NO ESTOY LISTO";
            _icon.texture = notReadyIcon;
            hex = "FF7C83";
            
        }
        /*Color color;
        ColorUtility.TryParseHtmlString (hex, out color);
        _readyUpText.transform.parent.gameObject.GetComponent<Image>().color = color;
        */
    }

    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
    }

    private void GetCurrentRoomPlayers()
    {
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }
        if (PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null)
        {
            return;
        }
        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
    }

    public override void OnLeftRoom()
    {
        _content.DestroyChildren();
    }

    private void AddPlayerListing(Player player)
    {
        PlayerListing listing = Instantiate(_playerListing, _content);
        if (listing != null)
        {
            listing.SetPlayerInfo(player);
            _listings.Add(listing);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddPlayerListing(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        int index = _listings.FindIndex(x => x.Player == otherPlayer);
        if (index != -1)
        {
            Destroy(_listings[index].gameObject);
            _listings.RemoveAt(index);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        _roomsCanvases.CurrentRoomCanvas.LeaveRoomMenu.OnClick_LeaveRoom();
    }
    public bool checkReadyPlayers()
    {
        bool allReady = true;
        for (int i = 0; i < _listings.Count; i++)
        {
            _listings[i].UpdateIcon();
            if (_listings[i].Player != PhotonNetwork.LocalPlayer)
            {
                if (!_listings[i].Ready)
                {
                    allReady = false;
                }
            }
        }
        return allReady;
    }

    public void OnClick_StartGame()
    {
        //Sólo el creador puede iniciar la partida
        if (PhotonNetwork.IsMasterClient)
        {
            if (checkReadyPlayers())
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                PhotonNetwork.LoadLevel(1);
            }
        }
    }

    public void OnClick_ReadyUp()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Color"))
        {
            int res = (int)PhotonNetwork.LocalPlayer.CustomProperties["Color"];
            if (MasterManager.isColorIndexValid(res))
            {

                if (!PhotonNetwork.IsMasterClient)
                {
                    SetReadyUp(!_ready);
                    base.photonView.RPC("RPC_ChangeReadyState", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer, _ready);
                    //Para evitar el tampering
                    //base.photonView.RpcSecure("RPC_ChangeReadyState", RpcTarget.MasterClient, true, PhotonNetwork.LocalPlayer, _ready);
                }

            }
        }

    }
    [PunRPC]
    private void RPC_ChangeReadyState(Player player, bool ready)
    {
        int index = _listings.FindIndex(x => x.Player == player);
        if (index != -1)
        {
            _listings[index].Ready = ready;
        }
    }
}
