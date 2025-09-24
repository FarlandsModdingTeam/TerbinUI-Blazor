using System.Text.Json;

namespace Sui.AccesoDatos
{
    public static class AccesoJson
    {
        // ***********************( Funciones )*********************** //
        public static (bool succes, string menssage)
            AccesoNormal(string eDirectory, string eArchivo, out Dictionary<string, string> exDiccionario)
        {
            exDiccionario = new();

            string rutaCompleta = Path.Combine(eDirectory, eArchivo);
            if (!File.Exists(rutaCompleta))
                return (false, $"No se encontró el archivo: {rutaCompleta}");

            string? archivo = File.ReadAllText(rutaCompleta);
            if (archivo == null)
                return (false, $"Archivo es Null: {rutaCompleta}");

            exDiccionario = JsonSerializer.Deserialize<Dictionary<string, string>>(archivo) ?? new();
            if (exDiccionario == null)
                return (false, $"No se pudo deserializar el archivo: {rutaCompleta}");

            return (true, string.Empty);
        }

        /*
        // TODO: Reacer para Unity.
        public static string AccesoUnity(string _rutaArchivo_s, string _archivo_s)
        {
            string _rutaCompleta_s = Path.Combine(_rutaArchivo_s, Path.GetFileNameWithoutExtension(_archivo_s));
            _rutaCompleta_s = _rutaCompleta_s.Replace("\\", "/");
            TextAsset _assetJSon = Resources.Load<TextAsset>(_rutaCompleta_s);

            if (_assetJSon == null)
            {
                Debug.LogWarning($"(f_accesoJson_s): No se encontró el archivo en Resources: {_rutaCompleta_s}");
                return null;
            }
            return _assetJSon.text;
        }*/
    }
}
