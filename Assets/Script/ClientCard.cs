using TCGame.Client.Enum;

namespace TCGame.Client
{
    public class ClientCard
    {
        public int Code { get; private set; }
        public int[] SetCode { get; private set; }//字段
        public string Name { get; private set; }
        public string Des { get; private set; }//描述
        public CardType CardType { get; private set; }  //怪兽|魔陷
        public CardDeType CardDeType { get; private set; }//通常|效果
        public int Level { get; private set; }//等级
        public CardAttribute CardAttribute { get; private set; }
        public CardRace CardRace { get; private set; }
        //public bool IsExChange { get; private set; }//怪兽作为魔陷，魔陷作为怪兽？这个写在具体游戏里面
        public Value Attack { get; private set; }
        public Value Defence { get; private set; }


        public ClientCard(int Code, int[] SetCode, string Name, string Des, CardType CardType, CardDeType CardDeType, int Level,
            CardAttribute CardAttribute, CardRace CardRace, int Attack, Value.ValueType Attack_Type, int Defence, Value.ValueType Defence_Type)
        {
            this.Code = Code;
            this.SetCode = SetCode;
            this.Name = Name;
            this.Des = Des;
            this.CardType = CardType;
            this.CardDeType = CardDeType;
            this.Level = Level;
            this.CardAttribute = CardAttribute;
            this.CardRace = CardRace;
            this.Attack = new Value(Attack, Attack_Type);
            this.Defence = new Value(Defence, Defence_Type);
        }
        public bool HasAttribute(CardAttribute cardAttribute)
        {
            return (CardAttribute & cardAttribute) != 0;
        }

        public bool HasRace(CardRace cardRace)
        {
            return (CardRace & cardRace) != 0;
        }

        public bool HasType(CardType cardType)
        {
            return (CardType & cardType) != 0;
        }
        public bool HasDeType(CardDeType cardDeType)
        {
            return (CardDeType & cardDeType) != 0;
        }

    }


}
