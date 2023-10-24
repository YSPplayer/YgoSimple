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
        public static List<ClientCard> Cards { get; private set; }//���ǵ����ݿ�
        public static Config Config { get; private set; }//�����ļ�
        public static DeckManage deckManage { get; private set; }//��ʼ�����ǵĶ�����

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
