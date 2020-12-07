using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreArea : MonoBehaviour
{
    public ParticleSystem winEffect;

    private void OnTriggerEnter(Collider coll){
        if(coll.CompareTag("Ball")){
            Debug.Log("CANASTA");
            winEffect.Play();
        }
    }
}
