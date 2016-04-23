namespace EniacSpi.Interfaces
{
    public interface IHost
    {
        string Name { get; set; }
        string Address { get; set; }
        bool IsConnected { get; }
    }
}