using System.Diagnostics;
using System.Text.Json;

namespace TerbinUI_Blazor.Script
{
    public static class ManagaIdioma
    {
        // ***********************( Variables )*********************** //
        private const string _directory = "wwwroot\\Data\\Languages\\";

        private static Dictionary<ushort, string>? _cache;

        private static List<string?>? _lenguagesAviable;

        private static string? _currentLanguage;

        // ***********************( Eventos )*********************** //
        public static event Action? OnLanguageChanged;

        // ***********************( GSI )*********************** //
        public static string? CurrentLanguage
        {
            get
            {
                var manage = manageLanguages();
                if (!manage.success)
                {
                    Console.WriteLine(manage.menssage);
                    return null;
                }

                return _currentLanguage;
            }
            set
            {
                if (value == null || value.Equals(_currentLanguage, StringComparison.OrdinalIgnoreCase))
                    return;
                if (!ExisteLanguage(value))
                    return;

                Console.WriteLine($"Cambiando idioma a: {value}");
                OnLanguageChanged?.Invoke();
                _currentLanguage = value;
                _cache = accesJson(_currentLanguage);
            }
        }

        public static List<string?> Idiomas
        {
            get
            {
                var manage = manageAviables();
                if (!manage.success)
                {
                    Console.WriteLine(manage.menssage);
                    return new List<string?>();
                }
                return _lenguagesAviable ?? new List<string?>();
            }
        }

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

        public static string GetLine(string eLanguage, ushort eKey)
        {
            return accesJson(eLanguage).TryGetValue(eKey, out var texto) ? texto : $"¡¡Nak Nak Nak!! {eKey}";
        }

        private static List<string?>? getLanguages()
        {
            if (!Directory.Exists(_directory))
                return null;

            return Directory.GetFiles(_directory, "*.json")
                .Select(Path.GetFileName)
                .ToList();
        }

        private static (bool success, string menssage) manageAviables()
        {
            _lenguagesAviable ??= getLanguages();
            if (_lenguagesAviable == null)
                return (false, $"_lenguagesAviable es NUll");
            if (_lenguagesAviable.Count <= 0)
                return (false, $"No se encontraron archivos en: {_directory}");
            return (true, "");
        }

        public static Dictionary<string, string> GetLanguages()
        {
            var manage = manageAviables();
            if (!manage.success)
            {
                Console.WriteLine(manage.menssage);
                return new Dictionary<string, string>();
            }
            // Filtrar nulos antes de usar ToDictionary
            return _lenguagesAviable!
                .Where(lang => !string.IsNullOrEmpty(lang))
                .ToDictionary(
                    lang => lang!, // lang no es nulo aquí
                    lang => GetLine(lang!, 0) // lang no es nulo aquí
                );
        }

        public static bool ExisteLanguage(string eLanguage)
        {
            var manage = manageAviables();
            if (!manage.success)
            {
                Console.WriteLine(manage.menssage);
                return false;
            }
            return _lenguagesAviable!.Contains(eLanguage);
        }

        private static (bool success, string menssage) manageLanguages()
        {
            var manage = manageAviables();
            if (!manage.success)
                return manage;

            _currentLanguage ??= _lenguagesAviable?.Find(a => a != null && a.Equals("English.json", StringComparison.OrdinalIgnoreCase))
                ?? _lenguagesAviable?.Find(a => a != null && a.Equals("Español.json", StringComparison.OrdinalIgnoreCase));

            if (CurrentLanguage == null)
                return (false, $"_currentLanguage es NUll");

            _cache ??= accesJson(CurrentLanguage);
            return (true, "");
        }

        public static string GetText(ushort eKey)
        {
            try
            {
                var manage = manageLanguages();
                if (!manage.success)
                    return $"{manage.menssage} - {eKey}";

                if (_cache == null)
                    return $"_cache es NUll | Nota: Esto nunca deberia suceder - {eKey}";

                return _cache.TryGetValue(eKey, out var texto) ? texto : $"¡¡Nak Nak Nak!! {eKey}";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error al obtener el texto para la clave {eKey}: {ex.Message}");
                return $"Error: {ex.Message} - {eKey}";
            }
        }
    }
}
