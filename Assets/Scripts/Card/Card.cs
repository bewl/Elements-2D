using System;
using System.Collections.Generic;
using Holoville.HOTween.Plugins.Core;
using UnityEngine;
using System.Collections;
using Holoville.HOTween;
public class Card : MonoBehaviour
{
    public GameObject go;
    public GameObject PlayerGO;
    public string CardName;
    public CardType CardType;
    public ResourceType ResourceType;
    public IList<CardResource> Cost;
    public Texture cardTexture;

    public bool IsAnimating         = false;
    public bool IsResetting         = false;
    public bool IsDragging          = false;
    public bool IsInHand            = true;
    public bool IsSelected          = false;
    public bool IsSummonSickness    = false;
    public bool IsInPlayableArea    = false;
    public bool IsSlotted           = false;

    public int HandIndex    = 0;
    public int Attack       = 0;
    public int Defense      = 0;
    
    public float BuildTime;
    public Vector3 startingPosition = new Vector3();
    public Vector3 startingScale    = new Vector3();
    public Transform myTransform;

    public MouseInput gm;

    void Awake()
    {
        myTransform = gameObject.GetComponent<Transform>();
    }

    void Start()
    {
        //gm = GameObject.Find("Main Camera").GetComponent<MouseInput>();
        startingScale = transform.localScale;
    }

    void OnMouseEnter()
    {
        Hover(true);
    }

    void OnMouseExit()
    {
        Hover(false);
    }

    public void SetCardData(Card card, int index)
    {
        IsInHand = true;
        Attack = card.Attack;
        CardType = card.CardType;
        Defense = card.Defense;
        ResourceType = card.ResourceType;
        CardName = card.CardName;
        HandIndex = index;
    }

    private void Hover(bool isUp)
    {
        if (!MouseInput.isDragging && !IsAnimating && IsInHand)
        {
            HOTween.To(gameObject.transform, 0.2f, new TweenParms()
                .Prop("position", new Vector3(gameObject.transform.position.x, (isUp ? startingPosition.y + 2 : startingPosition.y), gameObject.transform.position.z))
                .Loops(1)
                .Ease(EaseType.EaseInCirc));
        }
    }

    void AnimationDone()
    {
        IsAnimating = false;
    }

    void Update()
    {
        if (IsInHand)
        {
            if (IsSelected)
            {
                MouseInput.moveCard += this.dragCard;
                MouseInput.resetCard += this.resetCard;
                MouseInput.animateCardToPlayableArea += this.animateCardToPlayableArea;
                MouseInput.slotCard += this.slotCard;
            }

            else
            {
                MouseInput.moveCard -= this.dragCard;
                MouseInput.resetCard -= this.resetCard;
                MouseInput.animateCardToPlayableArea -= this.animateCardToPlayableArea;
            }
        }
    }

    public void dragCard(GameObject go, Ray ray, Vector3 offSet)
    {
        if (go == gameObject && IsInHand)
        {
            if (!IsDragging)
            {
                PlayerGO = GameObject.Find("Player");
                var player = PlayerGO.GetComponent<Player>();
                IsDragging = true;
                Debug.Log("Dragging card: " + CardName);
                player.AdjustCardsInHand();
                HOTween.To(transform, 0.25f,
                    new TweenParms().Prop("localScale", new Vector3(transform.localScale.x * 2, transform.localScale.y, transform.localScale.z * 2)));
            }
            gameObject.transform.position = new Vector3(ray.origin.x + offSet.x, ray.origin.y + offSet.y, -11);
        }
    }

    public void resetCard(GameObject go)
    {
        if (go == gameObject && !IsResetting && IsInHand)
        {
            Debug.Log("Resettting " + CardName);
            Debug.Log("StartingPos.X " + startingPosition.x);
            IsResetting = true;
            
            PlayerGO = GameObject.Find("Player");
            var player = PlayerGO.GetComponent<Player>();
            IsDragging = false;
            
            var resetSequence = new Sequence(new SequenceParms().Loops(1).OnComplete(() =>
            {
                IsResetting = false;
                MouseInput.isDragging = false;
                //gameObject.transform.position = new Vector3(myTransform.position.x, myTransform.position.y, startingPosition.z);
                player.AdjustCardsInHand();
            }));

            resetSequence.Insert(0, HOTween.To(transform, 0.25f,
                new TweenParms().Prop("localScale",
                    new Vector3(startingScale.x, startingScale.y, startingScale.z))));
            resetSequence.Insert(0, HOTween.To(transform, 0.25f, new TweenParms()
                .Prop("position", startingPosition)));

            resetSequence.Play();
        }
    }

    public void animateCardToPlayableArea(GameObject gobj, bool hasEnetered)
    {
        if (gobj == gameObject)
        {
            var newScale = new Vector3();

            if (!IsInPlayableArea && hasEnetered)
            {
                IsInPlayableArea = true;
                HOTween.To(transform, 0.25f,
                    new TweenParms().Prop("localScale", new Vector3(startingScale.x, startingScale.y, startingScale.z)));
            }
            else if (IsInPlayableArea && !hasEnetered)
            {
                IsInPlayableArea = false;
                HOTween.To(transform, 0.25f,
                    new TweenParms().Prop("localScale", new Vector3(transform.localScale.x * 2, transform.localScale.y, transform.localScale.z * 2)));
            }
        }
    }

    public void slotCard(GameObject gobj, GameObject slot)
    {
        if (gobj == gameObject && IsInHand)
        {
            IsInHand = false;
            MouseInput.isDragging = false;
            slot.GetComponent<SpriteRenderer>().enabled = false;

            slot.GetComponent<Slot>().slottedCard = gameObject;

            PlayerGO = GameObject.Find("Player");
            var player = PlayerGO.GetComponent<Player>();
            player.InPlay.Add(gameObject);

            HOTween.To(transform, 0.25f, new TweenParms().Prop("position", slot.transform.position));

        }
    }

    void Initialize()
    {

    }
}

public enum CardType
{
    Creature,
    Spell,
    Resource
}

public enum ResourceType
{
    Fire,
    Water,
    Wind,
    Earth
}


public class CardResource
{
    public int Amount { get; set; }
    public ResourceType Resource { get; set; }
}
