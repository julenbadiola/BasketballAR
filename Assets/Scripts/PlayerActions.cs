using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public GameObject ball;
    public Transform cam;
    public float ballDistance = 2f;
    public float ballForce = 250f;
    bool holdingBall = true;
    Rigidbody ballRB;
    
    void Start()
    {
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

    private void LateUpdate(){
        if(holdingBall == true){
            ball.transform.position = cam.position + cam.forward * ballDistance;

        }
    }
}
