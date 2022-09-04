using TestRegister.Models;

namespace TestRegister.Repository;

public interface IJWTManagerRepository
{
    Token Authenticate(User user);
}