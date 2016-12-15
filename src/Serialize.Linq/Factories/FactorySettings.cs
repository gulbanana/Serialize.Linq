namespace Serialize.Linq.Factories
{
    public class FactorySettings
    {
        public FactorySettings()
        {
            UseRelaxedTypeNames = true;
        }

        public bool UseRelaxedTypeNames { get; set; }

        public bool AllowPrivateFieldAccess { get; set; }
    }
}
