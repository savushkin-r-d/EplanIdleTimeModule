using Eplan.EplApi.HEServices;

namespace IdleTimeModule.EplanAPIHelper
{
    public interface IEplanHelper
    {
        /// <summary>
        /// Получить текущий проект
        /// </summary>
        /// <returns></returns>
        IProject GetCurrentProject();
    }

    public class EplanHelper : IEplanHelper
    {
        public IProject GetCurrentProject()
        {
            var selectionSet = GetUnlockedSelectionSet();
            var project = selectionSet.GetCurrentProject(true);
            return project;
        }

        private ISelectionSet GetUnlockedSelectionSet()
        {
            ISelectionSet selectionSet =
                new EplanSelectionSet(new SelectionSet());
            selectionSet.LockSelectionByDefault = false;
            return selectionSet;
        }
    }
}
