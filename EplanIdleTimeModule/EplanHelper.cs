using Eplan.EplApi.DataModel;
using Eplan.EplApi.HEServices;

namespace IdleTimeModule.EplanAPIHelper
{
    public interface IEplanHelper
    {
        /// <summary>
        /// Получить текущий проект
        /// </summary>
        /// <returns></returns>
        Project GetCurrentProject();
    }

    public class EplanHelper : IEplanHelper
    {
        public Project GetCurrentProject()
        {
            var selectionSet = GetUnlockedSelectionSet();
            return selectionSet.GetCurrentProject(true);
        }

        protected SelectionSet GetUnlockedSelectionSet()
        {
            var selectionSet = new SelectionSet();
            selectionSet.LockSelectionByDefault = false;
            return selectionSet;
        }
    }
}
