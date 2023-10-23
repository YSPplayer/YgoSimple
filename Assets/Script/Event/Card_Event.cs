using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Card_Event : MonoBehaviour,IBeginDragHandler, IDragHandler
{
   public void OnBeginDrag(PointerEventData eventData)
   {
        Debug.Log("»ØºÏ");
   }
   public void OnDrag(PointerEventData eventData)
   {
        transform.position = eventData.position;
   }
}
