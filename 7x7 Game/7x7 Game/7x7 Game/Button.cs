using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace _7x7_Game
{
    class Button
    {
        private Texture2D btTexture;

        private Rectangle btRectangle;

        private Vector2 position;

        private bool _isSelected = false;

        public bool isSelected { get { return _isSelected; } set { _isSelected = value; } }

        private float scale;

        public int index = 0;

        public int delayStart=100;

        public Button(ContentManager content, Vector2 _position, float _scale, string _btSource)
        {
            this.position = _position;
            this.scale = _scale;
            btTexture = content.Load<Texture2D>(_btSource);
        }

        public void Update(TouchCollection touches, GameTime gameTime)
        {
            if (touches.Count > 0 && touches[0].State ==
                 TouchLocationState.Pressed)
            {
                // Examine whether the tapped position is in the rectangle
                Point touchPoint = new
                    Point((int)touches[0].Position.X,
                    (int)touches[0].Position.Y);
                if (btRectangle.Contains(touchPoint))
                {
                    if (isSelected)
                    {
                        isSelected = false;
                    }
                    else
                    {
                        isSelected = true;
                    }
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isSelected)
            {
                spriteBatch.Draw(btTexture, position, new Rectangle(btTexture.Width / 2, 0, btTexture.Width / 2, btTexture.Height), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(btTexture, position, new Rectangle(0, 0, btTexture.Width / 2, btTexture.Height), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
            }

            Vector2 pos = new Vector2(position.X, position.Y);

            btRectangle = new Rectangle((int)pos.X, (int)pos.Y, btTexture.Width / (2*2) , btTexture.Height);
        }
    }
}
