using UnityEngine;
using System.Collections;

public class DealHand : MonoBehaviour
{

    public GameObject playerGO;
    public Player player;
	// Use this for initialization
	void Start ()
	{
	    player = playerGO.GetComponent<Player>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        var hand = player.Hand;

        foreach(var card in hand)
        {
            Destroy(card);
        }

        player.Hand.Clear();
        player.StopRoutines();
        player.InitDeck();
    }
}
