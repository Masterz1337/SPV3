using System.IO;
using Atarashii.Modules.OpenSauce;

namespace Atarashii.API
{
    /// <summary>
    ///     Static API for the Atarashii OpenSauce Module.
    /// </summary>
    public static class OpenSauce
    {
        /// <summary>
        ///     Retrieves a Configuration-type representation of the provided OpenSauce User configuration XML file.
        /// </summary>
        /// <param name="path">
        ///     Path to the OpenSauce User configuration XML file.
        /// </param>
        /// <returns>
        ///     Deserialised Configuration object representing the OpenSauce User configuration XML file.
        /// </returns>
        public static Configuration Parse(string path)
        {
            return ConfigurationFactory.GetFromXml(File.ReadAllText(path));
        }

        /// <summary>
        ///     Installs OpenSauce to the filesystem.
        /// </summary>
        /// <param name="path">
        ///     HCE installation directory.
        ///     This path is expected to contain a valid HCE executable.
        /// </param>
        public static void Install(string path)
        {
            new InstallerFactory(path).Get().Install();
        }
    }
}