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
        //If is already shoot, don´t shoot again
        if(posFixer.IsShoot)
            return;

        Force.Normalize();
        
        Vector3 force = Force * (forceMultiplier / 2) + (cam.forward * forceMultiplier);
        rb.useGravity = true;
        rb.AddForce(force);

        //Do not fix position in fron of camera anymore
        posFixer.IsShoot = true;
        
        //Send data force, position, rotation to other players
        Vector3 forceToSend = transform.parent.InverseTransformDirection(force);
        object[] datas = new object[] {
            transform.localRotation,
            transform.localPosition,
            forceToSend
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