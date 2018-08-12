namespace NConfig.Abstractions
{
    public interface IConfiguration<TConfig>
    {
        TConfig Load();
        void Save(TConfig value);
    }
}
