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
            var cardData = card.GetComponent<Card>();
            MouseInput.animateCardToPlayableArea -= cardData.animateCardToPlayableArea;
            MouseInput.resetCard -= cardData.resetCard;
            MouseInput.moveCard -= cardData.dragCard;
            MouseInput.slotCard -= cardData.slotCard;

            Destroy(card);
        }
        foreach(var card in player.InPlay)
        {
            var cardData = card.GetComponent<Card>();
            MouseInput.animateCardToPlayableArea -= cardData.animateCardToPlayableArea;
            MouseInput.resetCard -= cardData.resetCard;
            MouseInput.moveCard -= cardData.dragCard;
            MouseInput.slotCard -= cardData.slotCard;

            Destroy(card);
        }

        player.Hand.Clear();
        player.InPlay.Clear();

        player.StopRoutines();
        player.InitDeck();
        
    }
}
