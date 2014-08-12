using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{

    public GameObject Player;
	// Use this for initialization
	void Start ()
	{
	    Player = gameObject.transform.FindChild("Player").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
