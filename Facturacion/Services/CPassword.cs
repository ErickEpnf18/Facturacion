using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NesFactApiV4.Services
{
    public class CPassword
    {
        public bool VerificaPasswordHash(string Validador, byte[] ValidadorHash, byte[] ValidadorSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(ValidadorSalt))
            {
                var hashComputado = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Validador));

                for (int i = 0; i < hashComputado.Length; i++)
                {
                    if (hashComputado[i] != ValidadorHash[i]) return false;
                }
            }
            return true;
        }

        public void CrearPasswordHash(string Validador, out byte[] ValidadorHash, out byte[] ValidadorSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                ValidadorSalt = hmac.Key;
                ValidadorHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(Validador));
            }
        }
    }
}
