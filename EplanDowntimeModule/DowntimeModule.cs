using System;
using System.Runtime.InteropServices;
using PInvoke;
using System.Threading;
using System.Diagnostics;
using EplanAPIHelper;

namespace DowntimeModule
{
    /// <summary>
    /// Класс, отвечающий за модуль простоя приложения
    /// </summary>
    public static class DowntimeModule
    {
        /// <summary>
        /// Запустить поток модуля простоя приложения
        /// </summary>
        public static void Start()
        {
            idleThread = new Thread(Run);
            idleThread.Start();
        }

        /// <summary>
        /// Остановить модуль простоя приложения
        /// </summary>
        public static void Stop()
        {
            isRunning = false;
        }

        /// <summary>
        /// Закрыть приложение.
        /// </summary>
        public static void CloseApplication()
        {
            Process eplanProcess = Process.GetCurrentProcess();
            var isClosed = eplanProcess.CloseMainWindow();
            if (isClosed == false)
            {
                var project = EplanHelper.GetCurrentProject();
                if (project != null)
                {
                    project.Close();
                }

                var timeout = new TimeSpan(0, 0, 2);
                Thread.Sleep(timeout);
                eplanProcess.Kill();
            }
            else
            {
                eplanProcess.Close();
            }
        }

        /// <summary>
        /// Запустить модуль
        /// </summary>
        private static void Run()
        {
            isRunning = true;
            while (isRunning)
            {
                CheckIdle();
                Thread.Sleep(ModuleConfiguration.CheckInterval);
            }
        }

        /// <summary>
        /// Проверка состояния простоя
        /// </summary>
        private static void CheckIdle()
        {
            if (GetLastInputTime() > ModuleConfiguration.CheckInterval)
            {
                checksCounter++;
                if(checksCounter == ModuleConfiguration.MaxChecksCount)
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
        private static void ShowCountdownWindow()
        {
            DowntimeModuleForm.Form.Show();
            DowntimeModuleForm.Form.RunCountdown();
        }

        /// <summary>
        /// Получить время последнего ввода пользователя
        /// </summary>
        /// <returns>Время в миллисекундах</returns>
        private static TimeSpan GetLastInputTime()
        {
            uint idleTime = 0;
            PI.LASTINPUTINFO lastInputInfo = new PI.LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;

            uint envTicks = (uint)Environment.TickCount;

            if (PI.GetLastInputInfo(ref lastInputInfo))
            {
                uint lastInputTick = lastInputInfo.dwTime;
                idleTime = envTicks - lastInputTick;
            }

            return new TimeSpan((idleTime > 0) ? idleTime * TicksMultiplier : 0);
        }

        /// <summary>
        /// Множитель для тиков TimeSpan, чтобы корректно перевести время
        /// </summary>
        const int TicksMultiplier = 10000;

        /// <summary>
        /// Счетчик проверок
        /// </summary>
        private static int checksCounter = 0;

        /// <summary>
        /// Флаг запуска потока.
        /// </summary>
        private static bool isRunning = true;

        /// <summary>
        /// Поток модуля простоя
        /// </summary>
        private static Thread idleThread;
    }
}
