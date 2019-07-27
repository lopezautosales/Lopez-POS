using Lopez_Auto_Sales.Web;

namespace Lopez_Auto_Sales.Static
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