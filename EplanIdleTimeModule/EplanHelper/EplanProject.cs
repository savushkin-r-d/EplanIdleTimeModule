using Eplan.EplApi.DataModel;

namespace IdleTimeModule.EplanAPIHelper
{
    public interface IProject
    {
        void Close();
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

        private Project eplanProject;
    }
}
