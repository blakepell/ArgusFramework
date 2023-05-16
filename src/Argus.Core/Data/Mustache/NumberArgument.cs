namespace Argus.Data.Mustache
{
    public class NumberArgument : IArgument
    {
        private readonly decimal _value;

        public NumberArgument(decimal value)
        {
            _value = value;
        }

        public string GetKey()
        {
            return null;
        }

        public object GetValue(Scope keyScope, Scope contextScope)
        {
            return _value;
        }
    }
}
