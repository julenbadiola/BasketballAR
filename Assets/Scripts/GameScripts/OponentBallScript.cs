using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class OponentBallScript : MonoBehaviour
{
    public Color color;
    
    void Start()
    {
        //Destroy after 3 seconds
        Destroy(gameObject, 3.0f);
    }

    public void SetShootInfo(Player oponent, Quaternion rotation, Vector3 position, Vector3 force){
        //Set color (winEffect)
        if(oponent.CustomProperties.ContainsKey("Color")){
            color = MasterManager.getColorByIndex((int) oponent.CustomProperties["Color"]);
        }
        //Set ball name
        gameObject.name = oponent.NickName + " ball";
        //Set ball rotation
        transform.localRotation = rotation;
        //Set ball position
        transform.localPosition = position;
        //Set ball force
        gameObject.GetComponent<Rigidbody>().AddForce(force);
    }

}
