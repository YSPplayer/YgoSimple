using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using TCGame.Client.Deck;

namespace TCGame.Client
{
    public class Game : MonoBehaviour
    {
        public static List<ClientCard> Cards { get; private set; }//我们的数据库
        public static Config Config { get; private set; }//配置文件
        public static DeckManage deckManage { get; private set; }//初始化我们的对象类

        private void Awake()
        {
            LoadCardsDataFromJson();
            Initialize();
        }
        private void Initialize()
        {
            Config.Initialize();
            deckManage = new DeckManage();
        }
        private void LoadCardsDataFromJson()
        {
            string filePath = $"{Application.dataPath}/Resources/Data/CardData.json";
            Cards = JsonConvert.DeserializeObject<List<ClientCard>>(File.ReadAllText(filePath, Encoding.UTF8));
            filePath = $"{Application.dataPath}/Resources/Data/Config.json";
            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(filePath, Encoding.UTF8));
        }
    }
}
