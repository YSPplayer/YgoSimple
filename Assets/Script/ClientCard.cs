using TCGame.Client.Enum;

namespace TCGame.Client
{
    public class ClientCard
    {
        public int Code { get; private set; }
        public int[] SetCode { get; private set; }//�ֶ�
        public string Name { get; private set; }
        public string Des { get; private set; }//����
        public CardType CardType { get; private set; }  //����|ħ��
        public CardDeType CardDeType { get; private set; }//ͨ��|Ч��
        public int Level { get; private set; }//�ȼ�
        public CardAttribute CardAttribute { get; private set; }
        public CardRace CardRace { get; private set; }
        //public bool IsExChange { get; private set; }//������Ϊħ�ݣ�ħ����Ϊ���ޣ����д�ھ�����Ϸ����
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

    }


}
