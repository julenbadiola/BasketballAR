using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class ScoreListing : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_Text _player;
    [SerializeField]
    private TMP_Text _score;
    [SerializeField]
    private Image _color;

    public void SetInitialInfo(Player player)
    {
        _player.text = player.NickName;
        
        if(player.CustomProperties.ContainsKey("Color")){
            int res = (int) player.CustomProperties["Color"];
            _color.color = MasterManager.getColorByIndex(res);
        }
        _score.text = "0";
    }

    public void SetScore(int score){
        _score.text = score.ToString();
    }
}
