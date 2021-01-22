using System;
using System.Windows.Forms;

namespace IdleTimeModule
{
    public partial class IdleTimeModuleForm : Form
    {
        public delegate void BeforeClosingAppHandler();

        public event BeforeClosingAppHandler BeforeClosingApp;

        public IdleTimeModuleForm()
        {
            InitializeComponent();
            TopMost = true;
            startingCountdownSec = 60;
        }

        /// <summary>
        /// Инициализация таймера.
        /// </summary>
        public void RunCountdown()
        {
            timerLabel.Text = $"Осталось: {startingCountdownSec} с.";
            RunTimer();
        }

        /// <summary>
        /// Запуск отсчета времени и запуск формы.
        /// </summary>
        private void RunTimer()
        {
            countdownTimer = new Timer();
            countdownTimer.Interval = CountdownIntervalMillisec;
            countdownTimer.Tick += new EventHandler(TimerWorking);
            countdownTimer.Start();

            Application.Run(this);
        }

        /// <summary>
        /// Остановить таймер.
        /// </summary>
        private void StopCountdown()
        {
            countdownTimer.Stop();
            countdownTimer.Tick -= new EventHandler(TimerWorking);
        }

        /// <summary>
        /// Работа таймера.
        /// </summary>
        /// <param name="sencder"></param>
        /// <param name="e"></param>
        private void TimerWorking(object sencder, EventArgs e)
        {
            startingCountdownSec--;
            if (startingCountdownSec > 0)
            {
                timerLabel.Text = $"Осталось: {startingCountdownSec} с.";
            }
            else
            {
                Hide();
                Close();
                StopCountdown();
                BeforeClosingApp?.Invoke();
            }
        }

        /// <summary>
        /// Подтверждение активности.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void acceptButton_Click(object sender, EventArgs e)
        {
            StopCountdown();
            Hide();
            Close();
        }

        /// <summary>
        /// Таймер.
        /// </summary>
        private Timer countdownTimer;

        /// <summary>
        /// Стартовое значение таймера.
        /// </summary>
        private int startingCountdownSec;

        /// <summary>
        /// Интервал опроса таймера в миллисекундах.
        /// </summary>
        private const int CountdownIntervalMillisec = 1000;
    }
}