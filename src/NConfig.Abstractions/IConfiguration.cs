namespace NConfig
{
    public interface IConfiguration<TConfig> where TConfig : class, new()
    {
        TConfig Load();
        void Save(TConfig value);
    }
}
