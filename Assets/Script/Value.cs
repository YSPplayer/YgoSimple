namespace TCGame.Client
{
    //��ΪҪʹ�õ����޴���߸�����󣬶����ֽ��з�װ
    public class Value
    {
        public int Number { get; private set; }//�������ֵ
        public ValueType Value_Type { get; private set; }//�ж��Ƿ�Ϊ���޴�

        public enum ValueType
        {
            Infinity = 1,
            NeInfinity = 2,
            Unknown = 3,
            Normal = 4
        }
        public Value(int Number)
        {
            this.Number = Number;
            this.Value_Type = ValueType.Normal;
        }

        public Value(int Number, ValueType Value_Type)
        {
            this.Number = Number;
            this.Value_Type = Value_Type;
        }
    }
    
}
