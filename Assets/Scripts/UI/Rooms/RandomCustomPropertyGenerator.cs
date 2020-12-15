using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class RandomCustomPropertyGenerator : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text; 
    [SerializeField]
    private Image _color; 
    private ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();
    private Player localPlayer;
    void Start(){
        //Set color when entered room
        localPlayer = PhotonNetwork.LocalPlayer;
        SetCustomColor();
    }
    private void SetCustomColor()
    {
        int i = GetUniqueColorIndexInRoom();
        _myCustomProperties["Color"] = i;
        PhotonNetwork.SetPlayerCustomProperties(_myCustomProperties);

        _color.color = MasterManager.getColorByIndex(i);
        //PhotonNetwork.LocalPlayer.CustomProperties = _myCustomProperties;
    }

    private int GetUniqueColorIndexInRoom(){
        bool freeColor = false;        
        int lastTriedColorIndex = -1;

        if (localPlayer.CustomProperties.ContainsKey("Color"))
        {
            lastTriedColorIndex = (int) localPlayer.CustomProperties["Color"];
        }
        else
        {
            lastTriedColorIndex = MasterManager.getRandomColorIndex();
        }
        //Avoid having repeated color
        while (!freeColor) 
        {
            freeColor = true;
            lastTriedColorIndex = MasterManager.getNextColorIndex(lastTriedColorIndex);
            
            Debug.Log("////////////// " + lastTriedColorIndex.ToString());
            foreach (KeyValuePair<int, Player> row in PhotonNetwork.CurrentRoom.Players)
            {   
                Player player = row.Value;
                if(player.CustomProperties.ContainsKey("Color")){
                    //The color is not free if some player already
                    bool res = (int) player.CustomProperties["Color"] == lastTriedColorIndex;
                    
                    Debug.Log(player.ToString() + " - " + player.CustomProperties["Color"].ToString() + res.ToString());
                    
                    if(res){
                        freeColor = false;
                    }
                } 
            }
        }
        return lastTriedColorIndex;
    }
    
    public void OnClick_Button()
    {
        SetCustomColor();
    }
}
