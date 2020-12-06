using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallControl : MonoBehaviour
{
	public GameObject ball;
    public Transform cam;
    public float ballDistance = 10f;
    public float ballForce = 15000f;
    bool holdingBall = true;
    Rigidbody ballRB;

    void Start(){
        ballRB = ball.GetComponent<Rigidbody>();
        ballRB.useGravity = false;
    }

    void Update()
    {
        if(holdingBall == true){
            if(Input.GetMouseButtonDown(0)){
                holdingBall = false;
                ballRB.useGravity = true;
                ballRB.AddForce(cam.forward * ballForce);
            }
        }
    }

    void LateUpdate(){
        if(holdingBall == true){
            ball.transform.position = cam.position + cam.forward * ballDistance;
        }
    }

}
