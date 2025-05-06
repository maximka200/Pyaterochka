using NUnit.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pyaterochka;

[TestFixture]
public class MainMenuTests
{
    [Test]
    public void MainMenu_IsPlayClicked_DefaultsToFalse()
    {
        var menu = new MainMenu();
        Assert.IsFalse(menu.IsPlayClicked);
    }

    private class FakeContentManager : ContentManager
    {
        public FakeContentManager() : base(null) { }

        public override T Load<T>(string assetName)
        {
            if (typeof(T) == typeof(SpriteFont))
                return (T)(object)null; // Заглушка без создания SpriteFont
            throw new NotImplementedException();
        }
    }

}