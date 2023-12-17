using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace aziotedge_modinit;

public class Sas
{
    static string Sign(string requestString, string key)
        => Convert.ToBase64String(new HMACSHA256(Convert.FromBase64String(key)).ComputeHash(Encoding.UTF8.GetBytes(requestString)));

    internal static string GetSignature(string encodedURI, string key, long expiry)
        => Sign($"{encodedURI}\n{expiry}", key);

    public static string GetToken(string encodedURI, string key, long expiry = 0)
    {
        const int DEFAULT_TOKEN_EXPIRY_MINS = 60;
        long expiryValue = expiry == 0 ? DateTimeOffset.UtcNow.AddMinutes(DEFAULT_TOKEN_EXPIRY_MINS).ToUnixTimeSeconds() : expiry;
        string sig = WebUtility.UrlEncode(Sign($"{encodedURI}\n{expiryValue}", key));
        return $"SharedAccessSignature sr={encodedURI}&sig={sig}&se={expiryValue}";
    }
}