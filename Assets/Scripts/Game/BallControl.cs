using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallControl : MonoBehaviour
{
    //public
	public GameObject ballPrefab;
    public Transform cam;
    public float ballDistance = 10f;
    public float ballForce = 15000f;

    //Private
    GameObject ball;
    bool holdingBall = true;
    
    
    public void initialize(){
        Debug.Log("New ball");
        ball = Instantiate(ballPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        holdingBall = true;
    }

    void Start(){
        initialize();
    }

    public void throwedBall()
    {
        holdingBall = false;
        StartCoroutine(waitAndDestroy());
    }
    IEnumerator waitAndDestroy(){
        yield return new WaitForSeconds(3);
        Debug.Log("RESETTING");
        Destroy(ball);
        initialize();
    }

    void LateUpdate(){
        if(holdingBall == true){
            Vector3 pos = cam.position + (cam.forward * ballDistance) - new Vector3(0f, 5f, 0f);
            ball.transform.position = pos;
        }
    }

}
