using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Violet;

public static class Globals
{
	public static ContentManager Content { get; set; }
	public static SpriteBatch spriteBatch { get; set; }
    public static Point windowSize { get; } = new Point(1200, 600);
}
