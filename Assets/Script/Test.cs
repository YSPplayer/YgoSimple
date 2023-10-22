using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TCGame.Client;
using System.IO;
using Newtonsoft.Json;

public class Test : MonoBehaviour
{
    public List<ClientCard> Cards { get; private set; }

    //RectTransform
    private void Awake()
    {
        LoadCardsDataFromJson();
    }
    private void LoadCardsDataFromJson()
    {
        string filePath = $"{Application.dataPath}/Resources/Data/CardData.json";
        Cards = JsonConvert.DeserializeObject<List<ClientCard>>(File.ReadAllText(filePath, Encoding.Default));
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
