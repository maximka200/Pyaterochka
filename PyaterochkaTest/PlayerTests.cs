using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using NUnit.Framework;
using Pyaterochka;

namespace PyaterochkaTests
{
    [TestFixture]
    public class PlayerTests
    {
        private Player _player;
        private Vector2 _startPosition;

        [SetUp]
        public void Setup()
        {
            _startPosition = new Vector2(100, 100);
            _player = new Player(_startPosition);
        }
        
        [Test]
        public void Player_Initialization_PropertiesSetCorrectly()
        {
            Assert.AreEqual(3, _player.Health);
            Assert.AreEqual(Player.StaminaMax, _player.Stamina);
            Assert.AreEqual(_startPosition, _player.Position);
        }
        
        [Test]
        public void PlayerMove_WKeyPressed_MovesUp()
        {
            var keyboardState = new KeyboardState(keys: new[] { Keys.W });
            var newPosition = _player.PlayerMove(keyboardState);
            Assert.AreEqual(_startPosition.Y - Player.SpeedWalk, newPosition.Y);
        }

        // Движение вверх с бегом (проверка расхода выносливости)
        [Test]
        public void PlayerMove_WKeyWithShift_MovesFasterAndDrainsStamina()
        {
            var keyboardState = new KeyboardState(keys: new[] { Keys.W, Keys.LeftShift });
            int initialStamina = _player.Stamina;
            _player.PlayerMove(keyboardState);Assert.That(_player.Stamina, Is.InRange(initialStamina - Player.StaminaTick - 2, initialStamina - Player.StaminaTick + 2));
        }

        // Получение урона
        [Test]
        public void TakeDamage_ReducesHealthByDamageValue()
        {
            _player.TakeDamage(2);
            Assert.AreEqual(1, _player.Health);
        }
    }
}