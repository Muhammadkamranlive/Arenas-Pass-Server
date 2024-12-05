using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Server.Services
{
    public class KeyManagementService
    {
        private byte[] GenerateSeed(int tenantId)
        {
            using (var sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(Encoding.UTF8.GetBytes($"Tenant-{tenantId}-UniqueSeed"));
            }
        }

        public (string publicKey, string privateKey) GenerateKeyPair(int tenantId)
        {
            var seed = GenerateSeed(tenantId);

            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                // Optionally: Use the seed to influence the random number generator
                rsa.PersistKeyInCsp = false;

                var privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
                var publicKey  = Convert.ToBase64String(rsa.ExportRSAPublicKey());

                return (publicKey, privateKey);
            }
        }



        public string EncryptPrivateKey(string privateKey)
        {
            // Add encryption logic (e.g., AES encryption) to secure the private key
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(privateKey));
        }

        public string DecryptPrivateKey(string encryptedKey)
        {
            // Decrypt the encrypted private key
            return Encoding.UTF8.GetString(Convert.FromBase64String(encryptedKey));
        }
    }
}
