namespace streamer.Features.interfaces.services
{
    public interface ITokenFactory
    {
        string GenerateToken(int size = 32);
    }
}
