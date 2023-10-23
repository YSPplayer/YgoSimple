using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TCGame.Client.Event
{
    public class Card_Menu_Data_PreFab_Event : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private GameObject Card_Data_Pics;
        public Image bgImage;
        public int code;//卡片的卡号，将通过初始化时来赋值

        private Image imgCard;//这个是我们的游戏上的卡牌，我们需要修改卡牌的图片

        private GameObject Message_Name_Text;
        private Text Name_Text;
        private GameObject Message_Des_Context;
        private Text Des_Text;
        public string cardName; //卡片名称
        public string cardDes; //卡片描述
        private void Awake()
        {
            Card_Data_Pics = GameObject.Find("Card_Data_Pics");
            Message_Name_Text = GameObject.Find("Message_Name_Text");
            Message_Des_Context = GameObject.Find("Message_Des_Context");
            imgCard = Card_Data_Pics.GetComponentInChildren<Image>();

            Name_Text = Message_Name_Text.GetComponentInChildren<Text>();
            Des_Text = Message_Des_Context.GetComponentInChildren<Text>();
            cardName = "";
            cardDes = "";
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            //修改当前预制体的颜色
            bgImage.color = new Color(32f / 255f, 32f / 255f, 32f / 255f, 80f / 255f);
            //设置图片
            imgCard.sprite = Resources.Load<Sprite>($"Pics/{ code }");
            //修改文本
            Name_Text.text = $"{cardName}[{code}]";
            Des_Text.text = cardDes;
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
                //往卡组中添加卡片
            }
        }
    }
}