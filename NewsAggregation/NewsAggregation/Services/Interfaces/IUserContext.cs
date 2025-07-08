namespace NewsAggregation.Services.Interfaces
{
    public interface IUserContext
    {
        int? UserId { get; }
        void SetUserFromToken(string jwtToken);
    }
}