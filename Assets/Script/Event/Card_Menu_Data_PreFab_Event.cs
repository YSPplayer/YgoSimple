using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TCGame.Client.UI;
using System.Linq;
using System.Collections.Generic;

namespace TCGame.Client.Event
{
    public class Card_Menu_Data_PreFab_Event : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {

        public GameObject Card_PreFab;//卡片预制体
        public Image bgImage;//这个是我们的游戏上的卡牌，我们需要修改卡牌的图片
        public ClientCard card;
        private bool isMainChange = false;
        private bool isExtraChange = false;

        public void OnPointerEnter(PointerEventData eventData)
        {
            //修改当前预制体的颜色
            bgImage.color = new Color(32f / 255f, 32f / 255f, 32f / 255f, 80f / 255f);
            //修改图片 + 文本
            DeckUI.ChangeCardMessage($"Pics/{ card.Code }", $"{card.Name}[{card.Code}]", DeckUI.GetCardDesStr(card));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            bgImage.color = new Color(32f / 255f, 32f / 255f, 32f / 255f, 0f / 255f);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //鼠标右键
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                if (!CheckRule()) return;
                //往卡组中添加卡片
                GameObject Card_Data = Instantiate(Card_PreFab);
                Image image = Card_Data.GetComponentInChildren<Image>();
                image.sprite = Resources.Load<Sprite>($"Pics/{ card.Code}");
                Card_Event event_script = Card_Data.GetComponent<Card_Event>();
                event_script.card = card;
                Card_Data.SetActive(true);
                if (!card.IsExtraCard())
                {
                    Card_Data.transform.SetParent(DeckUI.mainDeckGroup.transform);//设置到父类物体
                    CheckGroupSize(0,image);
                    DeckUI.Main_Card_PreFabs.Add(Card_Data);
                    DeckUI.text_Main_Deck.text = $"{ Config.ConfigText[(int)ConfigKey.DeckMainText]}：{DeckUI.Main_Card_PreFabs.Count}";
                }
                else
                {
                    Card_Data.transform.SetParent(DeckUI.extraDeckGroup.transform);
                    CheckGroupSize(1, image);
                    DeckUI.Extra_Card_PreFabs.Add(Card_Data);
                    DeckUI.text_Extra_Deck.text = $"{ Config.ConfigText[(int)ConfigKey.DeckExtraText]}：{DeckUI.Extra_Card_PreFabs.Count}";
                }

            }
        }
        private bool CheckRule()
        {
            if (!card.IsExtraCard())
            {
                if (DeckUI.Main_Card_PreFabs.Count >= GameRule.MaxMainCount) return false;
                int count = DeckUI.Main_Card_PreFabs.Count(prefab =>
                {
                    Card_Event event_script = prefab.GetComponent<Card_Event>();
                    return event_script.card.Code == card.Code;
                });
                if (count >= GameRule.MaxRepeatCardCount) return false;
            }
            else
            {
                if (DeckUI.Extra_Card_PreFabs.Count >= GameRule.MaxExtraCount) return false;
                int count = DeckUI.Extra_Card_PreFabs.Count(prefab =>
                {
                    Card_Event event_script = prefab.GetComponent<Card_Event>();
                    return event_script.card.Code == card.Code;
                });
                if (count >= GameRule.MaxRepeatCardCount) return false;
            }
            return true;
        }
        private void CheckGroupSize(int type ,Image image)
        {
            if (type == 0 && DeckUI.Main_Card_PreFabs.Count > 0)
            {
                GameObject card_preFab = DeckUI.Main_Card_PreFabs[DeckUI.Main_Card_PreFabs.Count - 1];
                float x = card_preFab.GetComponent<RectTransform>().anchoredPosition.x;
                float y = card_preFab.GetComponent<RectTransform>().anchoredPosition.y;
                if (x > 870.0f && Math.Abs(y) > 300.0f)
                {
                    //设置透明度
                    image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);
                    isMainChange = true;
                }
            }
            else if(type == 1 && DeckUI.Extra_Card_PreFabs.Count > 0)
            {
                GameObject card_preFab = DeckUI.Extra_Card_PreFabs[DeckUI.Extra_Card_PreFabs.Count - 1];
                float x = card_preFab.GetComponent<RectTransform>().anchoredPosition.x;
                if (x > 870.0f)
                {
                    image.color = new Color(image.color.r, image.color.g, image.color.b, 0.0f);
                    isExtraChange = true;
                }
            }
        }
        private void UpdateGroupSize(int type)
        {
            RectTransform rectTransform = null;
            GridLayoutGroup group = null;
            GameObject Card_PreFab = null;
            if (type == 0)
            {
                //获取当前控件的高度
                rectTransform = DeckUI.main_deck_container.GetComponent<RectTransform>();
                group = DeckUI.mainDeckGroup;
                Card_PreFab = DeckUI.Main_Card_PreFabs[DeckUI.Main_Card_PreFabs.Count - 1];
            }
            else if (type == 1)
            {
                //修改的是额外卡组
                rectTransform = DeckUI.extra_deck_container.GetComponent<RectTransform>();
                group = DeckUI.extraDeckGroup;
                Card_PreFab = DeckUI.Extra_Card_PreFabs[DeckUI.Extra_Card_PreFabs.Count - 1];
            }
            float height = rectTransform.rect.height;
            float y = Card_PreFab.GetComponent<RectTransform>().anchoredPosition.y;
            if (Math.Abs(y) >= height)
                group.spacing = new Vector2(group.spacing.x - 1f, group.spacing.y);
            else
            {
                Image image = Card_PreFab.GetComponentInChildren<Image>();
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1.0f);
                if (type == 0) isMainChange = false;
                else if (type == 1) isExtraChange = false;
            }
        }
        private void Update()
        {
            if (!isMainChange && !isExtraChange) return;
            if (isMainChange) UpdateGroupSize(0);
            if (isExtraChange) UpdateGroupSize(1);
        }

    }
}