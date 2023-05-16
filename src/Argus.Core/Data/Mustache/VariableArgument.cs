namespace Argus.Data.Mustache
{
    public class VariableArgument : IArgument
    {
        private readonly string _name;

        public VariableArgument(string name)
        {
            _name = name;
        }

        public string GetKey()
        {
            return null;
        }

        public object GetValue(Scope keyScope, Scope contextScope)
        {
            return contextScope.Find(_name, false);
        }
    }
}
