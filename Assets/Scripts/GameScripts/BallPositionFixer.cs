using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPositionFixer : MonoBehaviour
{
    private Transform cam;
    private bool isShoot = false;
    public bool IsShoot {
        get {
            return isShoot;
        }
        set {
            isShoot = value;
        }
    }

    void Start()
    {
        cam = GameObject.Find("ARCamera").transform;
        transform.SetParent (GameObject.Find("ImageTarget").transform, true);
    }

    void Update(){
        transform.LookAt(cam);
    }

    void LateUpdate(){
        //Update ball´s position regarding the camera
        if(!isShoot){
            transform.position = cam.position + (cam.forward * 10f) - (cam.up * 5f);
        }
    }

}
