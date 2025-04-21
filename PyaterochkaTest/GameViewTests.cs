using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Moq;
using NUnit.Framework;
using Pyaterochka;

namespace PyaterochkaTests
{
    [TestFixture]
    public class GameViewTests
    {
        private GameView _gameView;
        private Mock<GameModel> _mockModel;

        [SetUp]
        public void Setup()
        {
            _mockModel = new Mock<GameModel>();
            _mockModel.Setup(m => m.TileSize).Returns(40);
            _gameView = new GameView(_mockModel.Object);
        }

        // Проверка отрисовки игрока
        [Test]
        public void Draw_PlayerPosition_CenteredCorrectly()
        {
            var player = new Player(new Vector2(100, 100));
            _mockModel.Setup(m => m.Player).Returns(player);

            var spriteBatch = new Mock<SpriteBatch>();
            _gameView.Draw(spriteBatch.Object);
        }
    }
}