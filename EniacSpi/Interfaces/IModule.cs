namespace EniacSpi.Interfaces
{
    public interface IModule
    {
        string Name { get; set; }
        string Address { get; set; }
        bool IsConnected { get; }
    }
}
