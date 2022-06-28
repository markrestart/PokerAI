using System;
using System.Collections.Generic;

public class GameState
{
    public List<Card> commonCards = new List<Card>();
    public List<Player> Players = new List<Player>();

    public int activePlayerIndex = 0;
    public int ActivePlayerIndex
    {
        get { return activePlayerIndex; }
        set
        {
            activePlayerIndex = value;
            if (activePlayerIndex >= Players.Count) { activePlayerIndex = 0; }
        }
    }
    public int currentPot;
    public List<(int, int)> sidePots;
    public int currentBet;
    public int cardsRevealed;

    public int smallBlindIndex;
    public int SmallBlindIndex
    {
        get { return smallBlindIndex; }
        set
        {
            smallBlindIndex = value;
            if (smallBlindIndex >= Players.Count) { smallBlindIndex = 0; }
        }
    }
    public int blindAmount = 50;

    public GameState()
    {
        Players.Add(new Player(new AI()));
        Players.Add(new Player(new AI()));
        Players.Add(new Player(new AI()));
        Players.Add(new Player(new AI()));
        Players.Add(new Player(new AI()));
        Players.Add(new Player(new AI()));
    }

    private int handNumber = 0;
    //Sets up betting for a new hand, returns false if game is over.
    public bool NewHand()
    {
        handNumber++;
        if (handNumber > 10000) { return false; }

        SmallBlindIndex++;
        currentPot = 0;
        sidePots = new List<(int, int)>();
        cardsRevealed = 0;
        foreach (Player p in Players)
        {
            p.NewHand();
        }
        Players[smallBlindIndex].Bet(blindAmount);
        Players[smallBlindIndex < Players.Count - 1 ? smallBlindIndex + 1 : 0].Bet(blindAmount * 2);
        currentPot = blindAmount * 3;
        currentBet = blindAmount * 2;
        ActivePlayerIndex = smallBlindIndex + 2;

        return true;
    }

    //Requests an action from the active player AI, returns false if hand is over.
    public bool NextMove()
    {
        if (!Players[ActivePlayerIndex].hasFolded && !Players[ActivePlayerIndex].isAllIn)
        {
            string move = Players[ActivePlayerIndex].manager.ResolveMove(this.PublicState(ActivePlayerIndex));
            switch (move.Split(' ')[0])
            {
                //Match the current bet.
                //If the current bet is above what can be afforded, go all in and make a side pot.
                case "check":
                case "call":
                    if (currentBet > Players[ActivePlayerIndex].amountBet) //True == call, False == check
                    {
                        if(currentBet - Players[ActivePlayerIndex].amountBet >= Players[ActivePlayerIndex].cash)
                        {
                            Players[ActivePlayerIndex].isAllIn = true;
                            Players[ActivePlayerIndex].hasCalled = true;
                            int shortBy = currentBet - Players[ActivePlayerIndex].amountBet + Players[ActivePlayerIndex].cash;
                            int sidePot = 0;
                            foreach(Player p in Players)
                            {
                                if (!p.isAllIn)
                                {
                                    sidePot += shortBy;
                                }
                            }
                            if (sidePots.Count == 0)
                            {
                                currentPot += Players[ActivePlayerIndex].cash - sidePot;
                                sidePots.Add((ActivePlayerIndex, sidePot));
                            }
                            else
                            {
                                sidePots[sidePots.Count - 1] = (sidePots[sidePots.Count - 1].Item1, sidePots[sidePots.Count - 1].Item2 + Players[ActivePlayerIndex].cash - sidePot);
                                sidePots.Add((ActivePlayerIndex, sidePot));
                            }
                            Players[ActivePlayerIndex].Bet(Players[ActivePlayerIndex].cash);
                            Players[ActivePlayerIndex].hasCalled = true;

                        }
                        else
                        {
                            if (sidePots.Count == 0)
                            {
                                currentPot += currentBet - Players[ActivePlayerIndex].amountBet;
                            }
                            else
                            {
                                sidePots[sidePots.Count - 1] = (sidePots[sidePots.Count - 1].Item1, sidePots[sidePots.Count - 1].Item2 + currentBet - Players[ActivePlayerIndex].amountBet);
                            }
                            Players[ActivePlayerIndex].Bet(currentBet - Players[ActivePlayerIndex].amountBet);
                            Players[ActivePlayerIndex].hasCalled = true;
                        }
                    }
                    else
                    {
                        Players[ActivePlayerIndex].hasCalled = true;
                    }
                    break;
                //Raise the current bet
                //If the raise is above what can be afforded, go all in and make a side pot
                case "raise":
                case "bet":
                    //Use amount given or default to small blind * 4 if no amount is provided
                    int amount = move.Split(' ').Length > 1 ? Int32.Parse(move.Split(' ')[1]) : blindAmount*4;
                    //Incease amount to minimum if amount given is below small blind * 4
                    if(amount < blindAmount * 4) { amount = blindAmount * 4; }

                    //If amount + current bet - player amount bet is greater than or equal to current cash, go all in
                    //Otherwise, make bet and continue
                    if (amount + currentBet - Players[ActivePlayerIndex].amountBet > Players[ActivePlayerIndex].cash)
                    {
                        Players[ActivePlayerIndex].isAllIn = true;
                        Players[ActivePlayerIndex].hasCalled = true;
                        int shortBy = currentBet - Players[ActivePlayerIndex].amountBet + Players[ActivePlayerIndex].cash;
                        int sidePot = 0;
                        foreach (Player p in Players)
                        {
                            if (!p.isAllIn)
                            {
                                sidePot += shortBy;
                            }
                        }
                        if (sidePots.Count == 0)
                        {
                            currentPot += Players[ActivePlayerIndex].cash - sidePot;
                            sidePots.Add((ActivePlayerIndex, sidePot));
                        }
                        else
                        {
                            sidePots[sidePots.Count - 1] = (sidePots[sidePots.Count - 1].Item1, sidePots[sidePots.Count - 1].Item2 + Players[ActivePlayerIndex].cash - sidePot);
                            sidePots.Add((ActivePlayerIndex, sidePot));
                        }
                        Players[ActivePlayerIndex].Bet(Players[ActivePlayerIndex].cash);
                        Players[ActivePlayerIndex].hasCalled = true;
                    }
                    else
                    {
                        foreach (Player p in Players) { p.hasCalled = false; }
                        if (sidePots.Count == 0)
                        {
                            currentPot += currentBet - Players[ActivePlayerIndex].amountBet + amount;
                        }
                        else
                        {
                            sidePots[sidePots.Count - 1] = (sidePots[sidePots.Count - 1].Item1, sidePots[sidePots.Count - 1].Item2 + currentBet - Players[ActivePlayerIndex].amountBet + amount);
                        }
                        Players[ActivePlayerIndex].Bet(currentBet - Players[ActivePlayerIndex].amountBet + amount);
                        currentBet += amount;
                        Players[ActivePlayerIndex].hasCalled = true;
                    }
                    break;
                case "fold":
                    Players[ActivePlayerIndex].hasFolded = true;
                    break;
            }
        }

        int numberNotFolded = 0;
        int numberCalled = 0;
        foreach (Player p in Players)
        {
            if (!p.hasFolded && !p.isAllIn)
            {
                numberNotFolded++;
                if (p.hasCalled)
                {
                numberCalled++;
                }
            }
            else
            {
                numberCalled++;
            }
        }

        if (numberNotFolded < 2)
        {
            return false;
        }

        if (numberCalled == Players.Count)
        {
            if (cardsRevealed == 0)
            {
                cardsRevealed += 3;
            }
            else if (cardsRevealed == 3 || cardsRevealed == 4)
            {
                cardsRevealed++;
            }
            else
            {
                return false;
            }
            ActivePlayerIndex = smallBlindIndex;
            return true;
        }
        else
        {
            ActivePlayerIndex++;
            return true;
        }
    }

    //Creates a game data object from the current state displaying only the information availabe to the index player
    private GameData PublicState(int index)
    {
        var cash = new List<int>();
        var bets = new List<int>();
        var folds = new List<bool>();

        foreach (Player p in Players)
        {
            cash.Add(p.cash);
            bets.Add(p.amountBet);
            folds.Add(p.hasFolded);
        }

        return new GameData(index, Players[index].hand, commonCards.GetRange(0, cardsRevealed), currentPot, sidePots, currentBet, SmallBlindIndex, blindAmount, cash.ToArray(), bets.ToArray(), folds.ToArray());
    }
}