using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.Security.Encryption;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Core.Utilities.Security.Jwt;

public class JwtHelper : ITokenHelper
{
    public IConfiguration Configuration;
    private TokenOption _tokenOption;
    private DateTime _accessTokenExpiration;

    public JwtHelper(IConfiguration configuration)
    {
        Configuration = configuration;
        _tokenOption = Configuration.GetSection("TokenOptions").Get<TokenOption>();
    }

    public AccessToken CreateToken(User user, List<OperationClaim> operationClaims)
    {
        _accessTokenExpiration = DateTime.Now.AddMinutes(_tokenOption.AccessTokenExpiration);
        var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOption.SecurityKey);
        var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
        var jwt = CreateJwtSecurityToken(_tokenOption, user, operationClaims, signingCredentials);
        var jwtSecurtiyTokenHandler = new JwtSecurityTokenHandler();
        var token = jwtSecurtiyTokenHandler.WriteToken(jwt);

        return new AccessToken()
        {
            Token = token,
            Expiration = _accessTokenExpiration
        };
    }

    private JwtSecurityToken CreateJwtSecurityToken(TokenOption tokenOption, User user, List<OperationClaim> operationClaims, SigningCredentials signingCredentials)
    {
        var jwt = new JwtSecurityToken(
            issuer: _tokenOption.Issuer,
            audience: _tokenOption.Audience,
            notBefore: DateTime.Now,
            signingCredentials: signingCredentials,
            claims: SetClaims(user, operationClaims)
        );
        return jwt;
    }

    private IEnumerable<Claim> SetClaims(User user, List<OperationClaim> operationClaims)
    {
        var claims = new List<Claim>();
        claims.AddEmail(user.Email);
        claims.AddName($"{user.FirstName} {user.LastName}");
        claims.AddNameIdentifier(user.Id.ToString());
        claims.AddRoles(operationClaims.Select(x=>x.Name).ToArray());

        return claims;
    }
}