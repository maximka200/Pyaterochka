using Microsoft.Xna.Framework;
using NUnit.Framework;
using Pyaterochka;
using Pyaterochka.Buyers;

namespace PyaterochkaTests
{
    [TestFixture]
    public class GameModelTests
    {
        private GameModel _gameModel;

        [SetUp]
        public void Setup()
        {
            _gameModel = new GameModel();
        }

        // Проверка завершения игры при нулевом здоровье
        [Test]
        public void Update_WhenPlayerHealthIsZero_SetsGameOver()
        {
            _gameModel.Player.TakeDamage(3);
            _gameModel.Update(new GameTime());
            Assert.IsTrue(_gameModel.IsGameOver);
        }

        // Проверка удаления забаненных покупателей
        [Test]
        public void Update_WhenBuyerIsBanned_RemovesFromList()
        {
            var buyer = new Babushka(Vector2.Zero, _gameModel.Player);
            buyer.Ban();
            _gameModel.Buyers.Add(buyer);
            
            _gameModel.Update(new GameTime());
            Assert.IsFalse(_gameModel.Buyers.Contains(buyer));
        }
    }
}