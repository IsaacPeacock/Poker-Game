using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Violet;

public class AIPlayer
{
    public Card[] Hand { get; set; }
    public Card HighCard { get; set; }
    public string Name { get; set; }
    public int BiddingAmount { get; set; }
    public int Score { get; set; }
    public bool Folded { get; set; }
    public bool Raised { get; set; }


    public AIPlayer(int bid)
	{
        BiddingAmount = bid;
        Hand = new Card[2];
        Score = 0;
        Folded = false;
        Raised = false;
        Name = "";
    }

    public void SetHighCard()
    {
        for (int i = 0; i < Hand.Length - 1; i++)
        {
            if (Hand[i].Value > Hand[i + 1].Value)
            {
                HighCard = Hand[i];
            }

            else
            {
                HighCard = Hand[i + 1];
            }
        }
    }
}
