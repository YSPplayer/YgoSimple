using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TCGame.Client.Enum;
using TCGame.Client.Event;
using System.Linq;
using System;

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
        public GameObject Star_Input;//星数文本
        public GameObject Search_Input;//搜索文本
        public Button Search_Button;//搜索按钮
        public Text Text_Second_Deck;//副卡组文本


        public VerticalLayoutGroup DeckGroup;//卡组列表
        public InputField Star_InputField;
        public InputField Search_InputField;
        public Dropdown Drop_Card_Type;
        public Dropdown Drop_Little_Type;
        public Dropdown Drop_Att;
        public Dropdown Drop_Race;

        private List<GameObject> Card_Menu_Datas = new List<GameObject>();//预制体
        public void LoadUI()
        {
            DeckGroup = DeckContent.GetComponentInChildren<VerticalLayoutGroup>();
            Drop_Card_Type = Card_Type_List.GetComponentInChildren<Dropdown>();
            Drop_Little_Type = Card_Little_Type_List.GetComponentInChildren<Dropdown>();
            Drop_Att = Att_List.GetComponentInChildren<Dropdown>();
            Drop_Race = Race_List.GetComponentInChildren<Dropdown>();
            Star_InputField = Star_Input.GetComponentInChildren<InputField>();
            Search_InputField = Search_Input.GetComponentInChildren<InputField>();

            SetListOptions(Drop_Card_Type, Config.Types.Values.ToList(),true);
            SetListOptions(Drop_Little_Type, new List<string>(), false); ;
            SetListOptions(Drop_Att, Config.Attributes.Values.ToList(), true);
            SetListOptions(Drop_Race, Config.Races.Values.ToList(), true);

            Drop_Little_Type.interactable = false;//设置不可用
            Drop_Att.interactable = false;
            Drop_Race.interactable = false;
            Star_InputField.interactable = false;
        }
        public void LoadEvent()
        {
            Search_Button.onClick.AddListener(SearchCard);
            Drop_Card_Type.onValueChanged.AddListener(SelectDropType);
        }
        private void RefreshUI()
        {
            Drop_Little_Type.value = 0;//这个表示选择索引1位置的元素
            Drop_Att.value = 0;
            Drop_Race.value = 0;
            Star_InputField.text = "";
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
                Star_InputField.interactable = false;
                RefreshUI();
            }
            else if (index == monster)
            {
                Drop_Little_Type.interactable = true;
                Drop_Att.interactable = true;
                Drop_Race.interactable = true;
                Star_InputField.interactable = true;
                RefreshUI();
                //这个地方我们设置一下显示的文字
                SetListOptions(Drop_Little_Type, new List<string>() { Config.DeTypes[CardDeType.MNormal], Config.DeTypes[CardDeType.Effect] }, false);
            }
            else if (index == spell || index == trap)
            {
                Drop_Little_Type.interactable = true;
                Drop_Att.interactable = false;
                Drop_Race.interactable = false;
                Star_InputField.interactable = false;
                RefreshUI();
                if (index == spell)  SetListOptions(Drop_Little_Type, new List<string>() { Config.DeTypes[CardDeType.SNormal] }, false);
                else  SetListOptions(Drop_Little_Type, new List<string>() { Config.DeTypes[CardDeType.SNormal] }, false);
            }
        }
        //搜索卡片
        private void SearchCard()
        {
            ClearCard();
            GetCard();
        }

        public void SetListOptions(Dropdown dropdown, List<string> options,bool isNone)
        {
            dropdown.ClearOptions();
            List<string> t_options = isNone? new List<string>() { Config.ConfigText[(int)ConfigKey.TextNone] }:
                new List<string>() { Config.ConfigText[(int)ConfigKey.TextNA] };
            t_options.AddRange(options);
            dropdown.AddOptions(t_options);
        }
        public Func<ClientCard, bool> FilterCardFunc()
        {
            object type = GetFilterParameter(Drop_Card_Type, Config.Types);
            object deType = GetFilterParameter(Drop_Little_Type, Config.DeTypes);
            object attribute = GetFilterParameter(Drop_Att, Config.Attributes);
            object race = GetFilterParameter(Drop_Race, Config.Races);
            int? level = null;
            if(!Star_InputField.text.Equals("")) level = int.Parse(Star_InputField.text);
            bool hasName = true;
            if (Search_InputField.text.Equals("")) hasName = false;
            return card => (type != null ? card.HasType((CardType)type) : true) &&
                           (deType != null ? card.HasDeType((CardDeType)deType) : true) &&
                           (attribute != null ? card.HasAttribute((CardAttribute)attribute) : true) &&
                           (race != null ? card.HasRace((CardRace)race) : true) &&
                           ((card.HasType(CardType.Monster) && level != null) ? card.Level == (int)level : true) &&
                           (hasName ? (card.Name.Contains(Search_InputField.text) || card.Des.Contains(Search_InputField.text)) : true);
        }
        public object GetFilterParameter<T>(Dropdown dropdown, Dictionary<T,string> dictionary)
        {
            //就是全部匹配
            if (dropdown.value == 0) return null;
            //获取文本
            string svaule = dropdown.options[dropdown.value].text;
            return dictionary.First(x => x.Value.Equals(svaule)).Key;
        }
        public void ClearCard()
        {
            //for (int i = DeckGroup.transform.childCount - 1; i >= 0; i--) Destroy(DeckGroup.transform.GetChild(i).gameObject);
            //不销毁而是隐藏，减少内存开辟的时间消耗
            for (int i = 0; i < Card_Menu_Datas.Count; i++) Card_Menu_Datas[i].SetActive(false);
        }
        public void GetCard()
        {
            Func<ClientCard, bool> filterFunc = FilterCardFunc();
            List<ClientCard> cards = Game.Cards.Where(card => card != null && filterFunc(card)).ToList();
            int result = cards.Count;
            for (int i = 0; i < cards.Count; i++)
            {
                ClientCard card = cards[i];
                //这个 GameObject对象我们初始化完成之后不需要把它内存释放，直接隐藏掉就可以了，如果我们下次用的时候，不需要再初始化，
                //减少内存的消耗
                GameObject Card_Menu_Data = null;
                if (i < Card_Menu_Datas.Count)
                    Card_Menu_Data = Card_Menu_Datas[i];
                else
                {
                    Card_Menu_Data = Instantiate(Card_Menu_Data_PreFab);
                    Card_Menu_Datas.Add(Card_Menu_Data);
                }
                Transform transformParent = Card_Menu_Data.transform;
                for (int j = 0; j < transformParent.childCount; j++)
                {
                    GameObject child = transformParent.GetChild(j).gameObject;
                    if (child.name.Equals("Card")) child.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>($"Pics/{ card.Code}");
                    else if (child.name.Equals("Name")) child.GetComponentInChildren<Text>().text = $"{card.Name}";
                    else if (child.name.Equals("Des"))
                    {
                        string Des = "";
                        if (card.HasType(CardType.Monster))
                        {
                            Des = $"{GetAttributesStr(card.CardAttribute)}/{GetRaceStr(card.CardRace)} ★{card.Level}";
                        }
                        else if (card.HasType(CardType.Spell) || card.HasType(CardType.Trap))
                        {
                            Des = $"{GetTypeStr(card.CardType, card.CardDeType)}";
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
                //设置我们的类脚本对象
                Card_Menu_Data_PreFab_Event event_script = Card_Menu_Data.GetComponent<Card_Menu_Data_PreFab_Event>();
                event_script.code = card.Code;
                event_script.cardName = card.Name;
                event_script.cardDes = GetCardDesStr(card);
                //设置物体可见
                Card_Menu_Data.SetActive(true);
            }
            //修改搜索结果
            Text_Second_Deck.text = $"{Config.ConfigText[(int)ConfigKey.SearchResult]}：{result}";

        }
        private string GetCardDesStr(ClientCard card)
        {
            string str = "";
            if (card.HasType(CardType.Monster))
            {
                str = $"[{GetTypeStr(card.CardType, card.CardDeType)}] {GetRaceStr(card.CardRace)}/{GetAttributesStr(card.CardAttribute)}\n[★{card.Level}] {GetPowerStr(card.Attack)}/{GetPowerStr(card.Defence)}\n{card.Des}";
            }
            else if (card.HasType(CardType.Spell))
            {
                str = $"[{GetTypeStr(card.CardType, card.CardDeType)}]\n{card.Des}";
            }
            return str;
        }
        private string GetTypeStr(CardType type, CardDeType deType)
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
