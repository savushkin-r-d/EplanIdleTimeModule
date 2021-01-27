using Eplan.EplApi.HEServices;

namespace IdleTimeModule.EplanAPIHelper
{
    /// <summary>
    /// Обертка над SelectionSet из Eplan
    /// </summary>
    public interface ISelectionSet
    {
        bool LockSelectionByDefault { get; set; }

        IProject GetCurrentProject(bool hideDialog);
    }

    public class EplanSelectionSet : ISelectionSet
    {
        public EplanSelectionSet(SelectionSet selectionSet)
        {
            this.selectionSet = selectionSet;
        }

        public bool LockSelectionByDefault
        {
            get
            {
                return selectionSet.LockSelectionByDefault;
            }
            set
            {
                selectionSet.LockSelectionByDefault = value;
            }
        }

        public IProject GetCurrentProject(bool hideDialog)
        {
            IProject project =
                new EplanProject(selectionSet?.GetCurrentProject(hideDialog));
            return project;
        }

        private SelectionSet selectionSet;
    }
}
