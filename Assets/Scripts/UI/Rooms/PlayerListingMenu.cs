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
    private Transform _content;
    [SerializeField]
    private PlayerListing _playerListing;
    [SerializeField]
    private TMP_Text _readyUpText;

    private List<PlayerListing> _listings = new List<PlayerListing>();
    private RoomsCanvases _roomsCanvases;
    private bool _ready = false;

    private void Awake()
    {
        GetCurrentRoomPlayers();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SetReadyUp(false);
    }

    private void SetReadyUp(bool state)
    {
        _ready = state;
        if(state)
        {
            _readyUpText.text = "Ready";
        }else{
            _readyUpText.text = "Not ready";
        }
        
    }

    public void FirstInitialize(RoomsCanvases canvases)
    {
        _roomsCanvases = canvases;
    }

    private void GetCurrentRoomPlayers()
    {
        if(!PhotonNetwork.IsConnected){
            return;
        }
        if(PhotonNetwork.CurrentRoom == null || PhotonNetwork.CurrentRoom.Players == null){
            return;
        }
        foreach (KeyValuePair<int, Player> playerInfo in PhotonNetwork.CurrentRoom.Players)
        {
            AddPlayerListing(playerInfo.Value);
        }
    }

    public override void OnLeftRoom(){
        _content.DestroyChildren();
    }

    private void AddPlayerListing(Player player)
    {
        PlayerListing listing = Instantiate (_playerListing, _content);
        if(listing != null)
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
        int index = _listings.FindIndex( x => x.Player == otherPlayer);
        if(index != -1){
            Destroy(_listings[index].gameObject);
            _listings.RemoveAt(index);
        }
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        _roomsCanvases.CurrentRoomCanvas.LeaveRoomMenu.OnClick_LeaveRoom();
    }

    public void OnClick_StartGame(){
        //Sólo el creador puede iniciar la partida
        if(PhotonNetwork.IsMasterClient)
        {
            //Si alguno de los jugadores no está listo, no empieza
            for (int i = 0; i < _listings.Count; i++)
            {
                if(_listings[i].Player != PhotonNetwork.LocalPlayer)
                {
                    if(!_listings[i].Ready)
                    {
                        return;
                    }
                }
            }

            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.LoadLevel(1);
        }
    }

    public void OnClick_ReadyUp()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            SetReadyUp(!_ready);
            base.photonView.RPC("RPC_ChangeReadyState", RpcTarget.MasterClient, PhotonNetwork.LocalPlayer, _ready);
            //Para evitar el tampering
            //base.photonView.RpcSecure("RPC_ChangeReadyState", RpcTarget.MasterClient, true, PhotonNetwork.LocalPlayer, _ready);
        }
    }
    [PunRPC]
    private void RPC_ChangeReadyState(Player player, bool ready)
    {
        int index = _listings.FindIndex( x => x.Player == player);
        if(index != -1)
        {
            _listings[index].Ready = ready;
        }
    }
}
