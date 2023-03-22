namespace who_am_i_be.Interfaces;

public interface ITokenEmitterService
{
    string GenerateAuthToken(string userId);
}