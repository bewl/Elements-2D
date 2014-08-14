using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Utilities;
using Holoville.HOTween;
using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public int deckSize = 60;
    public int startingHandSize = 7;
    public int Health = 100;
    public IList<Card> Deck = new List<Card>();
    public IList<GameObject> Hand = new List<GameObject>();
    public float DealTime = 1.0f;
    public Vector3 DeckLocation = new Vector3();
    public float handSpread = 0.4f;
    public GameObject CardPrefab;
    
    // Use this for initialization
    void Start()
    {
        InitDeck();

    }

    public void InitDeck()
    {
        GenerateDeck();
        StartCoroutine(DealCards(startingHandSize));
        
    }


    //REMOVE ME!
    public void GenerateDeck()
    {
        
        Deck = new List<Card>();

        var cardTypes = Enum.GetValues(typeof(CardType));
        var resTypes = Enum.GetValues(typeof(ResourceType));
        for (var i = 0; i < deckSize; i++)
        {
            var r = new System.Random();
            var card = new Card()
            {
                Attack = r.Next(0, 12),
                Defense = r.Next(0, 24),
                CardType = (CardType)cardTypes.GetValue(r.Next(cardTypes.Length)),
                ResourceType = (ResourceType)resTypes.GetValue(r.Next(resTypes.Length)),
                CardName = "Card #" + i
            };

            Deck.Add(card);
        }

        Deck.Shuffle();
    }

    void IssueHand()
    {
        
    }

    public void AdjustCardsInHand()
    {
        var cardData = Hand.Select(card => card.GetComponent<Card>()).Where(a => !a.IsDragging).ToList();

        for (var i = 0; i < cardData.Count; i++)
        {
            var go = cardData[i].gameObject;

            var endPosition = Vector3.zero;
            endPosition.x = i * (handSpread*10);
            endPosition.z = i * -0.1f;

            iTween.MoveTo(go, iTween.Hash("position", endPosition, "time", 0.5f));
        }
    }

    private IEnumerator DealCards(int cardAmount)
    {
        MouseInput.isAnimating = true;
        for (int i = 0; i < cardAmount; i++)
        {
            var card = Deck.First();

            
            Deck.RemoveAt(0);

            StartCoroutine(DealCard(card, i));

            yield return new WaitForSeconds(DealTime);
            
        }
    }

    private IEnumerator DealCard(Card card, int index)
    {
        var endPosition = Vector3.zero;
        endPosition.x = index * (handSpread * 10);
        endPosition.z = index * -0.1f;

        var startPosistion = new Vector3(60, 0, index * -0.1f);
        
        GameObject go = (GameObject)Instantiate(CardPrefab, startPosistion, Quaternion.Euler(new Vector3(-90, 0, 0)));
        var data = go.GetComponent<Card>();

        //
        //go.transform.Find("Card Front").renderer.material.SetTextureScale("_MainTex", new Vector2(-1, -1));
        go.transform.Find("Card Front").renderer.material.mainTexture = AssignCardTexture(card.ResourceType);
        //go.transform.Find("Card Front").renderer.material = AssignCardMaterial(card.ResourceType);

        

        var attackTextMesh = go.transform.Find("AttackText").GetComponent<TextMesh>();
        float pixelRatio = (Camera.main.orthographicSize * 2.0f) / Camera.main.pixelHeight;
        attackTextMesh.transform.localScale = new Vector3(pixelRatio * 10.0f, pixelRatio * 10.0f, pixelRatio * 0.1f);
        attackTextMesh.text = card.Attack.ToString();

        go.transform.Find("DefenseText").GetComponent<TextMesh>().text = card.Defense.ToString();
        
        data.go = go;

        data.SetCardData(card, index);
        
        data.startingPosition = endPosition;
        Hand.Add(go);

        //start with the card face down
        go.transform.rotation = Quaternion.Euler(-90, 0, -180);

        var dealSequence = new Sequence(new SequenceParms().Loops(1));

        dealSequence.Insert(0,
            HOTween.To(go.transform, 1f,
                new TweenParms().Prop("rotation", new Vector3(-90, go.transform.rotation.y, 0))));
        dealSequence.Insert(0,
            HOTween.To(go.transform, 1f,
                new TweenParms().Prop("position", endPosition)));

        dealSequence.Play();

        yield return new WaitForSeconds(0);
        
    }

    public void StopRoutines()
    {
        StopAllCoroutines();
        MouseInput.isAnimating = false;
    }
    Material AssignCardMaterial(ResourceType type)
    {
        var mat = Resources.Load("Materials/odyssey-rpg-cards-perseus", typeof(Material)) as Material;
        switch (type)
        {

            case ResourceType.Earth:
                mat = Resources.Load("Materials/odyssey-rpg-cards-perseus-EARTH", typeof(Material)) as Material;
                break;
            case ResourceType.Fire:
                mat = Resources.Load("Materials/odyssey-rpg-cards-perseus-FIRE", typeof(Material)) as Material;
                break;
            case ResourceType.Water:
                mat = Resources.Load("Materials/odyssey-rpg-cards-perseus-WATER", typeof(Material)) as Material;
                break;
            case ResourceType.Wind:
                mat = Resources.Load("Materials/odyssey-rpg-cards-perseus-WIND", typeof(Material)) as Material;
                break;
        }

        return mat;
    }

    Texture AssignCardTexture(ResourceType type)
    {
        Texture tex = Resources.Load("Textures/odyssey-rpg-cards-perseus", typeof(Texture)) as Texture;
        switch (type)
        {

            case ResourceType.Earth:
                tex = Resources.Load("Textures/odyssey-rpg-cards-perseus-EARTH", typeof(Texture)) as Texture;
                break;
            case ResourceType.Fire:
                tex = Resources.Load("Textures/odyssey-rpg-cards-perseus-FIRE", typeof(Texture)) as Texture;
                break;
            case ResourceType.Water:
                tex = Resources.Load("Textures/odyssey-rpg-cards-perseus-WATER", typeof(Texture)) as Texture;
                break;
            case ResourceType.Wind:
                tex = Resources.Load("Textures/odyssey-rpg-cards-perseus-WIND", typeof(Texture)) as Texture;
                break;
        }

        return tex;
    }


    void DrawCard()
    {

    }
}
