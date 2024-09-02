using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Violet;

public class Player
{
	public Card[] hand {  get; set; }
	public Card HighCard { get; set; }
	public int BiddingAmount {  get; set; }
	public int Score { get; set; }
	public bool Folded { get; set; }
	public bool Raised { get; set; }

	public Player(int bid)
	{
		BiddingAmount = bid;
		hand = new Card[2];
		Score = 0;
		Folded = false;
		Raised = false;
	}

	public void SetHighCard()
	{
		for(int i = 0; i < hand.Length - 1; i++) 
		{
			if (hand[i].Value > hand[i + 1].Value)
			{
				HighCard = hand[i];
			}

			else
			{
				HighCard = hand[i + 1];
			}
		}
	}
}
