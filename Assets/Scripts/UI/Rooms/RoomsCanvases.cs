using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsCanvases : MonoBehaviour
{
    [SerializeField]
    private CreateOrJoinRoomCanvas _createOrJoinRoomCanvas;
    [SerializeField]
    public CreateOrJoinRoomCanvas CreateOrJoinRoomCanvas{
        get {
            return _createOrJoinRoomCanvas;
        }
    }


    [SerializeField]
    private CurrentRoomCanvas _currentRoomCanvas;
    [SerializeField]
    public CurrentRoomCanvas CurrentRoomCanvas{
        get {
            return _currentRoomCanvas;
        }
    }

    private void Awake(){
        FirstInitialize();
    }

    private void FirstInitialize(){
        CreateOrJoinRoomCanvas.FirstInitialize(this);
        CurrentRoomCanvas.FirstInitialize(this);
    }


}
