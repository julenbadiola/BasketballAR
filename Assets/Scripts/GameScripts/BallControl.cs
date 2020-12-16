using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class BallControl : MonoBehaviour
{
    [SerializeField]
	private GameObject ballPrefab;
    [SerializeField]
    private Transform cam;
    private GameObject ball;

    public void initialize()
    {
        ball = PhotonNetwork.Instantiate("Temp/ballPrefab", new Vector3(0, 0, 0), Quaternion.identity);
        ball.name = PhotonNetwork.LocalPlayer.NickName + "Ball";
    }

    void Start()
    {
        initialize();
    }

}
