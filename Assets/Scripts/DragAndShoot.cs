using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DragAndShoot : MonoBehaviour
{
    private Vector3 mousePressDownPos;
    private Vector3 mouseReleasePos;

    private Rigidbody rb;
    private GameObject hoop;
    private Transform cam;
    
    private bool isShoot;
    private float forceMultiplier;
    private BallControl main;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        main = GameObject.Find("MAINSCR").GetComponent<BallControl>();
        forceMultiplier = main.ballForce;
        hoop = GameObject.Find("basketball_hoop");
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
        rb.AddForce(Force * (forceMultiplier / 2));
        rb.AddForce(cam.forward * forceMultiplier);
        isShoot = true;
        main.throwedBall();
    }
    
}