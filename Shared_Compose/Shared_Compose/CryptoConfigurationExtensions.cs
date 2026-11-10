// Autor: Juan Sebastián Montaño Pérez
// Módulo: Seguridad / Criptografía
// Fecha: 2026-11-10

using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Shared.Cryptography
{
    public static class CryptoConfigurationExtensions
    {
        public static IServiceCollection AddSharedCryptography(this IServiceCollection services, IConfiguration configuration)
        {
            // Ruta: 1) Variable de entorno IIS/SO, 2) Secretos de usuario (raíz), 3) Secretos de usuario (sección)
            var keysPath = Environment.GetEnvironmentVariable("DP_KEYS_PATH") // You must defined this variable name on your server
                        ?? configuration["DP_KEYS_PATH"]
                        ?? configuration["DataProtection:KeysPath"];

            if (string.IsNullOrWhiteSpace(keysPath))
            {
                throw new InvalidOperationException("CRITICAL ERROR: We don't found a Cryptography keys (DP_KEYS_PATH), You must defined it on your server like 'IIS (Internet Information Services)' also if you're working on the local project you must defenid it on your 'Secrets user'");
            }

            keysPath = Environment.ExpandEnvironmentVariables(keysPath.Trim());
            var directoryInfo = new DirectoryInfo(keysPath);
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }

            var dataProtection = services.AddDataProtection()
                .PersistKeysToFileSystem(directoryInfo)
                .SetApplicationName("NAME-OF-PROJECT"); // Here's it's your project name.

            if (OperatingSystem.IsWindows())
            {
                dataProtection.ProtectKeysWithDpapi();
            }

            services.AddSingleton<ICryptography, Cryptography>();
            return services;
        }
    }
}
