using System;
using System.Threading;

namespace MountBlade2BannerlordTrainer
{
    /// <summary>
    /// Handles the console-based user interface for the trainer.
    /// Provides real-time status updates and key bindings.
    /// </summary>
    public class TrainerUI
    {
        private bool _infiniteHealthActive;
        private bool _unlimitedGoldActive;
        private bool _maxSkillsActive;
        private Thread _uiThread;

        public event Action OnInfiniteHealthToggle;
        public event Action OnUnlimitedGoldToggle;
        public event Action OnMaxSkillsActivate;
        public event Action OnExit;

        public void Start()
        {
            _uiThread = new Thread(Run);
            _uiThread.IsBackground = true;
            _uiThread.Start();
        }

        private void Run()
        {
            Console.Clear();
            Console.WriteLine("=== Mount & Blade II: Bannerlord Trainer ===");
            Console.WriteLine("Press F1: Toggle Infinite Health");
            Console.WriteLine("Press F2: Toggle Unlimited Gold");
            Console.WriteLine("Press F3: Activate Max Skills");
            Console.WriteLine("Press F4: Exit");
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Status:");
            UpdateStatus();

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    switch (key)
                    {
                        case ConsoleKey.F1:
                            _infiniteHealthActive = !_infiniteHealthActive;
                            OnInfiniteHealthToggle?.Invoke();
                            UpdateStatus();
                            break;
                        case ConsoleKey.F2:
                            _unlimitedGoldActive = !_unlimitedGoldActive;
                            OnUnlimitedGoldToggle?.Invoke();
                            UpdateStatus();
                            break;
                        case ConsoleKey.F3:
                            _maxSkillsActive = true;
                            OnMaxSkillsActivate?.Invoke();
                            UpdateStatus();
                            break;
                        case ConsoleKey.F4:
                            OnExit?.Invoke();
                            return;
                    }
                }
                Thread.Sleep(50);
            }
        }

        private void UpdateStatus()
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.WriteLine($"Infinite Health: {(_infiniteHealthActive ? "ON" : "OFF")}");
            Console.WriteLine($"Unlimited Gold: {(_unlimitedGoldActive ? "ON" : "OFF")}");
            Console.WriteLine($"Max Skills: {(_maxSkillsActive ? "ACTIVATED" : "INACTIVE")}");
        }
    }
}
