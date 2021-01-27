using System;
using System.Runtime.InteropServices;
using IdleTimeModule.PI;
using System.Threading;
using IdleTimeModule.EplanAPIHelper;

namespace IdleTimeModule
{
    public delegate void ClosingProjectHandler(bool silentMode = true);

    public interface IIdleTimeModule
    {
        /// <summary>
        /// Остановить модуль простоя приложения
        /// </summary>
        void Stop();

        /// <summary>
        /// Запустить поток модуля простоя приложения
        /// </summary>
        /// <param name="assemblyPath">Путь к dll, опционально</param>
        void Start(string assemblyPath = "");

        /// <summary>
        /// Закрыть приложение.
        /// </summary>
        void CloseApplication();

        /// <summary>
        /// Событие вызываемое перед закрытием проекта.
        /// </summary>
        event ClosingProjectHandler BeforeClosingProject;
    }

    /// <summary>
    /// Класс, отвечающий за модуль простоя приложения
    /// </summary>
    public class IdleTimeModule : IIdleTimeModule
    {
        public IdleTimeModule(IEplanHelper eplanHelper,
            IModuleConfiguration moduleConfiguration,
            IRunningProcess runningProcess)
        {
            this.eplanHelper = eplanHelper;
            this.moduleConfiguration = moduleConfiguration;
            this.runningProcess = runningProcess;
        }

        public event ClosingProjectHandler BeforeClosingProject;

        public void Start(string assemblyPath = "")
        {
            moduleConfiguration.Read(assemblyPath);

            idleThread = new Thread(Run);
            idleThread.IsBackground = true;
            idleThread.Start();
        }

        public void Stop()
        {
            isRunning = false;
        }

        public void CloseApplication()
        {
            Stop();
            bool isClosed = runningProcess.CloseMainWindow();
            if (isClosed == false)
            {
                CloseProject();
                var timeout = TimeSpan.FromSeconds(2);
                Thread.Sleep(timeout);
                runningProcess.Kill();
            }
            else
            {
                runningProcess.Close();
            }
        }

        /// <summary>
        /// Закрыть проект в Eplan.
        /// </summary>
        private void CloseProject()
        {
            var project = eplanHelper.GetCurrentProject();
            if (project != null)
            {
                BeforeClosingProject?.Invoke();
                project.Close();
            }
        }

        /// <summary>
        /// Запустить модуль
        /// </summary>
        private void Run()
        {
            isRunning = true;
            while (isRunning)
            {
                CheckIdle();
                Thread.Sleep(moduleConfiguration.CheckInterval);
            }
        }

        /// <summary>
        /// Проверка состояния простоя
        /// </summary>
        private void CheckIdle()
        {
            if (GetLastInputTime() > moduleConfiguration.CheckInterval)
            {
                checksCounter++;
                if(checksCounter == moduleConfiguration.MaxChecksCount)
                {
                    ShowCountdownWindow();
                }
            }
            else
            {
                checksCounter = 0;
            }
        }

        /// <summary>
        /// Показать форму с таймером и запустить таймер
        /// </summary>
        private void ShowCountdownWindow()
        {
            if (form == null || form?.IsDisposed == true)
            {
                form = new IdleTimeModuleForm();
                form.ClosingApp += CloseApplication;
            }

            form.Show();
            form.RunCountdown();
        }

        /// <summary>
        /// Получить время последнего ввода пользователя
        /// </summary>
        /// <returns>Время в миллисекундах</returns>
        private TimeSpan GetLastInputTime()
        {
            uint idleTimeMs = 0;
            var lastInputInfo = new PInvokeUtil.LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;

            uint envTicksMs = (uint)Environment.TickCount;

            if (PInvokeUtil.GetLastInputInfo(ref lastInputInfo))
            {
                uint lastInputMs = lastInputInfo.dwTime;
                idleTimeMs = envTicksMs - lastInputMs;
            }

            idleTimeMs = idleTimeMs > 0 ? idleTimeMs : 0;
            return TimeSpan.FromMilliseconds(idleTimeMs);
        }

        /// <summary>
        /// Счетчик проверок
        /// </summary>
        private int checksCounter = 0;

        /// <summary>
        /// Флаг запуска потока.
        /// </summary>
        private bool isRunning = true;

        /// <summary>
        /// Поток модуля простоя
        /// </summary>
        private Thread idleThread;

        /// <summary>
        /// Хелпер для взаимодействия с API Eplan
        /// </summary>
        private IEplanHelper eplanHelper;

        /// <summary>
        /// Конфигурация модуля простоя
        /// </summary>
        private IModuleConfiguration moduleConfiguration;

        /// <summary>
        /// Форма с отображением обратного отсчета
        /// </summary>
        private IdleTimeModuleForm form;

        /// <summary>
        /// Запущенный процесс, которым управляет модуль простоя.
        /// </summary>
        private IRunningProcess runningProcess;
    }
}
