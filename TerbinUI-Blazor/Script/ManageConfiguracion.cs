using Sui.AccesoDatos;

namespace TerbinUI_Blazor.Script
{
    public static class ManageConfiguracion
    {
        // ***********************( Variables )*********************** //
        private const string _filePath = "wwwroot\\Data\\";
        private const string _fileName = "config.json";

        private static Dictionary<string, string>? _config;

        private const string _pathInstancias = "instancias";
        private const string _pathMods = "mods";
        private const string _language = "idioma";
        private const string _apiKey = "apiKey";

        // ***********************( GSI )*********************** //
        public static Dictionary<string, string>? Config
        {
            set { _config = value; }
            get
            {
                handleArchivo();
                return _config;
            }
        }

        public static string PathInstancias
        {
            set
            {
                if (Config != null)
                    Config[_pathInstancias] = value;
            }
            get
            {
                return (Config != null && Config.TryGetValue(_pathInstancias, out string? value)) ? value : "";
            }
        }

        // ***********************( Funciones )*********************** //
        private static bool existsConfig()
        {
            return File.Exists(Path.Combine(_filePath, _fileName));
        }

        private static (bool succes, string menssage) leerConfig(out Dictionary<string, string> exDicionario)
        {
            exDicionario = new();

            if (!existsConfig())
                return (false, "No existe el archivo de configuración.");

            var acceso = AccesoJson.AccesoNormal(_filePath, _fileName, out Dictionary<string, string> diccionario);
            if (!acceso.succes)
                return (false, acceso.menssage);

            exDicionario = diccionario;
            return (true, acceso.menssage);
        }

        public static bool leerConfig()
        {
            var acceso = leerConfig(out var exDicionario);
            if (!acceso.succes)
            {
                Console.WriteLine($"(Terbin-UI > handleArchivo): {acceso.menssage}");
                return false;
            }

            _config = exDicionario;
            return true;
        }

        private static void crearConfigPorDefecto()
        {
            var datos = new Dictionary<string, string>()
            {
                { _pathInstancias, "" },
                { _pathMods, ""},
                { _language, "English.json"},
                { _apiKey, "-1"}
            };
            var json = System.Text.Json.JsonSerializer.Serialize(datos);

            if (!Directory.Exists(_filePath))
                Directory.CreateDirectory(_filePath);

            File.WriteAllText(Path.Combine(_filePath, _fileName), json);

            if (!leerConfig())
                Console.WriteLine("(Terbin-UI > crearConfigPorDefecto): No se pudo leer el archivo de configuración luego de crearlo.");
        }

        private static void handleArchivo()
        {
            if (existsConfig())
            {
                if (!leerConfig())
                    crearConfigPorDefecto();
            }
            else
            {
                crearConfigPorDefecto();
            }
        }

    }
}
