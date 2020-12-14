using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OponentBallScript : MonoBehaviour
{

    void Start()
    {
        transform.SetParent (GameObject.Find("ImageTarget").transform, true);
    }

    public void SetShootInfo(string oponent, Vector3 position, Vector3 force){
        gameObject.name = oponent + " ball";
        gameObject.GetComponent<Rigidbody>().AddForce(force);
    }

}
