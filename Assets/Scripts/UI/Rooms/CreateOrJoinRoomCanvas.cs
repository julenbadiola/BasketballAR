using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateOrJoinRoomCanvas : MonoBehaviour
{
    [SerializeField]
    private CreateRoomMenu _createRoomMenu;

    private RoomsCanvases _roomsCanvases;
    
    [SerializeField]
    private RoomListingMenu _roomListingsMenu;

    public void FirstInitialize(RoomsCanvases canvases){
        _roomsCanvases = canvases;
        _createRoomMenu.FirstInitialize(canvases); 
        _roomListingsMenu.FirstInitialize(canvases);
    }
}
