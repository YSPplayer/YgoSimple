namespace TCGame.Client
{
    //因为要使用到无限大或者负无穷大，对数字进行封装
    public class Value
    {
        public int Number { get; private set; }//具体的数值
        public ValueType Value_Type { get; private set; }//判断是否为无限大

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
