using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 using System;

public class GifImage : MonoBehaviour
{
    private int framesCount = 125;
    public Texture2D[] frames;
    private int fps = 10;

    void Start(){
        frames = new Texture2D[framesCount];
        foreach (Texture2D item in Resources.LoadAll ("gif",typeof(Texture2D)))
        {
            string numstr = item.name.Replace("frame_", "");
            int num = Convert.ToInt32(numstr);
            frames[num] = item;
        }
    }
    
    void Update()
    {
        int index = (int) (Time.time * fps) % framesCount;
        GetComponent<RawImage>().texture = frames[index];
    }
}
