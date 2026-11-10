// Autor: Juan Sebastián Montaño Pérez
// Módulo: Seguridad / Criptografía
// Fecha: 2026-11-10

using Microsoft.AspNetCore.DataProtection;
using System;
using System.Security.Cryptography;

namespace Shared.Cryptography
{
    public class Cryptography : ICryptography
    {
        private readonly IDataProtector _protector;

        public Cryptography(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("Credentials.v1");
        }

        public string Desencriptar(string textoCifrado)
        {
            if (string.IsNullOrEmpty(textoCifrado)) return string.Empty;

            try
            {
                return _protector.Unprotect(textoCifrado);
            }
            catch (CryptographicException e)
            {
                throw new InvalidOperationException($"No se pudo desencriptar el texto. La llave maestra es incorrecta o los datos están corruptos. {e.Message}", e);
            }
        }

        public string Encriptar(string textoPlano)
        {
            if (string.IsNullOrEmpty(textoPlano)) return string.Empty;
            return _protector.Protect(textoPlano);
        }

        public bool EsTextoCifrado(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto)) return false;

            try
            {
                _protector.Unprotect(texto);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
