using System;
using System.IO;
using Xunit;
using MountBlade2BannerlordTrainer;

namespace MountBlade2BannerlordTrainer.Tests
{
    /// <summary>
    /// Unit tests for the TrainerUI class.
    /// Verifies that events are triggered correctly on key presses.
    /// </summary>
    public class TrainerUITests
    {
        [Fact]
        public void TestInfiniteHealthToggleEvent()
        {
            // Arrange
            var ui = new TrainerUI();
            bool eventFired = false;
            ui.OnInfiniteHealthToggle += () => eventFired = true;

            // Simulate F1 key press by redirecting console input
            var input = new StringReader("F1\n");
            Console.SetIn(input);

            // Act
            ui.Start();
            System.Threading.Thread.Sleep(200); // Allow UI thread to process

            // Assert
            Assert.True(eventFired, "Infinite health toggle event should have fired.");
        }

        [Fact]
        public void TestMaxSkillsActivateEvent()
        {
            // Arrange
            var ui = new TrainerUI();
            bool eventFired = false;
            ui.OnMaxSkillsActivate += () => eventFired = true;

            var input = new StringReader("F3\n");
            Console.SetIn(input);

            // Act
            ui.Start();
            System.Threading.Thread.Sleep(200);

            // Assert
            Assert.True(eventFired, "Max skills activate event should have fired.");
        }

        [Fact]
        public void TestExitEvent()
        {
            // Arrange
            var ui = new TrainerUI();
            bool eventFired = false;
            ui.OnExit += () => eventFired = true;

            var input = new StringReader("F4\n");
            Console.SetIn(input);

            // Act
            ui.Start();
            System.Threading.Thread.Sleep(200);

            // Assert
            Assert.True(eventFired, "Exit event should have fired.");
        }
    }
}
