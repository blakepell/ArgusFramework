namespace Argus.Data.Mustache
{
    public class StringArgument : IArgument
    {
        private readonly string _value;

        public StringArgument(string value)
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
