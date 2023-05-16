namespace Argus.Data.Mustache
{
    public class PlaceholderArgument : IArgument
    {
        private readonly string _name;

        public PlaceholderArgument(string name)
        {
            _name = name;
        }

        public string GetKey()
        {
            return _name;
        }

        public object GetValue(Scope keyScope, Scope contextScope)
        {
            return keyScope.Find(_name, false);
        }
    }
}
