using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSpace
{
    public class PlayerActions : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform cam;
    
    bool holdingBall = true;
    GameObject ball;
    
    void Start()
    {
        resetBall();
    }
    
    void Update()
    {
        if(holdingBall == true){
            if(Input.GetMouseButtonDown(0)){
                holdingBall = false;
            }
        }
        
    }
    
    void resetBall(){
        holdingBall = true;
        ball = Instantiate(ballPrefab, Vector3.zero, Quaternion.identity);
        ball.transform.SetParent(gameObject.transform, false);
    }

    private void LateUpdate(){
        if(holdingBall == true){
            float x = cam.position.x;
            float y = cam.position.y - 6f;
            float z = cam.position.z + 6f;
            ball.transform.position = new Vector3(x, y, z);
        }
    }
}

}
