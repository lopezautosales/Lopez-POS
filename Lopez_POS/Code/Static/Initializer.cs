using Lopez_POS.Web;

namespace Lopez_POS.Static
{
    /// <summary>
    /// Class for initializing project
    /// </summary>
    internal static class Initializer
    {
        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal static void Initialize()
        {
            Storage.Init();
            WebManager.Init();
        }
    }
}