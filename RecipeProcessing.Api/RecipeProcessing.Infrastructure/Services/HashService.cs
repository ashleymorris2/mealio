using System.Security.Cryptography;
using RecipeProcessing.Infrastructure.Interfaces;

namespace RecipeProcessing.Infrastructure.Services;

internal class HashService : IHashService
{
    public string ComputeHashFromStream(Stream inputStream)
    {
        ArgumentNullException.ThrowIfNull(inputStream);
        if (inputStream.Length == 0) throw new ArgumentException("Input stream is empty.");
        
        if (inputStream.CanSeek)
        {
            inputStream.Seek(0, SeekOrigin.Begin);
        }
        
        using var sha256 = SHA256.Create();
        Byte[] hashedData = sha256.ComputeHash(inputStream);
        
        return BitConverter.ToString(hashedData).Replace("-", "").ToLowerInvariant();
    }
}