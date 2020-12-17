using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
using System.Linq;

public class ScoreArea : MonoBehaviour
{
    public ParticleSystem winEffect;

    private string _localNickname;
    private Color _localPlayerColor;
    [SerializeField]
    private TextMeshProUGUI alert;
    [SerializeField]
    private OnlineEvents _onlineEvents;
    void Start(){
        _localPlayerColor = MasterManager.GetColorOfPlayer(PhotonNetwork.LocalPlayer);
    }
    
    private void OnTriggerEnter(Collider coll){
        //If the ball scored owner = LocalPlayer => send "I scored"
        if(coll.CompareTag("Ball"))
        {
            //StartCoroutine(ShowAlert());
            _onlineEvents.AddScore();
            playWinEffect(_localPlayerColor);
        }
    }

    public void playWinEffect(Color color)
    {
        winEffect.Stop();
        winEffect.startColor = color;
        winEffect.Play();
    }

    /*
    IEnumerator ShowAlert(){
        alert.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        alert.gameObject.SetActive(false);
    }*/
}
