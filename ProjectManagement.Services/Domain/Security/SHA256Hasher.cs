using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace ProjectManagement.Services.Domains.Security;
public static class SHA256Hasher
{
    public static string Hash(string input)
    {
        byte[] digest = SHA256.HashData(Encoding.ASCII.GetBytes(input));
        StringBuilder sb = new();

        foreach (byte b in digest)
        {
            sb.Append(b.ToString("X2", CultureInfo.InvariantCulture));
        }

        return sb.ToString();
    }
}