using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using TCGame.Client.Enum;

namespace TCGame.Client
{
    public class Game : MonoBehaviour
    {
        public static List<ClientCard> Cards { get; private set; }//我们的数据库
        public static Config Config { get; private set; }//配置文件

        private void Awake()
        {
            LoadData();
            Initialize();
        }
        private void Initialize()
        {

            LoadConfig();
        }
        private void LoadConfig()
        {
            string filePath = $"{Application.dataPath}/Resources/Data/Config.json";
            Config = JsonConvert.DeserializeObject<Config>(File.ReadAllText(filePath, Encoding.UTF8));
            Config.Initialize();
        }
        private void LoadData()
        {
            LoadCardsDataFromJson();
            LoadCardsDataFromCdb();
        }
        private void LoadCardsDataFromCdb()
        {
            //从cdb读取我们的数据
            string dbPath = $"{Application.dataPath}/Resources/Data/cards.cdb";
            if (File.Exists(dbPath))
            {
                using (SQLiteConnection sqliteconn = new SQLiteConnection(@"Data Source=" + dbPath + ";version = 3; Character Set = utf8"))
                {
                    sqliteconn.Open();
                    using (SQLiteTransaction trans = sqliteconn.BeginTransaction())
                    {
                        using (SQLiteCommand sqlitecommand = new SQLiteCommand(sqliteconn))
                        {
                            //获取我们卡号匹配的名称
                            string SQLstr = $"SELECT datas.*,texts.* FROM datas,texts WHERE datas.id=texts.id";
                            sqlitecommand.CommandText = SQLstr;
                            using (SQLiteDataReader reader = sqlitecommand.ExecuteReader())
                            {

                                while (reader.Read())
                                {
                                    int code = reader.GetInt32(reader.GetOrdinal("id"));
                                    if (Cards.Any(card => card != null && card.IsCode(code))) continue;
                                    long type = reader.GetInt64(reader.GetOrdinal("type"));
                                    ClientCard card = new ClientCard
                                    (
                                        code,
                                        null,
                                        reader.GetString(reader.GetOrdinal("name")),
                                        reader.GetString(reader.GetOrdinal("desc")),
                                        GetCdbCardType(type),
                                        GetCdbCardDeType(type),
                                        reader.GetInt32(reader.GetOrdinal("level")),
                                        (CardAttribute)reader.GetInt32(reader.GetOrdinal("attribute")),
                                        (CardRace)reader.GetInt64(reader.GetOrdinal("race")),
                                        reader.GetInt32(reader.GetOrdinal("atk")),
                                        Value.ValueType.Normal,
                                        reader.GetInt32(reader.GetOrdinal("def")),
                                        Value.ValueType.Normal
                                    );
                                    Cards.Add(card);
                                }
                                reader.Close();
                            }
                        }
                        trans.Commit();
                    }
                    sqliteconn.Close();
                }
            }
            else
                Log.WriteLog("cdb数据库不存在！");
        }
        private CardType GetCdbCardType(long type)
        {
            int Monster = 0x1;
            int Spell = 0x2;
            int Trap = 0x4;
            int result = 0;
            if ((type & Monster) != 0) result |= Monster;
            if ((type & Spell) != 0) result |= Spell;
            if ((type & Trap) != 0) result |= Trap;
            return (CardType)result;
        }
        private CardDeType GetCdbCardDeType(long type)
        {
            int Monster = 0x1;
            int Normal = 0x10;
            int Effect = 0x20;
            int Fusion = 0x40;
            int result = 0;
            if ((type & Normal) != 0) result |= (type & Monster) != 0 ? (int)CardDeType.MNormal : (int)CardDeType.SNormal;
            if ((type & Effect) != 0) result |= (int)CardDeType.Effect;
            if ((type & Fusion) != 0) result |= (int)CardDeType.Fusion;
            return (CardDeType)result;
        }
        private void LoadCardsDataFromJson()
        {
            string filePath = $"{Application.dataPath}/Resources/Data/CardData.json";
            if (File.Exists(filePath))  
                Cards = JsonConvert.DeserializeObject<List<ClientCard>>(File.ReadAllText(filePath, Encoding.UTF8));
            else 
                Log.WriteLog("json数据库不存在！");
        }
    }
}
