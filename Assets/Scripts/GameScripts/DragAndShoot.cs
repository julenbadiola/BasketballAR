using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class DragAndShoot : MonoBehaviourPun
{
    private BallControl main;
    private Transform cam;
    private Rigidbody rb;
    private BallPositionFixer posFixer;

    //Variables for shooting
    private Vector3 mousePressDownPos;
    private Vector3 mouseReleasePos;
    [SerializeField]
    private float forceMultiplier = 60000f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        main = GameObject.Find("main").GetComponent<BallControl>();
        cam = GameObject.Find("ARCamera").transform;
        posFixer = gameObject.AddComponent<BallPositionFixer>();
    }

    private void OnMouseDown()
    {
        mousePressDownPos = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        mouseReleasePos = Input.mousePosition;
        Shoot(mouseReleasePos-mousePressDownPos);
    }

    void Shoot(Vector3 Force)
    {
        if(posFixer.IsShoot)
            return;

        //Set forces
        Force.Normalize();
        //Debug.Log("The direction is "+ Force);
        Vector3 force1 = Force * (forceMultiplier / 2);
        Vector3 force2 = cam.forward * forceMultiplier;
        rb.useGravity = true;
        rb.AddForce(force1 + force2);
        posFixer.IsShoot = true;

        //Send Data to server
        object[] datas = new object[] {
            PhotonNetwork.LocalPlayer,
            transform.localPosition,
            force1 + force2
        };
        PhotonNetwork.RaiseEvent(
            MasterManager.BALL_THROW_EVENT, 
            datas, 
            RaiseEventOptions.Default,
            SendOptions.SendReliable
        );
        
        StartCoroutine(main.resetBallAfterThrow());
    }
    
}