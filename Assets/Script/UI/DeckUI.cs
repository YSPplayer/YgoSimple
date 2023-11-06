using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TCGame.Client.Enum;
using TCGame.Client.Event;
using TCGame.Client.Deck;
using TCGame.Client.Core;
using TCGame.Client.CSocket;
using System.Linq;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;
using UnityEngine.SceneManagement;

namespace TCGame.Client.UI
{
    public class DeckUI : MonoBehaviour
    {
        public GameObject Card_PreFab;//��ƬԤ����
        public GameObject Card_Menu_Data_PreFab;//��Ƭ�б�Ԥ����
        public GameObject DeckContent;//�����б����ڵ�����
        public GameObject Card_Type_List;//��Ƭ����ɸѡ�б�
        public GameObject Card_Little_Type_List;//��Ƭ������ɸѡ�б�
        public GameObject Att_List;//��Ƭ����ɸѡ�б�
        public GameObject Race_List;//��Ƭ����ɸѡ�б�
        public GameObject Star_Input;//�����ı�
        public GameObject Search_Input;//�����ı�
        public Button Search_Button;//������ť
        public Button Save_Button;//���水ť
        public Button Save_As_Button;//��水ť
        public Button Delete_Button;//ɾ����ť
        public Button Exit_Button;//�˳���ť
        public Text Text_Search;//�������ı�
        public GameObject Save_As_Input;//���
        public GameObject Deck_List;//����

        public VerticalLayoutGroup DeckGroup;//�����б�
        public InputField Star_InputField;
        public InputField Search_InputField;
        public InputField Save_As_InputField;
        public Dropdown Drop_Card_Type;
        public Dropdown Drop_Little_Type;
        public Dropdown Drop_Att;
        public Dropdown Drop_Race;
        public Dropdown Drop_Deck_List;

        private List<GameObject> Card_Menu_Datas = new List<GameObject>();//Ԥ����

        //��Ƭ���ڲ���UI����
        public static List<GameObject> Main_Card_PreFabs = new List<GameObject>();
        public static List<GameObject> Extra_Card_PreFabs = new List<GameObject>();
        public static List<GameObject> Second_Card_PreFabs = new List<GameObject>();

        //���Ǳ༭������ߵ�Ԥ����
        public static Image cardImage;
        public static Text name_Text;
        public static Text des_Text;
        public static Text text_Main_Deck;
        public static Text text_Extra_Deck;
        public static Text text_Second_Deck;
        public static GameObject main_deck_container;
        public static GameObject extra_deck_container;
        public static GameObject second_deck_container;
        public static GridLayoutGroup mainDeckGroup;
        public static GridLayoutGroup extraDeckGroup;
        public static GridLayoutGroup secondDeckGroup;

        //���ǵ�ǰ�Ŀ���
        private GameDeck currentDeck;

        public void LoadUI()
        {
            currentDeck = null;
            cardImage = GameObject.Find("Card_Data_Pics").GetComponentInChildren<Image>();
            name_Text = GameObject.Find("Message_Name_Text").GetComponentInChildren<Text>();
            des_Text = GameObject.Find("Message_Des_Context").GetComponentInChildren<Text>();
            main_deck_container = GameObject.Find("Main_Deck");
            extra_deck_container = GameObject.Find("Extra_Deck");
            second_deck_container = GameObject.Find("Second_Deck");
            mainDeckGroup = main_deck_container.GetComponentInChildren<GridLayoutGroup>();
            extraDeckGroup = extra_deck_container.GetComponentInChildren<GridLayoutGroup>();
            secondDeckGroup = second_deck_container.GetComponentInChildren<GridLayoutGroup>();
            text_Main_Deck = GameObject.Find("Text_Main_Deck").GetComponentInChildren<Text>();
            text_Extra_Deck = GameObject.Find("Text_Extra_Deck").GetComponentInChildren<Text>();
            text_Second_Deck = GameObject.Find("Text_Second_Deck").GetComponentInChildren<Text>();

            DeckGroup = DeckContent.GetComponentInChildren<VerticalLayoutGroup>();
            Drop_Card_Type = Card_Type_List.GetComponentInChildren<Dropdown>();
            Drop_Little_Type = Card_Little_Type_List.GetComponentInChildren<Dropdown>();
            Drop_Deck_List = Deck_List.GetComponentInChildren<Dropdown>();
            Drop_Att = Att_List.GetComponentInChildren<Dropdown>();
            Drop_Race = Race_List.GetComponentInChildren<Dropdown>();
            Star_InputField = Star_Input.GetComponentInChildren<InputField>();
            Search_InputField = Search_Input.GetComponentInChildren<InputField>();
            Save_As_InputField = Save_As_Input.GetComponentInChildren<InputField>();

            SetListOptions(Drop_Card_Type, Config.Types.Values.ToList(), true);
            SetListOptions(Drop_Little_Type, new List<string>(), false);
            SetListOptions(Drop_Att, Config.Attributes.Values.ToList(), true);
            SetListOptions(Drop_Race, Config.Races.Values.ToList(), true);

            Drop_Little_Type.interactable = false;//���ò�����
            Drop_Att.interactable = false;
            Drop_Race.interactable = false;
            Star_InputField.interactable = false;

            //��ʼ��UI������ı�
            initializeUIStr();

            //��ʼ�����ǵĿ����Լ����
            LoadDeck(Drop_Deck_List.value);
        }
        private void DestoryListCard(List<GameObject> cardGameObjects)
        {
            foreach (var cardGameObject in cardGameObjects)
                Destroy(cardGameObject);
        }
        private void ClearDeck()
        {
            DestoryListCard(Main_Card_PreFabs);
            DestoryListCard(Extra_Card_PreFabs);
            DestoryListCard(Second_Card_PreFabs);
            //��������ǵĿ���
            Main_Card_PreFabs.Clear();
            Extra_Card_PreFabs.Clear();
            Second_Card_PreFabs.Clear();

            text_Main_Deck.text = $"{ Config.ConfigText[(int)ConfigKey.DeckMainText]}��0";
            text_Extra_Deck.text = $"{ Config.ConfigText[(int)ConfigKey.DeckExtraText]}��0";
            currentDeck = null;
        }
        private void LoadDeck(int index)
        {
            if (Drop_Deck_List.options.Count <= 0) return;
            ClearDeck();
            //�������ǵĿ��鵽���ǵ�UI
            string text = Drop_Deck_List.options[index].text;
            if (text.Equals("")) return;
            string filePath = $"{Application.dataPath}/Resources/Deck/{text}.json";
            if (!File.Exists(filePath)) return;
            GameDeck deck = null;
            try
            {
                //��ֹ�����޸����ǵ�jsond��ɶ�ȡ���˵Ĵ���
                deck = JsonConvert.DeserializeObject<GameDeck>(File.ReadAllText(filePath, Encoding.UTF8));
            }
            catch (Exception e) { Log.WriteLog(e.Message); }
            if (deck == null) return;
            currentDeck = deck;
            //��Ԥ������spcing�ĳߴ�
            mainDeckGroup.spacing = new Vector2(GetSpacingX(deck.MainSpacing), GetSpacingY(deck.MainSpacing));
            extraDeckGroup.spacing = new Vector2(GetSpacingX(deck.ExtraSpacing), GetSpacingY(deck.ExtraSpacing));
            secondDeckGroup.spacing = new Vector2(GetSpacingX(deck.SecondSpacing), GetSpacingY(deck.SecondSpacing));
            //�������ǵ�UI���󲢼��ص���Ӧ������
            CreateCardGameObjectFromCode(deck.MianCodes);
            CreateCardGameObjectFromCode(deck.ExtraCodes);
            CreateCardGameObjectFromCode(deck.SecondCodes);
            text_Main_Deck.text = $"{ Config.ConfigText[(int)ConfigKey.DeckMainText]}��{Main_Card_PreFabs.Count}";
            text_Extra_Deck.text = $"{ Config.ConfigText[(int)ConfigKey.DeckExtraText]}��{Extra_Card_PreFabs.Count}";
        }
        private float GetSpacingX(float[] spacing)
        {
           return spacing == null ? -20.0f : spacing[0];
        }
        private float GetSpacingY(float[] spacing)
        {
            return spacing == null ? 0.0f : spacing[1];
        }
        private void CreateCardGameObjectFromCode(List<int> codes)
        {
            if (codes == null) return;
            foreach (var code in codes)
            {
                if (!Game.CardsMap.ContainsKey(code)) continue;
                CreateCardGameObject(Game.CardsMap[code]);
            }
        }
        public void CreateCardGameObject(ClientCard card)
        {
            GameObject Card_Data = Instantiate(Card_PreFab);
            Image image = Card_Data.GetComponentInChildren<Image>();
            image.sprite = Resources.Load<Sprite>($"Pics/{ card.Code }");
            Card_Event event_script = Card_Data.GetComponent<Card_Event>();
            event_script.card = card;
            Card_Data.SetActive(true);
            if (!card.IsExtraCard())
            {
                Card_Data.transform.SetParent(mainDeckGroup.transform);
                Main_Card_PreFabs.Add(Card_Data);
            }
            else
            {
                Card_Data.transform.SetParent(extraDeckGroup.transform);
                Extra_Card_PreFabs.Add(Card_Data);
            }

        }

        public void initializeUIStr()
        {
            Text_Search.text = $"{Config.ConfigText[(int)ConfigKey.SearchResult]}��0";
            text_Main_Deck.text = $"{ Config.ConfigText[(int)ConfigKey.DeckMainText]}��0";
            text_Extra_Deck.text = $"{ Config.ConfigText[(int)ConfigKey.DeckExtraText]}��0";

            Drop_Deck_List.options.Clear();
            //�����ʼ�����ǵ�decklist
            try
            {
                string[] deckFiles = Directory.GetFiles($"{Application.dataPath}/Resources/Deck/", "*.json");
                List<string> deckFilesList = new List<string>();
                foreach (string filePath in deckFiles)
                {
                    string fileName = Path.GetFileNameWithoutExtension(filePath);
                    deckFilesList.Add(fileName);
                }
                //Ԥ�ȼ������ǵ�json�ı�����
                Drop_Deck_List.AddOptions(deckFilesList);
            }
            catch (Exception e) { }

        }
        public void LoadEvent()
        {
            Search_Button.onClick.AddListener(SearchCard);
            Save_As_Button.onClick.AddListener(SaveDeckAs);
            Delete_Button.onClick.AddListener(RemoveDeck);
            Save_Button.onClick.AddListener(SaveDeck);
            Exit_Button.onClick.AddListener(ExitDeckEdit);
            Drop_Card_Type.onValueChanged.AddListener(SelectDropType);
            Drop_Deck_List.onValueChanged.AddListener(DeckListChange);
           
        }
        private void ExitDeckEdit()
        {
            //�˳�����༭����
            //������һ������
            SceneManager.LoadScene("GameDuel");
            GameDueUI.playerDeck = currentDeck;
            if (!SocketManage.StartServer()) SocketManage.CloseServer();
            if (!SocketManage.InitClientSocket()) return;

            


        }
        private void DeckListChange(int index)
        {
            LoadDeck(index);
        }
        private void RemoveDeck()
        {
            string text = Drop_Deck_List.options[Drop_Deck_List.value].text;
            if (text.Equals("")) return;
            //��������ǵ�ǰ�µ�UI����
            ClearDeck();
            //Ȼ��������ǵ�options�б�
            int index = Drop_Deck_List.value;
            Drop_Deck_List.options.RemoveAt(index);
            if (index >= 0)
            {

                Drop_Deck_List.value = 0;
                //Ҫˢ��һ�£���ˢ�²���ı�
                Drop_Deck_List.RefreshShownValue();
            }
            //Ȼ��ɾ����Ӧ���ļ�
            string filePath = $"{Application.dataPath}/Resources/Deck/{text}.json";
            if (!File.Exists(filePath)) return;
            File.Delete(filePath);
            Debug.Log("�ļ�ɾ���ɹ���");
        }
        private GameDeck GetDeck()
        {
            if (currentDeck != null) return currentDeck;
            List<int> mainCodes = new List<int>();
            List<int> extraCodes = new List<int>();
            List<int> secondCodes = new List<int>();
            foreach (var prefab in Main_Card_PreFabs)
            {
                ClientCard card = prefab.GetComponent<Card_Event>().card;
                mainCodes.Add(card.GetCode());
            }
            foreach (var prefab in Extra_Card_PreFabs)
            {
                ClientCard card = prefab.GetComponent<Card_Event>().card;
                extraCodes.Add(card.GetCode());
            }
            foreach (var prefab in Second_Card_PreFabs)
            {
                ClientCard card = prefab.GetComponent<Card_Event>().card;
                secondCodes.Add(card.GetCode());
            }
            return currentDeck = new GameDeck(mainCodes.Count > 0 ? mainCodes : null, extraCodes.Count > 0 ? extraCodes : null, secondCodes.Count > 0 ? secondCodes : null, new float[] { mainDeckGroup.spacing.x, mainDeckGroup.spacing.y },
                new float[] { extraDeckGroup.spacing.x, extraDeckGroup.spacing.y }, new float[] { secondDeckGroup.spacing.x, secondDeckGroup.spacing.y });
        }
        private void SaveDeck()
        {
            string text = Drop_Deck_List.options[Drop_Deck_List.value].text;
            if (text.Equals("")) return;
            //�������ǵ�ǰ�Ŀ������ݵ�json
            string json = JsonConvert.SerializeObject(GetDeck());
            string path = $"{Application.dataPath}/Resources/Deck/{text}.json";
            try
            {
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                return;
            }
            Debug.Log("����ɹ���");
        }
        private void SaveDeckAs()
        {
            string text = Save_As_InputField.text;
            if (text.Equals(""))
            {
                Debug.Log("���������֣�");
                return;
            }
            GameDeck deck = GetDeck();
            string json = JsonConvert.SerializeObject(deck);
            string path = $"{Application.dataPath}/Resources/Deck/{text}.json";
            try
            {
                File.WriteAllText(path, json);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex);
                return;
            }
            Debug.Log("д��ɹ���");
            //�������Ǹ���һ��������б�
            Drop_Deck_List.AddOptions(new List<string> { text });
            //ͬʱҪ�����б�ǰ��ֵΪ�������ֵ
            Drop_Deck_List.value = Drop_Deck_List.options.Count - 1;
            //�����ǵ�text�ÿ�
            Save_As_InputField.text = "";
        }
        private void RefreshUI()
        {
            Drop_Little_Type.value = 0;//�����ʾѡ������1λ�õ�Ԫ��
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
            //��һ��������������
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
                //����ط���������һ����ʾ������
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
                else  SetListOptions(Drop_Little_Type, new List<string>() { Config.DeTypes[CardDeType.MNormal],Config.DeTypes[CardDeType.Fusion] }, false);
            }
        }
        //������Ƭ
        private void SearchCard()
        {
            ClearCard();
            GetCard();
        }
        public static void ChangeCardMessage(string path,string name,string text)
        {
            //����ͼƬ
            cardImage.sprite = Resources.Load<Sprite>(path);
            //�޸��ı�
            name_Text.text = name;
            des_Text.text = text;
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
            //����ȫ��ƥ��
            if (dropdown.value == 0) return null;
            //��ȡ�ı�
            string svaule = dropdown.options[dropdown.value].text;
            return dictionary.First(x => x.Value.Equals(svaule)).Key;
        }
        public void ClearCard()
        {
            //for (int i = DeckGroup.transform.childCount - 1; i >= 0; i--) Destroy(DeckGroup.transform.GetChild(i).gameObject);
            //�����ٶ������أ������ڴ濪�ٵ�ʱ������
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
                //��� GameObject�������ǳ�ʼ�����֮����Ҫ�����ڴ��ͷţ�ֱ�����ص��Ϳ����ˣ���������´��õ�ʱ�򣬲���Ҫ�ٳ�ʼ����
                //�����ڴ������
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
                            Des = $"{GetAttributesStr(card.CardAttribute)}/{GetRaceStr(card.CardRace)} ��{card.Level}";
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
                Card_Menu_Data.transform.SetParent(DeckContent.transform);//���ø�������
                Card_Menu_Data_PreFab_Event event_script = Card_Menu_Data.GetComponent<Card_Menu_Data_PreFab_Event>();
                event_script.card = card;
                //��������ɼ�
                Card_Menu_Data.SetActive(true);
            }
            //�޸��������
            Text_Search.text = $"{Config.ConfigText[(int)ConfigKey.SearchResult]}��{result}";

        }
        public static string GetCardDesStr(ClientCard card)
        {
            string str = "";
            if (card.HasType(CardType.Monster))
            {
                str = $"[{GetTypeStr(card.CardType, card.CardDeType)}] {GetRaceStr(card.CardRace)}/{GetAttributesStr(card.CardAttribute)}\n[��{card.Level}] {GetPowerStr(card.Attack)}/{GetPowerStr(card.Defence)}\n{card.Des}";
            }
            else if (card.HasType(CardType.Spell))
            {
                str = $"[{GetTypeStr(card.CardType, card.CardDeType)}]\n{card.Des}";
            }
            return str;
        }
        private static string GetTypeStr(CardType type, CardDeType deType)
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
        private static string GetAttributesStr(CardAttribute attribute)
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
        private static string GetRaceStr(CardRace race)
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

        private static string GetPowerStr(Value value)
        {
            switch (value.Value_Type)
            {
                case Value.ValueType.Infinity:
                    return "��";
                case Value.ValueType.NeInfinity:
                    return "-��";
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
