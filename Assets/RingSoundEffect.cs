using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class RingSoundEffect : MonoBehaviour
{
    void OnCollisionEnter()
    {
        GetComponent<AudioSource>().Play();
    }
}