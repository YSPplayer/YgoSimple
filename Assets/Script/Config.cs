using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TCGame.Client.Enum;
using Newtonsoft.Json;

namespace TCGame.Client
{
    public enum ConfigKey
    { 
        DeckMainText,
        DeckSecondText,
        DeckExtraText,
        SearchResult,
        TextNone,
        TextNA

    }
    public class Config
    {
        public string[] strAttribute { get; private set; }
        public string[] strRace { get; private set; }
        public string[] strType { get; private set; }
        public string[] strDeType { get; private set; }
        public string[] strDeckUI { get; private set; }
        //我们的对应枚举
        public static Dictionary<CardAttribute, string> Attributes;
        public static Dictionary<CardRace, string> Races;
        public static Dictionary<CardType, string> Types;
        public static Dictionary<CardDeType, string> DeTypes;
        public static List<string> ConfigText;
        public Config(string[] strAttribute, string[] strRace,string[] strType,string[] strDeType, string[] strDeckUI)
        {
            this.strAttribute = strAttribute;
            this.strRace = strRace;
            this.strType = strType;
            this.strDeType = strDeType;
            this.strDeckUI = strDeckUI;

        }
        //初始化我们对应的字典
        public void Initialize()
        {
            Attributes = new Dictionary<CardAttribute, string>();
            Races = new Dictionary<CardRace, string>();
            Types = new Dictionary<CardType, string>();
            DeTypes = new Dictionary<CardDeType, string>();
            ConfigText = new List<string>();
            for (int i = 0; i < strAttribute.Length; i++) Attributes.Add((CardAttribute)(1 << i), strAttribute[i]);
            for (int i = 0; i < strRace.Length; i++) Races.Add((CardRace)(1 << i), strRace[i]);
            for (int i = 0; i < strType.Length; i++) Types.Add((CardType)(1 << i), strType[i]);
            for (int i = 0; i < strDeType.Length; i++) DeTypes.Add((CardDeType)(1 << i), strDeType[i]);
            for (int i = 0; i < strDeckUI.Length; i++) ConfigText.Add(strDeckUI[i]);
            strAttribute = null;
            strRace = null;
            strType = null;
            strDeType = null;
            strDeckUI = null;
        }


    }
}
