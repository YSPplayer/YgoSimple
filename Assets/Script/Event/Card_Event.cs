using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TCGame.Client.UI;

namespace TCGame.Client.Event
{
    public class Card_Event : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerEnterHandler, IPointerClickHandler
    {
        public ClientCard card;
        public void OnPointerEnter(PointerEventData eventData)
        {
            //这里更新我们场景上的图片
            DeckUI.ChangeCardMessage($"Pics/{ card.Code }", $"{card.Name}[{card.Code}]", DeckUI.GetCardDesStr(card));
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            //从列表删除我们的卡片
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (!card.IsExtraCard())
                {
                    DeckUI.Main_Card_PreFabs.Remove(this.gameObject);

                }
                else
                {
                    DeckUI.Extra_Card_PreFabs.Remove(this.gameObject);
                }
                Destroy(this.gameObject);
            }
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            //Debug.Log("回合");
        }
        public void OnDrag(PointerEventData eventData)
        {
            transform.position = eventData.position;
        }
    }
}