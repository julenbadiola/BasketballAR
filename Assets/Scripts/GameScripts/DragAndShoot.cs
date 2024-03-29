using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;

[RequireComponent(typeof(Rigidbody))]
public class DragAndShoot : MonoBehaviour
{
    private Transform cam;
    private Rigidbody rb;
    private BallPositionFixer posFixer;

    //Variables for shooting
    private Vector3 mousePressDownPos;
    private Vector3 mouseReleasePos;
    [SerializeField]
    private float forceMultiplier = 60000f;
    public Color color;
    private PhotonView PV;
    private OnlineEvents _onlineEvents;
    void Start()
    {
        _onlineEvents = GameObject.Find("ScoreCanvas").GetComponent<OnlineEvents>();
        transform.SetParent(GameObject.Find("ImageTarget").transform, true);

        rb = GetComponent<Rigidbody>();
        ResetBall();

        PV = GetComponent<PhotonView>();
        color = MasterManager.GetColorOfPlayer(PV.Owner);

        if (PV.IsMine)
        {
            MyBall();
        }
        else
        {
            OponentBall();
        }
    }

    private void OponentBall()
    {
        Debug.Log("OPONENT BALL");
        transform.gameObject.tag = "OponentBall";

    }
    private void MyBall()
    {
        Debug.Log("MY BALL");
        cam = GameObject.Find("ARCamera").transform;

        //If is my ball, put in front of camera
        posFixer = gameObject.AddComponent<BallPositionFixer>();
        transform.gameObject.tag = "Ball";
    }

    private void OnMouseDown()
    {
        mousePressDownPos = Input.mousePosition;
    }

    private void OnMouseUp()
    {
        Debug.Log("UP");
        mouseReleasePos = Input.mousePosition;
        Shoot(mouseReleasePos - mousePressDownPos);
    }

    void Shoot(Vector3 Force)
    {
        //If my ball, let shoot it
        if (PV.IsMine)
        {
            //If is already shoot, don´t shoot again
            if (posFixer.IsShoot)
                return;

            Force.Normalize();

            Vector3 force = Force * (forceMultiplier / 2) + (cam.forward * forceMultiplier);
            rb.useGravity = true;
            rb.AddForce(force);

            //Do not fix position in fron of camera anymore
            posFixer.IsShoot = true;

            //Reset ball in 1.5 seconds
            StartCoroutine(resetBallAfterThrow());
        }

    }

    private void ResetBall()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }

    private IEnumerator resetBallAfterThrow()
    {
        _onlineEvents.AddThrow();
        //Wait 3 seconds (1.5 bc timeScale = 2) after throw and create new ball
        yield return new WaitForSeconds(3);
        ResetBall();
        posFixer.IsShoot = false;
    }

}