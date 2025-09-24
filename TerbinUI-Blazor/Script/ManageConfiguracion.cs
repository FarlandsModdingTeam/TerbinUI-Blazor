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
            get
            {
                // TODO: Gestionar.
                return _config;
            }
        }

        //private static string 

        // ***********************( Funciones )*********************** //
        private static bool existsConfig()
        {
            return File.Exists(Path.Combine(_filePath, _fileName));
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
        }

        private static void handleArchivo()
        {
            if (!existsConfig())
            {
                crearConfigPorDefecto();
            }
        }

    }
}
