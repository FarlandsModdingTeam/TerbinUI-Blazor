using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Resources;
using System.Text.Json;

namespace TerbinUI_Blazor.Script
{
    public static class ManagaIdioma
    {
        // ***********************( Variables )*********************** //
        private const string _directory = "./wwwroot/Data/Languages/";

        private static Dictionary<ushort, string>? _cache;

        private static List<string?>? _lenguagesAviable;

        private static string? _currentLanguage;

        // ***********************( Funciones )*********************** //
        private static Dictionary<ushort, string> accesJson(string archivo)
        {
            string rutaCompleta = Path.Combine(_directory, archivo);
            if (!File.Exists(rutaCompleta))
                throw new FileNotFoundException($"No se encontró el archivo: {rutaCompleta}");

            string json = File.ReadAllText(rutaCompleta);
            var diccionario = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

            // Convertir claves string a ushort
            var resultado = new Dictionary<ushort, string>();
            if (diccionario != null)
            {
                foreach (var kvp in diccionario)
                {
                    if (ushort.TryParse(kvp.Key, out ushort clave))
                        resultado[clave] = kvp.Value;
                }
            }
            return resultado;
        }

        private static List<string?>? getLanguages()
        {
            if (!Directory.Exists(_directory))
                return null;

            return Directory.GetFiles(_directory, "*.json")
                .Select(Path.GetFileName)
                .ToList();
        }

        private static (bool success, string menssage) manageLanguages()
        {
            _lenguagesAviable ??= getLanguages();
            if (_lenguagesAviable == null)
                return (false, $"_lenguagesAviable es NUll");
            if (_lenguagesAviable.Count <= 0)
                return (false, $"No se encontraron archivos en: {_directory}");

            _currentLanguage ??=_lenguagesAviable.Find(a => a != null && a.Equals("English.json", StringComparison.OrdinalIgnoreCase))
                ?? _lenguagesAviable.Find(a => a != null && a.Equals("Español.json", StringComparison.OrdinalIgnoreCase));

            if (_currentLanguage == null)
                return (false, $"_currentLanguage es NUll");

            _cache ??= accesJson(_currentLanguage);
            return (true, "");
        }

        public static string GetText(ushort eKey)
        {
            var manage = manageLanguages();
            if (!manage.success)
                return $"{manage.menssage} - {eKey}";

            if (_cache == null)
                return $"_cache es NUll | Nota: Esto nunca deberia suceder - {eKey}";

            return _cache.TryGetValue(eKey, out var texto) ? texto : $"¡¡Nak Nak Nak!! {eKey}";
        }
    }
}
