using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class RandomCustomPropertyGenerator : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text; 

    private ExitGames.Client.Photon.Hashtable _myCustomProperties = new ExitGames.Client.Photon.Hashtable();

    private void SetCustomColor()
    {
        int result = MasterManager.getRandomColorIndex();
        _text.text = result.ToString();

        _myCustomProperties["Color"] = result;
        PhotonNetwork.SetPlayerCustomProperties(_myCustomProperties);
        //PhotonNetwork.LocalPlayer.CustomProperties = _myCustomProperties;
    }

    public void OnClick_Button()
    {
        SetCustomColor();
    }
}
