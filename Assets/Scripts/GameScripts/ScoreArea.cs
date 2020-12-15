using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class ScoreArea : MonoBehaviour
{
    public ParticleSystem winEffect;

    void Start(){
        if(PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Color")){
            int res = (int) PhotonNetwork.LocalPlayer.CustomProperties["Color"];
            winEffect.startColor = MasterManager.getColorByIndex(res);
        }
    }
    private void OnTriggerEnter(Collider coll){
        if(coll.CompareTag("Ball")){
            Debug.Log("CANASTA");
            winEffect.Play();
        }
        /*
        else if(coll.CompareTag("OponentBall"))
        {
            Debug.Log("CANASTA ADVERSARIO");
            winEffect.startColor = new Color(1, 0, 1, .5f);
        }*/
    }
}
