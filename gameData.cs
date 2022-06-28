using System.Collections.Generic;

public class GameData
{
    public int playerIndex;             //Your place in the player arrays
    public List<Card> hand;             //Your hand
    public List<Card> common;           //The common cards that have been revealed
    public int currentPot;              //The amount currently in the pot
    public List<(int, int)> sidePots;   //The side pots made, Item1 is the player index not eleigable for the pot, Item 2 is the pot amount
    public int currentBet;              //The total individual current bet for the hand
    public int smallBlindIndex;         //The index of the player with the small blind
    public int blindAmount;             //The small blind for the hand, big blind is double

    public int[] playerCash;            //Array of all players curent cash
    public int[] playerBets;            //Array of all players currently paid bets
    public bool[] playersFolded;        //Array of all players fold status

    public GameData(int i, List<Card> h, List<Card> c, int pot, List<(int,int)> side, int bet, int iBlind, int aBlind, int[] cash, int[] bets, bool[] fold)
    {
        playerIndex = i; ;
        hand = h;
        common = c;
        currentPot = pot;
        sidePots = side;
        currentBet = bet;
        smallBlindIndex = iBlind;
        blindAmount = aBlind;

        playerCash = cash;
        playerBets = bets;
        playersFolded = fold;
    }
}