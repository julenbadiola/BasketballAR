using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class DragAndShoot : MonoBehaviourPun
{
    private Vector3 mousePressDownPos;
    private Vector3 mouseReleasePos;

    private Rigidbody rb;
    private GameObject hoop;
    private Transform cam;
    
    private bool isShoot;
    private float forceMultiplier;
    private BallControl main;

    private eventcodes eventcodes;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        eventcodes = GameObject.Find("online").GetComponent<eventcodes>();
        
        main = GameObject.Find("main").GetComponent<BallControl>();
        forceMultiplier = main.ballForce;
        
        hoop = GameObject.Find("ring");
        cam = GameObject.Find("ARCamera").transform;
    }

    void Update(){
        transform.LookAt(hoop.transform);
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
        if(isShoot)
            return;
        rb.useGravity = true;

        Force.Normalize();
        Debug.Log("The direction is "+ Force);
        
        Vector3 force1 = Force * (forceMultiplier / 2);
        Vector3 force2 = cam.forward * forceMultiplier;
        rb.AddForce(force1);
        rb.AddForce(force2);

        object[] datas = new object[] {
            force1, 
            force2 
        };

        PhotonNetwork.RaiseEvent(
            eventcodes.BALL_THROW_EVENT, 
            datas, 
            RaiseEventOptions.Default,
            SendOptions.SendReliable
        );

        isShoot = true;
        main.throwedBall();
    }
    
}