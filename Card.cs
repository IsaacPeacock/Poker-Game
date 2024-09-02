using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Violet;
public class Card
{
    public enum Suits
    {
        Spades,
        Hearts,
        Diamonds,
        Clubs
    }

    public enum Values
    {
        Two = 2,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        J,
        Q,
        K,
        A
    }

    public Rectangle Rectangle { get; set; } = new Rectangle(0, 0, 140, 190);
    public Suits Suit { get; set; }
    public Values Value { get; set; }
    public Texture2D _texture { get; set; }
    public Texture2D _backTexture { get; set; }
    public bool IsFaceUp { get; set; } = true;
    public Vector2 Position { get; set; }

    public Card(Suits suit, Values value)
    {
        Suit = suit;
        Value = value;
        _backTexture = Globals.Content.Load<Texture2D>("Cards/cardBack_blue5");

        if(suit == Suits.Spades)
        {
            if(value == Values.A || value == Values.J || value == Values.Q || value == Values.K)
            {
                _texture = Globals.Content.Load<Texture2D>("Cards/cardSpades" + value.ToString());
            }
            else
            {
                _texture = Globals.Content.Load<Texture2D>("Cards/cardSpades" + (int)value);
            }
        }

        else if(suit == Suits.Hearts)
        {
            if(value == Values.A || value == Values.J || value == Values.Q || value == Values.K)
            {
                _texture = Globals.Content.Load<Texture2D>("Cards/cardHearts" + value.ToString());
            }
            else
            {
                _texture = Globals.Content.Load<Texture2D>("Cards/cardHearts" + (int)value);
            }
        }

        else if(suit == Suits.Diamonds)
        {
            if(value == Values.A || value == Values.J || value == Values.Q || value == Values.K)
            {
                _texture = Globals.Content.Load<Texture2D>("Cards/cardDiamonds" + value.ToString());
            }
            else
            {
                _texture = Globals.Content.Load<Texture2D>("Cards/cardDiamonds" + (int)value);
            }
        }

        else if(suit == Suits.Clubs)
        {
            if(value == Values.A || value == Values.J || value == Values.Q || value == Values.K)
            {
                _texture = Globals.Content.Load<Texture2D>("Cards/cardClubs" + value.ToString());
            }
            else
            {
                _texture = Globals.Content.Load<Texture2D>("Cards/cardClubs" + (int)value);
            }
        }
    }

    public void Draw()
    {
        if(IsFaceUp)
        {
            Globals.spriteBatch.Draw(_texture, new Rectangle((int)Position.X, (int)Position.Y, 140, 140), Rectangle,Color.White);
        }
        else
        {
            Globals.spriteBatch.Draw(_backTexture, new Rectangle((int)Position.X, (int)Position.Y, 140, 140), Rectangle, Color.White);
        }
    }

    public void Update()
    {
        if(InputManager.LeftClick && IsFaceUp)
        {
            IsFaceUp = !IsFaceUp;
            Draw();
        }

        if(InputManager.LeftClick && !IsFaceUp)
        {
            IsFaceUp = !IsFaceUp;
            Draw();
        }
    }
}