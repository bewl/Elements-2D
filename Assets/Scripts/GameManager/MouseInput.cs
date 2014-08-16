using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class MouseInput : MonoBehaviour
{
    public Card cardData;
    public GameObject obj;
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

        if (Input.GetMouseButtonDown(0))
        {
            var tempHit = new RaycastHit();
            if (Physics.Raycast(rayCast, out tempHit, Mathf.Infinity))
            {
                var obj = tempHit.transform.gameObject;

                
            }

        }

        if (cardData == null && Input.GetMouseButtonDown(0) && !MouseInput.isDragging)
        {
            if (Physics.Raycast(rayCast, out hit, Mathf.Infinity))
            {
                
                obj = hit.transform.gameObject;

                if (obj.tag == "Card")
                {
                    MouseInput.isDragging = true;

                    cardData = obj.GetComponent<Card>();
                    cardData.IsSelected = true;

                    offSet = obj.transform.position - rayCast.origin;

                    Debug.Log(cardData.CardName + " was selected");
                }
                else
                {
                    var sprite = obj.GetComponent<SpriteRenderer>() as SpriteRenderer;

                    if (sprite != null)
                    {
                        Debug.Log("CLICKED A SPRITE");
                        sprite.enabled = !sprite.enabled;
                    }
                
                }
            }
            else if(cardData != null)
            {
                resetCard(obj);
                MouseInput.isDragging = false;
                cardData.IsSelected = false;
                cardData = null;
            }
        }
        else if (cardData != null && Input.GetMouseButtonUp(0))
        {
            resetCard(obj);
            //MouseInput.isDragging = false;
            cardData.IsSelected = false;
            cardData = null;
        }

        if (moveCard != null && offSet != null && cardData != null && cardData.IsSelected)
        {
            moveCard(obj, rayCast, offSet);
        }

    }


}

