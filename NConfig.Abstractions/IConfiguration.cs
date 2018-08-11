namespace NConfig.Abstractions
{
    // TODO: This is more of an IConfigurator or IConfigProvider or something.
    // IConfigurationStore! That's it
    public interface IConfiguration<TConfig>
    {
        TConfig Load();
        void Save(TConfig value);
    }
}
