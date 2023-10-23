using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace TCGame.Client.Event
{
    public class Card_Menu_Data_PreFab_Event : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        private GameObject Card_Data_Pics;
        public Image bgImage;
        public int code;//��Ƭ�Ŀ��ţ���ͨ����ʼ��ʱ����ֵ

        private Image imgCard;//��������ǵ���Ϸ�ϵĿ��ƣ�������Ҫ�޸Ŀ��Ƶ�ͼƬ

        private GameObject Message_Name_Text;
        private Text Name_Text;
        private GameObject Message_Des_Context;
        private Text Des_Text;
        public string cardName; //��Ƭ����
        public string cardDes; //��Ƭ����
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
            //�޸ĵ�ǰԤ�������ɫ
            bgImage.color = new Color(32f / 255f, 32f / 255f, 32f / 255f, 80f / 255f);
            //����ͼƬ
            imgCard.sprite = Resources.Load<Sprite>($"Pics/{ code }");
            //�޸��ı�
            Name_Text.text = $"{cardName}[{code}]";
            Des_Text.text = cardDes;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            bgImage.color = new Color(32f / 255f, 32f / 255f, 32f / 255f, 0f / 255f);

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            //����Ҽ�
            if (eventData.button == PointerEventData.InputButton.Right)
            { 
                //����������ӿ�Ƭ
            }
        }
    }
}