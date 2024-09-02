using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Violet;

public class Button
{
    private Texture2D Texture;
    private Vector2 Position;
    public Rectangle rectangle;
    private Color Shade = Color.White;

    public Button(Texture2D t, Vector2 p)
	{
        Texture = t;
        Position = p;
        rectangle = new((int)p.X, (int)p.Y, t.Width, t.Height);
    }

    public void Update()
    {
        if(rectangle.Contains(InputManager.MousePosition))
        {
            Shade = Color.DarkGray;
        }

        else
        {
            Shade = Color.White;
        }
    }

    public void Draw() 
    {
        Globals.spriteBatch.Draw(Texture, Position, Shade);
    }

    public void DrawScaled()
    {
        rectangle = new Rectangle((int)Position.X, (int)Position.Y, 192, 192);
        Globals.spriteBatch.Draw(Texture, rectangle, Shade);
    }
}
