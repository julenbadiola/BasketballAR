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
    private Vector3 initialPos;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        hoop = GameObject.Find("basketball_hoop");
        cam = GameObject.Find("ARCamera").transform;
        initialPos = transform.position;
    }

    IEnumerator wait(){
        yield return new WaitForSeconds(3);
        reset();
    }

    public void reset(){
        Debug.Log("Resetting");
        rb.useGravity = false;  
        isShoot = false;
        transform.position = initialPos;
        
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

    private float forceMultiplier = 2;
    void Shoot(Vector3 Force)
    {
        if(isShoot)
            return;
        rb.useGravity = true;
        
        /*Debug.Log("X" + Force.x);
        Debug.Log("Y" + Force.y);
        Debug.Log("Z" + Force.z);
        //rb.AddForce(new Vector3(Force.x,Force.y,Force.y) * forceMultiplier);
        rb.AddForce(cam.forward * forceMultiplier, ForceMode.Force);*/


        Force.Normalize();
        Debug.Log("The direction is "+ Force);
        rb.AddForce(Force * 1000);

        isShoot = true;
        StartCoroutine(wait());
    }
    
}