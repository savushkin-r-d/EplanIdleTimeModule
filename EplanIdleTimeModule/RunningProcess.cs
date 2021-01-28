using System.Diagnostics;

namespace IdleTimeModule
{
    /// <summary>
    /// Запущенный процесс
    /// </summary>
    public interface IRunningProcess
    {
        /// <summary>
        /// Закрывает процесс, имеющий пользовательский интерфейс,
        /// посылая сообщение о закрытии главному окну процесса
        /// </summary>
        /// <returns></returns>
        bool CloseMainWindow();

        /// <summary>
        /// Освобождает все ресурсы, связанные с этим компонентом
        /// </summary>
        void Close();

        /// <summary>
        /// Немедленно останавливает связанный процесс
        /// </summary>
        void Kill();
    }

    public class RunningProcess : IRunningProcess
    {
        public RunningProcess(Process process)
        {
            this.process = process;
        }

        public bool CloseMainWindow()
        {
            return process.CloseMainWindow();    
        }

        public void Close()
        {
            process.Close();
        }

        public void Kill()
        {
            process.Kill();
        }

        Process process;
    }
}
