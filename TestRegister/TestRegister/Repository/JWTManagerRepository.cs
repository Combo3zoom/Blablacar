using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TestRegister.Models;

namespace TestRegister.Repository;

public class JWTManagerRepository : IJWTManagerRepository
{
    Dictionary<string, string> UsersRecords = new Dictionary<string, string>
    {
        { "user1","password1"},
        { "user2","password2"},
        { "user3","password3"},
    };

    private readonly IConfiguration iconfiguration;
    public JWTManagerRepository(IConfiguration iconfiguration)
    {
        this.iconfiguration = iconfiguration;
    }

    public Token Authenticate(User users)
    {
        if (!UsersRecords.Any(x => x.Key == users.Name && x.Value == users.Password))
        {
            return null!;
        }

        // Else we generate JSON Web Token
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(iconfiguration["JWT:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, users.Name)
            }),
            Expires = DateTime.UtcNow.AddMinutes(10),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new Token {AccessToken = tokenHandler.WriteToken(token)};
    }
}