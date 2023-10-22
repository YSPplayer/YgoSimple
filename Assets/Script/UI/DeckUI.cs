using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TCGame.Client.Enum;
using System.Linq;

namespace TCGame.Client.UI
{
    public class DeckUI : MonoBehaviour
    {
        public GameObject Card_Menu_Data_PreFab;//卡片预制体
        public GameObject DeckContent;//卡组列表所在的主类
        public GameObject Card_Type_List;//卡片种类筛选列表
        public GameObject Card_Little_Type_List;//卡片子种类筛选列表
        public GameObject Att_List;//卡片属性筛选列表
        public GameObject Race_List;//卡片种族筛选列表
        public Button Search_Button;//搜索按钮
        public Text Text_Second_Deck;//副卡组文本

        public VerticalLayoutGroup DeckGroup;//卡组列表
        public Dropdown Drop_Card_Type;
        public Dropdown Drop_Little_Type;
        public Dropdown Drop_Att;
        public Dropdown Drop_Race;
        public void LoadUI()
        {
            DeckGroup = DeckContent.GetComponentInChildren<VerticalLayoutGroup>();
            Drop_Card_Type = Card_Type_List.GetComponentInChildren<Dropdown>();
            Drop_Little_Type = Card_Little_Type_List.GetComponentInChildren<Dropdown>();
            Drop_Att = Att_List.GetComponentInChildren<Dropdown>();
            Drop_Race = Race_List.GetComponentInChildren<Dropdown>();
            List<string> t_options = new List<string>() { Config.ConfigText[(int)ConfigKey.TextNone] };
            List<string> t_options2 = new List<string>() { Config.ConfigText[(int)ConfigKey.TextNA] };
            List<string> options = new List<string>(t_options);
            options.AddRange(Config.Types.Values.ToList());
            SetListOptions(Drop_Card_Type, options);
            options = new List<string>(t_options2);
            options.AddRange(Config.DeTypes.Values.ToList());
            SetListOptions(Drop_Little_Type, options);
            options = new List<string>(t_options);
            options.AddRange(Config.Attributes.Values.ToList());
            SetListOptions(Drop_Att, options);
            options = new List<string>(t_options);
            options.AddRange(Config.Races.Values.ToList());
            SetListOptions(Drop_Race, options);
            Drop_Little_Type.interactable = false;//设置不可用
            Drop_Att.interactable = false;
            Drop_Race.interactable = false;
        }
        public void LoadEvent()
        {
            Search_Button.onClick.AddListener(SearchCard);
           // Drop_Card_Type.onValueChanged.AddListener();
        }
        private void SelectDropType(int index)
        {
            const int none = 0;
            const int monster = 1;
            const int spell = 2;
            const int trap = 3;
            //第一个索引，就是无
            if (index == none)
            {
                Drop_Little_Type.interactable = false;
                Drop_Att.interactable = false;
                Drop_Race.interactable = false;
            }
            else if (index == monster)
            {

            }
            else if (index == spell)
            {

            }
            else if (index == trap)
            { 

            }
        }
        //搜索卡片
        private void SearchCard()
        {
            ClearCard();
            GetCard();
        }

        public void SetListOptions(Dropdown dropdown, List<string> options)
        {
            dropdown.AddOptions(options);
        }
        public void ClearCard()
        {
            for (int i = DeckGroup.transform.childCount - 1; i >= 0; i--) Destroy(DeckGroup.transform.GetChild(i).gameObject);
        }
        public void GetCard()
        {
            List<ClientCard> cards = Game.Cards;
            int result = cards.Count;
            for (int i = 0; i < cards.Count; i++)
            {
                ClientCard card = cards[i];
                if (card == null) { --result; continue; }
                GameObject Card_Menu_Data = Instantiate(Card_Menu_Data_PreFab);
                Transform transformParent = Card_Menu_Data.transform;
                for (int j = 0; j < transformParent.childCount; j++)
                {
                    GameObject child = transformParent.GetChild(j).gameObject;
                    if (child.name.Equals("Card")) child.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>($"Pics/{ card.Code}");
                    else if (child.name.Equals("Name")) child.GetComponentInChildren<Text>().text = $"{card.Name}";
                    else if (child.name.Equals("Des"))
                    {
                        string Des = "";
                        if ((card.CardType & CardType.Monster) != 0)
                        {
                            Des = $"{GetAttributesStr(card.CardAttribute)}/{GetRaceStr(card.CardRace)} ★{card.Level}";
                        }
                        else if ((card.CardType & CardType.Spell) != 0 || (card.CardType & CardType.Trap) != 0)
                        {
                            Des = $"{GetSpellStr(card.CardType, card.CardDeType)}";
                        }
                        child.GetComponentInChildren<Text>().text = Des;
                    }
                    else if (child.name.Equals("Power"))
                    {
                        string Des = "";
                        if ((card.CardType & CardType.Monster) != 0)
                        {
                            Des = $"{GetPowerStr(card.Attack)}/{GetPowerStr(card.Defence)}";
                        }
                        child.GetComponentInChildren<Text>().text = Des;
                    } 
                }
                Card_Menu_Data.transform.SetParent(DeckContent.transform);//设置父类物体
                //设置物体可见
                Card_Menu_Data.SetActive(true);
            }
            //修改搜索结果
            Text_Second_Deck.text = $"{Config.ConfigText[(int)ConfigKey.SearchResult]}：{result}";

        }
        private string GetSpellStr(CardType type, CardDeType deType)
        {
            int iType = (int)type;
            int iDeType = (int)deType;
            string str = "";
            for (int i = 0; i < Config.Types.Count; i++)
            {
                int key = 1 << i;
                if ((iType & key) != 0)
                {
                    str += $"{Config.Types[(CardType)key]}|";
                }
            }
            for (int i = 0; i < Config.DeTypes.Count; i++)
            {
                int key = 1 << i;
                if ((iDeType & key) != 0)
                {
                    str += $"{Config.DeTypes[(CardDeType)key]}|";
                }
            }
            return str.Substring(0, str.Length - 1);
        }
        private string GetAttributesStr(CardAttribute attribute)
        {
            int iAttribute = (int)attribute;
            string str = "";
            for (int i = 0; i < Config.Attributes.Count; i++)
            {
                int key = 1 << i;
                if ((iAttribute & key) != 0)
                {
                    str += $"{Config.Attributes[(CardAttribute)key]}|";
                }
            }
            return str.Substring(0, str.Length - 1);
        }
        private string GetRaceStr(CardRace race)
        {
            int iRace = (int)race;
            string str = "";
            for (int i = 0; i < Config.Races.Count; i++)
            {
                int key = 1 << i;
                if ((iRace & key) != 0)
                {
                    str += $"{Config.Races[(CardRace)key]}|";
                }
            }
            return str.Substring(0, str.Length - 1);
        }

        private string GetPowerStr(Value value)
        {
            switch (value.Value_Type)
            {
                case Value.ValueType.Infinity:
                    return "∞";
                case Value.ValueType.NeInfinity:
                    return "-∞";
                case Value.ValueType.Unknown:
                    return "?";
                case Value.ValueType.Normal:
                    return value.Number.ToString();
                default:
                    return "";
            }
        }
    }
}
