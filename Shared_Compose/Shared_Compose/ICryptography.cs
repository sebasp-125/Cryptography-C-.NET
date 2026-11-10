// Autor: Juan Sebastián Montaño Pérez
// Módulo: Seguridad / Criptografía
// Fecha: 2026-11-10

namespace Shared.Cryptography
{
    public interface ICryptography
    {
        string Encriptar(string textoPlano);
        string Desencriptar(string textoCifrado);
        bool EsTextoCifrado(string texto);
    }
}
