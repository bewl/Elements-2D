using UnityEngine;
using System.Collections;

public class test : MonoBehaviour {

	public float updateInterval = 0.5f;
 
private float accum = 0.0f; 
private float frames = 0;
private float timeleft; 
 
void Start()
{
    if( !guiText )
    {
        print ("FramesPerSecond needs a GUIText component!");
        enabled = false;
        return;
    }
    timeleft = updateInterval;  
}
 
void Update()
{
    timeleft -= Time.deltaTime;
    accum += Time.timeScale/Time.deltaTime;
    ++frames;
 
    if( timeleft <= 0.0 )
    {
        // display two fractional digits (f2 format)
        guiText.text = "" + (accum/frames).ToString("f2");
        timeleft = updateInterval;
        accum = 0.0f;
        frames = 0;
    }
}
}
