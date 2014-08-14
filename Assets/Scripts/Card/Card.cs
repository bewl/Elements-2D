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

    public int HandIndex;
    public bool IsAnimating = false;
    public bool IsResetting = false;
    public bool IsDragging = false;
    public int Attack;
    public int Defense;
    public bool IsInHand = true;
    public bool IsSelected;
    public bool IsSummonSickness;
    public float BuildTime;
    public Vector3 startingPosition = new Vector3();
    public Vector3 startingScale = new Vector3();
    public Transform myTransform;

    public MouseInput gm;

    void Awake()
    {
        myTransform = gameObject.GetComponent<Transform>();
    }

    void Start()
    {
        gm = GameObject.Find("Main Camera").GetComponent<MouseInput>();
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

        //transform.Find("Defense").GetComponent<TextMesh>().text = Defense.ToString();
        //transform.Find("Attack").GetComponent<TextMesh>().text = Attack.ToString();

    }

    private void Hover(bool isUp)
    {
        if (!MouseInput.isDragging /*&& !MouseInput.isAnimating*/)
        {
            HOTween.To(gameObject.transform, 0.2f, new TweenParms()
                .Prop("position", new Vector3(gameObject.transform.position.x, (isUp ? startingPosition.y + 2 : startingPosition.y), gameObject.transform.position.z))
                .Loops(1)
                .Ease(EaseType.EaseInCirc));
            //iTween.MoveTo(gameObject, iTween.Hash("y", (isUp ? startingPosition.y + 2 : startingPosition.y), "time", 0.5f, "oncomplete", "AnimationDone"));
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
            }

            else
            {
                MouseInput.moveCard -= this.dragCard;
                MouseInput.resetCard -= this.resetCard;
            }
        }
    }

    private void dragCard(GameObject go, Ray ray, Vector3 offSet)
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
            gameObject.transform.position = new Vector3(ray.origin.x + offSet.x, ray.origin.y + offSet.y, -5);
        }
    }

    private void resetCard(GameObject go)
    {
        if (go == gameObject && !IsResetting)
        {
            Debug.Log("Resettting " + CardName);
            IsResetting = true;
            IsDragging = false;
            PlayerGO = GameObject.Find("Player");
            var player = PlayerGO.GetComponent<Player>();
            player.AdjustCardsInHand();
            var resetSequence = new Sequence(new SequenceParms().Loops(1).OnComplete(() =>
            {
                IsResetting = false;
                MouseInput.isDragging = false;
                //gameObject.transform.position = new Vector3(myTransform.position.x, myTransform.position.y,startingPosition.z);

            }));

            resetSequence.Insert(0, HOTween.To(transform, 0.25f,
                new TweenParms().Prop("localScale",
                    new Vector3(startingScale.x, startingScale.y, startingScale.z))));
            resetSequence.Insert(0, HOTween.To(transform, 0.25f, new TweenParms()
                .Prop("position", new Vector3(startingPosition.x, startingPosition.y, startingPosition.z))));

            resetSequence.Play();
        }
    }

    //private void hovercard(gameobject go, vector3 pos)
    //{

    //    if (go == gameobject && isinhand)
    //        itween.moveto(gameobject, itween.hash("y", mytransform.position.y + 2, "z", mytransform.position.z - 5, "time", 0.5f, "oncomplete", "animationdone"));
    //    else
    //        itween.moveto(gameobject, itween.hash("y", mytransform.position.y - 2, "z", mytransform.position.z - 5, "time", 0.5f, "oncomplete", "animationdone"));
    //}

    void resetZindex(float z)
    {
        MouseInput.isDragging = false;
        //gameObject.transform.localScale = new Vector3(1, 0.1f, 1);
        gameObject.transform.position = new Vector3(myTransform.position.x, myTransform.position.y, z);
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
