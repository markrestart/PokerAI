using System.Collections.Generic;

public class Player
{
    public List<Card> hand;
    public int handValue;
    public int cash;
    public int buyIns;
    public int amountBet;
    public bool hasFolded;
    public bool isAllIn;
    public bool hasCalled;
    public AI manager;

    public Player(AI ai)
    {
        cash = 10000;
        buyIns = 1;
        manager = ai;
    }

    public void Bet(int amount)
    {
        amountBet += amount;
        cash -= amount;
        hasCalled = true;
    }

    public void NewHand()
    {
        amountBet = 0;
        hasFolded = false;
        hasCalled = false;
        isAllIn = false;

        if (cash <= 0)
        {
            cash += 10000;
            buyIns++;
        }
    }
}