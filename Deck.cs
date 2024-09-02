using System.Collections.Generic;       
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Violet;

public class Deck
{ 
    private readonly List<Card> cards;
    private Texture2D texture;

    public Vector2 Position { get; set; }

    public Rectangle rectangleArea { get; }

    public int round { get; private set;}

    public Deck(Vector2 position)
    {
        round = 0;
        Position = position;
        texture = Globals.Content.Load<Texture2D>("Cards/cardBack_blue5");
        rectangleArea = new(Position.ToPoint(), new(texture.Width, texture.Height));

        cards = new List<Card>();

        foreach (Card.Suits suit in System.Enum.GetValues(typeof(Card.Suits)))
        {
            foreach (Card.Values value in System.Enum.GetValues(typeof(Card.Values)))
            {
                cards.Add(new Card(suit, value));
            }
        }
    }

    public void Shuffle()
    {
        Random random = new Random();
        int n = cards.Count;

        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            Card value = cards[k];
            cards[k] = cards[n];
            cards[n] = value;
        }
    }

    public Card DrawCard()
    {
        Card card = cards.First();
        cards.Remove(card);
        return card;
    }

    public void Draw()
    {
        Globals.spriteBatch.Draw(texture, Position, Color.White);
    }

    public void Clicked(List<Card> cardList)
    {
        if(InputManager.LeftClick)
        {
            if(rectangleArea.Contains(InputManager.MousePosition) && round == 0)
            {
                for(int i = 0; i < 3; i++)
                {
                    cardList.Add(DrawCard());
                    cardList[i].Position = new Vector2(100 + (i * 200), 100);
                }
                round++;

            }

            else if((rectangleArea.Contains(InputManager.MousePosition)) && (round == 1 || round == 2))
            {
                    cardList.Add(DrawCard());

                if (round == 1)
                    cardList[3].Position = new Vector2(100 + (3 * 200), 100);

                if (round == 2)
                    cardList[4].Position = new Vector2(100 + (4 * 200), 100);
                    
                round++;
            }
        }
    }
}	

