using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class MouseInput : MonoBehaviour
{
    public Card cardData;
    public GameObject card;
    Ray rayCast;
    RaycastHit hit;
    Vector3 offSet;
    public delegate void PositionCard(GameObject go);
    public delegate void DragCard(GameObject go, Ray ray, Vector3 offSet);
    public static event PositionCard hoverCard;
    public static event PositionCard resetCard;
    public static event DragCard moveCard;
    public static bool isDragging = false;
    public static bool isAnimating = false;

    void Update()
    {
        //if(!MouseInput.isAnimating)
            DetectMouse();
    }

    void DetectMouse()
    {
        rayCast = Camera.main.ScreenPointToRay(Input.mousePosition);
        var hit = new RaycastHit();
        
        if (cardData == null && Input.GetMouseButtonDown(0) && !MouseInput.isDragging)
        {
            if (Physics.Raycast(rayCast, out hit, Mathf.Infinity))
            {
                MouseInput.isDragging = true;
                card = hit.transform.gameObject;
            
                cardData = card.GetComponent<Card>();
                cardData.IsSelected = true;

                offSet = card.transform.position - rayCast.origin;

                Debug.Log(cardData.CardName + " was selected");
            }
            else if(cardData != null)
            {
                resetCard(card);
                MouseInput.isDragging = false;
                cardData.IsSelected = false;
                cardData = null;
            }
        }
        else if (cardData != null && Input.GetMouseButtonUp(0))
        {
            resetCard(card);
            //MouseInput.isDragging = false;
            cardData.IsSelected = false;
            cardData = null;
        }

        if (moveCard != null && offSet != null && cardData != null && cardData.IsSelected)
        {
            moveCard(card, rayCast, offSet);
        }

    }


}

