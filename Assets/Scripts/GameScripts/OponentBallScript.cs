using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class OponentBallScript : MonoBehaviour
{
    public Color color;
    private Rigidbody rb;

    void Awake()
    {
        //Destroy after 3 seconds
        rb = gameObject.GetComponent<Rigidbody>();
        Destroy(gameObject, 3.0f);
    }

    public void SetShootInfo(Player oponent, Quaternion rotation, Vector3 position, Vector3 force)
    {
        //Set color (winEffect)
        if(oponent.CustomProperties.ContainsKey("Color")){
            color = MasterManager.getColorByIndex((int) oponent.CustomProperties["Color"]);
        }
        //Set ball name
        gameObject.name = oponent.NickName + " ball";

        Debug.Log("Lanzada " + gameObject.name);

        //Set ball rotation
        transform.localRotation = rotation;
        //Set ball position
        transform.localPosition = position;
        //Set ball force
        rb.useGravity = true;
        Vector3 realForce = transform.parent.TransformDirection(force);
        rb.AddForce(realForce);
    }

}
