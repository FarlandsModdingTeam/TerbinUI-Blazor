using System.Text.Json;

namespace Sui.AccesoDatos
{
    public static class AccesoJson
    {
        // ***********************( Funciones )*********************** //
        public static (bool succes, string menssage)
            AccesoNormal(string eDirectory, string eArchivo, out string exContenido)
        {
            exContenido = string.Empty;

            string rutaCompleta = Path.Combine(eDirectory, eArchivo);
            if (!File.Exists(rutaCompleta))
                return (false, $"No se encontró el archivo: {rutaCompleta}");

            string? archivo = File.ReadAllText(rutaCompleta);
            if (archivo == null)
                return (false, $"Archivo es Null: {rutaCompleta}");

            exContenido = archivo;
            return (true, "Acceso Exitoso");
        }

        public static (bool succes, string menssage)
            AccesoNormal(string eDirectory, string eArchivo, out Dictionary<string, string> exDiccionario)
        {
            exDiccionario = new();

            var acceso = AccesoNormal(eDirectory, eArchivo, out string archivo);
            if (!acceso.succes)
                return (false, acceso.menssage);

            exDiccionario = JsonSerializer.Deserialize<Dictionary<string, string>>(archivo) ?? new();
            if (exDiccionario == null)
                return (false, $"No se pudo deserializar el archivo: {Path.Combine(eDirectory, eArchivo)}");

            return (true, acceso.menssage);
        }

        public static (bool succes, string menssage)
            AccesoNormal(string eDirectory, string eArchivo, string eContenido)
        {
            try
            {
                string rutaCompleta = Path.Combine(eDirectory, eArchivo);
                string json = JsonSerializer.Serialize(eContenido);

                if (!Directory.Exists(eDirectory))
                    Directory.CreateDirectory(eDirectory);
                File.WriteAllText(rutaCompleta, json);

                return (true, "Guardado Exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al guardar el archivo: {ex.Message}");
            }
        }

        public static (bool succes, string menssage)
            AccesoNormal(string eDirectory, string eArchivo, Dictionary<string, string> eDiccionario)
        {
            try
            {
                string rutaCompleta = Path.Combine(eDirectory, eArchivo);
                string json = JsonSerializer.Serialize(eDiccionario);

                if (!Directory.Exists(eDirectory))
                    Directory.CreateDirectory(eDirectory);
                File.WriteAllText(rutaCompleta, json);

                return (true, "Guardado Exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al guardar el archivo: {ex.Message}");
            }
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
