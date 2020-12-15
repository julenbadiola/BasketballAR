using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class ScoreArea : MonoBehaviour
{
    public ParticleSystem winEffect;

    private string _localNickname;
    private Color _localPlayerColor;
    private ScoreEvents _scoreboard;

    void Start(){
        _localNickname = PhotonNetwork.LocalPlayer.NickName;
        if(PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Color")){
            int res = (int) PhotonNetwork.LocalPlayer.CustomProperties["Color"];
            _localPlayerColor = MasterManager.getColorByIndex(res);
        }
        _scoreboard = GameObject.Find("online").GetComponent<ScoreEvents>();
    }
    
    private void OnTriggerEnter(Collider coll){
        if(coll.CompareTag("Ball")){            
            setWinEffect(_localPlayerColor);
            _scoreboard.AddScore(_localNickname);
        }

        if(coll.CompareTag("OponentBall")){
            Color color = coll.gameObject.GetComponent<OponentBallScript>().color;
            setWinEffect(color);
        }
    }

    private void setWinEffect(Color color)
    {
        winEffect.Stop();
        winEffect.startColor = color;
        winEffect.Play();
    }
}
