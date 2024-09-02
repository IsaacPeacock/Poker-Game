using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Violet;
public class PokerRules
{
    private List<Card> combinedHand; 
    public List<Card> DealerHand { get; set; }
    public int round {  get; set; }
    public int pot {  get; private set; }
    private int numStraight;
    private int numSpades, numClubs, numHearts, numDiamonds;
    private Card[] straightCards;

    public PokerRules()
    {
        DealerHand = new List<Card>();
        pot = 0;
        round = 0;
    }

    public bool IsPair()
    {
        for (int i = 0; i < combinedHand.Count - 1; i++)
        {
            if (combinedHand[i].Value == combinedHand[i + 1].Value)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsTwoPair()
    {
        int pairCount = 0;
        for (int i = 0; i < combinedHand.Count - 1; i++)
        {
            if (pairCount == 2)
            {
                return true;
            }

            if (combinedHand[i].Value == combinedHand[i + 1].Value)
            {
                pairCount++;
                i++;
            }
        }
        if (pairCount == 2)
        {
            return true;
        }
        return false;
    }

    public bool IsThreeOfAKind()
    {
        for (int i = 0; i < combinedHand.Count - 2; i++)
        {
            if (combinedHand[i].Value == combinedHand[i + 1].Value && combinedHand[i + 1].Value == combinedHand[i + 2].Value)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsStraight()
    {
        straightCards = new Card[5];
        numStraight = 0;
        for (int i = 0; i < combinedHand.Count - 1; i++)
        {
            if (combinedHand[i].Value + 1 != combinedHand[i + 1].Value)
            {
                numStraight = 0;
            }

            else
            {
                straightCards[numStraight] = combinedHand[i];
                numStraight++;
            }

            if(numStraight == 5)
            {
                return true;
            }
        }
        return false;
    }

    public bool IsFlush()
    {
        numSpades = 1;
        numClubs = 1;
        numHearts = 1;
        numDiamonds = 1;

        for (int i = 0; i < combinedHand.Count - 1; i++)
        {
            if (combinedHand[i].Suit == combinedHand[i + 1].Suit)
            {
                if (combinedHand[i].Suit == Card.Suits.Spades)
                {
                    numSpades++;
                }

                else if (combinedHand[i].Suit == Card.Suits.Clubs)
                {
                    numClubs++;
                }

                else if (combinedHand[i].Suit == Card.Suits.Hearts)
                {
                    numHearts++;
                }

                else if (combinedHand[i].Suit == Card.Suits.Diamonds)
                {
                    numDiamonds++;
                }

                if (numClubs == 5 || numSpades == 5 || numHearts == 5 || numDiamonds == 5)
                    return true;
            }
        }
        return false;
    }

    public bool IsFullHouse()
    {
        if(IsTwoPair() && IsThreeOfAKind())
            return true;

        return false;
    }

    public bool IsFourOfAKind()
    {
        for(int i = 0; i < combinedHand.Count - 3; i++)
        {
            if (combinedHand[i].Value == combinedHand[i + 1].Value && combinedHand[i].Value == combinedHand[i + 2].Value && combinedHand[i].Value == combinedHand[i + 3].Value)
                return true;
        }
        return false;
    }

    public bool IsStraightFlush()
    {
        if(IsFlush() && IsStraight())
        {
            if(numSpades == 5)
            {
                for(int i = 0; i < 5; i++)
                {
                    if (straightCards[i].Suit != Card.Suits.Spades)
                        return false;
                }
                return true;
            }

            else if(numClubs == 5)
            { 
                for(int i = 0; i < 5; i++)
                {
                    if (straightCards[i].Suit != Card.Suits.Clubs)
                        return false;
                }
                return true;
            }

            else if(numHearts == 5)
            {
                for(int i = 0; i < 5; i++)
                {
                    if (straightCards[i].Suit != Card.Suits.Hearts)
                        return false;
                }
                return true;
            }

            else if(numDiamonds == 5)
            {
                for(int i = 0; i < 5; i++)
                {
                    if (straightCards[i].Suit != Card.Suits.Diamonds)
                        return false;
                }
                return true;
            }
        }

        return false;
    }

    public bool IsRoyalFlush()
    {
        if(IsStraightFlush())
        {
            if (straightCards[4].Value == Card.Values.A)
                return true;
        }
       
        return false;
    }

    public void setScore(Player player)
    {
        combinedHand = new List<Card>(player.hand);
        combinedHand.AddRange(DealerHand);
        combinedHand.Sort((x, y) => x.Value.CompareTo(y.Value));

        if (IsRoyalFlush())
            player.Score = 10;
        else if (IsStraightFlush())
            player.Score = 9;
        else if (IsFourOfAKind())
            player.Score = 8;
        else if (IsFullHouse())
            player.Score = 7;
        else if (IsFlush())
            player.Score = 6;
        else if (IsStraight())
            player.Score = 5;
        else if (IsThreeOfAKind())
            player.Score = 4;
        else if (IsTwoPair())
            player.Score = 3;
        else if (IsPair())
            player.Score = 2;
        else
            player.Score = 1;
    }

    public void setScore(AIPlayer player)
    {
        combinedHand = new List<Card>(player.Hand);
        combinedHand.AddRange(DealerHand);
        combinedHand.Sort((x, y) => x.Value.CompareTo(y.Value));

        if (IsRoyalFlush())
            player.Score = 10;
        else if (IsStraightFlush())
            player.Score = 9;
        else if (IsFourOfAKind())
            player.Score = 8;
        else if (IsFullHouse())
            player.Score = 7;
        else if (IsFlush())
            player.Score = 6;
        else if (IsStraight())
            player.Score = 5;
        else if (IsThreeOfAKind())
            player.Score = 4;
        else if (IsTwoPair())
            player.Score = 3;
        else if (IsPair())
            player.Score = 2;
        else
            player.Score = 1;
    }

    public void GetDealerHand(List<Card> dealerHand) 
    {
        DealerHand = new List<Card>(dealerHand);
    }

    public void BidtoPot(int bid, Player player)
    {
        pot += bid;
        player.BiddingAmount -= bid;
    }

    public void BidtoPot(int bid, AIPlayer player)
    {
        pot += bid;
        player.BiddingAmount -= bid;
    }

    public void GiveWinnings(Player player)
    {
        player.BiddingAmount += pot;
        pot = 0;
    }

    public void GiveWinnings(AIPlayer player)
    {
        player.BiddingAmount += pot;
        pot = 0;
    }

    public void UpdateDealer(List <Card> cardList, Deck dealer)
    {
        if(round == 0)
        {
            for (int i = 0; i < 3; i++)
            {
                cardList.Add(dealer.DrawCard());
                cardList[i].Position = new Vector2(100 + (i * 200), 100);
            }
            round++;
        }

        else if(round == 1 || round == 2)
        {
            cardList.Add(dealer.DrawCard());

            if (round == 1)
                cardList[3].Position = new Vector2(100 + (3 * 200), 100);

            if (round == 2)
                cardList[4].Position = new Vector2(100 + (4 * 200), 100);

            round++;
        }
    }
}
