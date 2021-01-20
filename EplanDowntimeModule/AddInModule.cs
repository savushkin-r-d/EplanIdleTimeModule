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
            // TODO: Check loaded EasyEplanner
            // TODO: If loaded -> doesn't run this module
            DowntimeModule.Start();
            return true;
        }

        public bool OnExit()
        {
            // TODO: Check loaded EasyEplanner
            // TODO: If loaded -> doesn't stop module
            DowntimeModule.Stop();
            return true;
        }

        public void OnBeforeInit(string strOriginalAssemblyPath)
        {
        }
    }
}
