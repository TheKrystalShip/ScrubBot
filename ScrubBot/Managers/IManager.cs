namespace ScrubBot.Managers
{
    public interface IManager<T> where T : class
    {
        T Get(ulong id);
    }
}
