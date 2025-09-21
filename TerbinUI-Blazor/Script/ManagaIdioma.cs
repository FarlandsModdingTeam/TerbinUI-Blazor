using System.Collections.Generic;

namespace TerbinUI_Blazor.Script
{
    public static class ManagaIdioma
    {
        private const string directory = "./wwwroot/Data/Languages/";

        private static Dictionary<ushort, string> getLanguages()
        {
            return new();
        }

        public static string GetText(ushort eKey)
        {
            return $"¡¡Nak Nak Nak!! {eKey}";
        }
    }
}
