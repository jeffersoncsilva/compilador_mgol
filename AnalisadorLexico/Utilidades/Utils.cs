using System.Reflection;

namespace CompiladorMGol.Utilidades;

internal class Utils
{
    public static string PegaCaminhoPastaResources()
    {
        var path = Assembly.GetEntryAssembly()?.Location;

        if (path == null)
            path = "";
        int ultimoIdx = path.LastIndexOf("\\");
        path = path.Substring(0, ultimoIdx);
        return path;
    }
}
