using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;
using Pyaterochka;

namespace PyaterochkaTests
{
    [TestFixture]
    public class GameControllerTests
    {
        private GameController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new GameController();
        }

        // Проверка обработки нажатия Enter
        [Test]
        public void Update_EnterKeyPressed_TriggersAccusation()
        {
            var keyboardState = new KeyboardState(keys: new[] { Keys.Enter });
            _controller.Update(new GameTime()); // Первое нажатие
            // Проверьте здесь логику AccusationSystem, если требуется
        }
    }
}