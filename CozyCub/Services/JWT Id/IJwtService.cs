using Razorpay.Api;

namespace CozyCub.JWT_Id
{
    public interface IJwtService
    {
        int GetUserIdFromToken(string token);
    }
}
