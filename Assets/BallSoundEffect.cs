using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class BallSoundEffect : MonoBehaviour
{
    void OnCollisionEnter()
    {
        GetComponent<AudioSource>().Play();
    }
}