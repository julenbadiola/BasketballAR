using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using TMPro;
public class ScoreArea : MonoBehaviour
{
    public ParticleSystem winEffect;

    private string _localNickname;
    private Color _localPlayerColor;
    private ScoreEvents _scoreboard;
    [SerializeField]
    private TextMeshProUGUI alert;
    void Start(){
        _localNickname = PhotonNetwork.LocalPlayer.NickName;
        _localPlayerColor = MasterManager.GetColorOfPlayer(PhotonNetwork.LocalPlayer);
        _scoreboard = GameObject.Find("ScoreCanvas").GetComponent<ScoreEvents>();
    }
    
    private void OnTriggerEnter(Collider coll){
        //If the ball scored owner = LocalPlayer => send "I scored"
        if(coll.CompareTag("Ball"))
        {
            //StartCoroutine(ShowAlert());
            _scoreboard.AddScore(_localNickname);
            playWinEffect(coll.gameObject.GetComponent<DragAndShoot>().color);
        }
    }

    IEnumerator ShowAlert(){
        alert.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        alert.gameObject.SetActive(false);
    }

    public void playWinEffect(Color color)
    {
        winEffect.Stop();
        winEffect.startColor = color;
        winEffect.Play();
    }
}
