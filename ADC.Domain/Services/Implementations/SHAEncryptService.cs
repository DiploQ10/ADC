using System.Security.Cryptography;
using System.Text;

namespace ADC.Domain.Services.Implementations;

internal class SHAEncryptService : IEncryptService
{

    /// <summary>
    /// Encripta una clave
    /// </summary>
    public string Encrypt(string clave)
    {
        // Convert the input string to a byte array
        byte[] inputBytes = Encoding.UTF8.GetBytes(clave);

        // Create a SHA-512 hash object

        // Compute the hash value from the input bytes
        byte[] hashBytes = SHA512.HashData(inputBytes);

        return Convert.ToBase64String(hashBytes);

    }
}