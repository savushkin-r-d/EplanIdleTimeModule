using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Starter;

[assembly: EplanSignedAssemblyAttribute(true)]

namespace DowntimeModule.AddIn
{
    /// <summary>
    /// Точка входа в приложение, стартовый класс.
    /// </summary>
    public class AddInModule : IEplAddIn, IEplAddInShadowCopy
    {
        public bool OnInitGui()
        {
            return true;
        }

        public bool OnRegister(ref bool bLoadOnStart)
        {
            bLoadOnStart = true;
            return true;
        }

        public bool OnUnregister()
        {
            return true;
        }

        public bool OnInit()
        {
            ModuleConfiguration.Read(OriginalAssemblyPath);
            DowntimeModule.Start();
            return true;
        }

        public bool OnExit()
        {
            return true;
        }

        public void OnBeforeInit(string strOriginalAssemblyPath)
        {
            OriginalAssemblyPath = strOriginalAssemblyPath;
        }

        /// <summary>
        /// Путь к дополнению (откуда подключена).
        /// </summary>
        public static string OriginalAssemblyPath;
    }
}
