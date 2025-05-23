using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace Oxigin.Attendance.Core.Services.Managers
{
  class ApiTokenManager
  {
    private RSA privateKey;
    private string apiKey;

    /// <summary>
    /// Initializes a new instance of the ApiTokenManager class.
    /// </summary>
    /// <param name="privateKeyPem">The private key in PEM format.</param>
    /// <param name="apiKey">The API key.</param>
    public ApiTokenManager(string privateKeyPem, string apiKey)
    {
      this.privateKey = RSA.Create();
      this.privateKey.ImportFromPem(privateKeyPem);
      this.apiKey = apiKey;
    }

    /// <summary>
    /// Signs a JWT token with the specified path and body JSON.
    /// </summary>
    /// <param name="path">The path for the JWT token.</param>
    /// <param name="bodyJson">The body JSON for the JWT token.</param>
    /// <returns>The signed JWT token.</returns>
    public string SignJwt(string path, string bodyJson)
    {
      JwtPayload payload = new JwtPayload
        {
            { "uri", path },
            { "nonce", Guid.NewGuid().ToString() },
            { "iat", DateTimeOffset.UtcNow.ToUnixTimeSeconds() },
            { "exp", DateTimeOffset.UtcNow.AddSeconds(120).ToUnixTimeSeconds() },
            { "sub", apiKey }
        };

      if (bodyJson != null)
      {
        payload.Add("bodyHash", CalculateBodyHash(bodyJson));
      }

      var rsaKey = new RsaSecurityKey(privateKey);

      var header = new JwtHeader(new SigningCredentials(rsaKey, SecurityAlgorithms.RsaSha256));
      var secToken = new JwtSecurityToken(header, payload);
      var handler = new JwtSecurityTokenHandler();

      return handler.WriteToken(secToken);
    }

    /// <summary>
    /// Calculates the SHA256 hash of the specified body JSON.
    /// </summary>
    /// <param name="bodyJson">The body JSON to calculate the hash for.</param>
    /// <returns>The calculated SHA256 hash.</returns>
    private string CalculateBodyHash(string bodyJson)
    {
      using (SHA256 sha256 = SHA256.Create())
      {
        byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(bodyJson));
        return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
      }
    }
  }
}
