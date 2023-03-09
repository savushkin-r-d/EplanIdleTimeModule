using Eplan.EplApi.DataModel;

namespace IdleTimeModule.EplanAPIHelper
{
    public interface IProject
    {
        void Close();

        bool IsOpenProject { get; }
    }

    public class EplanProject : IProject
    {
        public EplanProject(Project project)
        {
            eplanProject = project;
        }

        public void Close()
        {
            eplanProject?.Close();
        }

        public bool IsOpenProject 
        {
            get 
            {
                return eplanProject != null;
            } 
        }
        private Project eplanProject;
    }
}
