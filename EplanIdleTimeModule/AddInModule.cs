using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.Starter;
using IdleTimeModule.EplanAPIHelper;

[assembly: EplanSignedAssemblyAttribute(true)]

namespace IdleTimeModule.AddIn
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
            IEplanHelper eplanHelper = new EplanHelper();
            IModuleConfiguration moduleConfiguration =
                new ModuleConfiguration();
            idleTimeModule =
                new IdleTimeModule(eplanHelper, moduleConfiguration);
            idleTimeModule.Start(OriginalAssemblyPath);
            return true;
        }

        public bool OnExit()
        {
            idleTimeModule?.Stop();
            return true;
        }

        public void OnBeforeInit(string strOriginalAssemblyPath)
        {
            OriginalAssemblyPath = strOriginalAssemblyPath;
        }

        private IdleTimeModule idleTimeModule;

        /// <summary>
        /// Путь к дополнению (откуда подключена).
        /// </summary>
        public static string OriginalAssemblyPath;
    }
}
