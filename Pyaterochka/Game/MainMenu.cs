using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pyaterochka
{
    public class MainMenu
    {
        private SpriteFont font;
        private Button playButton;
        private Viewport viewport;

        public bool IsPlayClicked { get; private set; }

        public void LoadContent(ContentManager content, Viewport viewport)
        {
            font = content.Load<SpriteFont>("font");
            this.viewport = viewport;
            
            var buttonWidth = 200;
            var buttonHeight = 50;
            var buttonRect = new Rectangle(
                (viewport.Width / 2) - (buttonWidth / 2),
                (viewport.Height / 2) - (buttonHeight / 2),
                buttonWidth,
                buttonHeight);
            
            playButton = new Button(buttonRect, "Играть");
        }

        public void Update(GameTime gameTime)
        {
            IsPlayClicked = false;
            var mouseState = Mouse.GetState();
            playButton.Update(mouseState);

            if (playButton.IsClicked)
            {
                IsPlayClicked = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            playButton.Draw(spriteBatch, font);
            spriteBatch.End();
        }
    }

    public class Button
    {
        private Rectangle Bounds;
        private string Text;
        private Color Color;
        public bool IsClicked;
        private bool wasPressed;

        public Button(Rectangle bounds, string text)
        {
            Bounds = bounds;
            Text = text;
            Color = Color.White;
        }

        public void Update(MouseState mouseState)
        {
            bool isMouseOver = Bounds.Contains(mouseState.Position);
            IsClicked = false;

            if (isMouseOver)
            {
                Color = Color.Yellow;
                if (mouseState.LeftButton == ButtonState.Pressed)
                {
                    wasPressed = true;
                }
                else if (wasPressed && mouseState.LeftButton == ButtonState.Released)
                {
                    IsClicked = true;
                    wasPressed = false;
                }
            }
            else
            {
                Color = Color.White;
                wasPressed = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            Vector2 textSize = font.MeasureString(Text);
            Vector2 textPosition = new Vector2(
                Bounds.X + (Bounds.Width - textSize.X) / 2,
                Bounds.Y + (Bounds.Height - textSize.Y) / 2);
            
            spriteBatch.DrawString(font, Text, textPosition, Color);
        }
    }
}