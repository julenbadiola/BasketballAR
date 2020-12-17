using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class FinalScoreListing : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_Text _player;
    [SerializeField]
    private TMP_Text _score;
    [SerializeField]
    private TMP_Text _throws;
    [SerializeField]
    private Image _color;

    public void SetData(string nickname, int color, int throws, int score){
        _player.text = nickname;
        _color.color = MasterManager.getColorByIndex(color);
        _throws.text = throws.ToString();
        _score.text = score.ToString();
    }
}
