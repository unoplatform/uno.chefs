namespace Chefs.Data;
public class UserEndpoint : IUserEndpoint
{
    public ValueTask<UserData> GetUserInformation(CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
