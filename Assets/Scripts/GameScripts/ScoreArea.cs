using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

public class ScoreArea : MonoBehaviour
{
    private Color localPlayerColor;
    public ParticleSystem winEffect;

    void Start(){
        if(PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Color")){
            int res = (int) PhotonNetwork.LocalPlayer.CustomProperties["Color"];
            localPlayerColor = MasterManager.getColorByIndex(res);
        }
    }
    private void OnTriggerEnter(Collider coll){
        if(coll.CompareTag("Ball")){            
            setWinEffect(localPlayerColor);

            //Send I scored!
            PhotonNetwork.RaiseEvent(
                MasterManager.SCORE_UPDATE, 
                PhotonNetwork.LocalPlayer.NickName, 
                RaiseEventOptions.Default,
                SendOptions.SendReliable
            );
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
