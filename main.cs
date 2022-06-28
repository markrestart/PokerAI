using System;
using System.Collections.Generic;

class Program
{
    public static void Main(string[] args)
    {
        var watch = new System.Diagnostics.Stopwatch();
        watch.Start();
        List<int> rawHandWins = new List<int>();
        rawHandWins.Add(0);
        rawHandWins.Add(0);
        rawHandWins.Add(0);
        rawHandWins.Add(0);
        rawHandWins.Add(0);
        rawHandWins.Add(0);
        rawHandWins.Add(0);
        rawHandWins.Add(0);
        rawHandWins.Add(0);
        rawHandWins.Add(0);

        //Setup the deck and gamestate
        List<Card> Deck = new List<Card>();
        foreach (Suit suit in (Suit[])Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank rank in (Rank[])Enum.GetValues(typeof(Rank)))
            {
                Deck.Add(new Card(suit, rank));
            }
        }
        GameState privateState = new GameState();
        //End Setup

        bool gameOver = false;
        while (!gameOver)
        {
            //Deal a new hand
            bool handOver = false;
            Deal(privateState, Deck);
            gameOver = !privateState.NewHand();
            //Loop through the hand until the gamestate returns false for the next move
            while (!handOver)
            {
                handOver = !privateState.NextMove();
            }
            //Check for the winner of the hand
            int bestRaw = 0;
            int bestRawIndex = 0;

            int bestHandIndex = 0;
            int index = 0;
            int bestHandValue = 0;
            foreach (Player p in privateState.Players)
            {
                var hand = new List<Card>();

                hand.AddRange(privateState.commonCards);
                hand.AddRange(p.hand);
                p.handValue = HandUtil.valueAllHands(hand);

                if(p.handValue > bestRaw || (p.handValue == bestRaw && HandUtil.valueAllHands(p.hand) > HandUtil.valueAllHands(privateState.Players[bestRawIndex].hand)))
                {
                    bestRaw = p.handValue;
                    bestRawIndex = index;
                }


                if (!p.hasFolded && (p.handValue > bestHandValue || (p.handValue == bestHandValue && HandUtil.valueAllHands(p.hand) > HandUtil.valueAllHands(privateState.Players[bestHandIndex].hand))))
                {
                    bestHandIndex = index;
                    bestHandValue = p.handValue;
                }
                index++;
            }
            Console.WriteLine("Player " + (bestHandIndex + 1) + " wins");
            privateState.Players[bestHandIndex].cash += privateState.currentPot;
            rawHandWins[bestRawIndex]++;

            //Side pots
            List<int> allIns = new List<int>();
            foreach((int,int) pot in privateState.sidePots)
            {
                allIns.Add(pot.Item1);

                int side_bestHandIndex = 0;
                int side_index = 0;
                int side_bestHandValue = 0;
                foreach (Player p in privateState.Players)
                {
                    var hand = new List<Card>();

                    hand.AddRange(privateState.commonCards);
                    hand.AddRange(p.hand);
                    p.handValue = HandUtil.valueAllHands(hand);
                    if (!p.hasFolded && !allIns.Contains(side_index) && (p.handValue > side_bestHandValue || (p.handValue == side_bestHandIndex && HandUtil.valueAllHands(p.hand) > HandUtil.valueAllHands(privateState.Players[side_bestHandIndex].hand))))
                    {
                        side_bestHandIndex = side_index;
                        side_bestHandValue = p.handValue;
                    }
                    side_index++;
                }
                Console.WriteLine("Player " + (side_bestHandIndex + 1) + " wins side pot");
                privateState.Players[side_bestHandIndex].cash += pot.Item2;
            }
            //End check winner

            //Temp debug code to fine missing money
            int totalCash = 0;
            foreach(Player p in privateState.Players)
            {
                totalCash += p.cash;
            }
            Console.WriteLine(totalCash);
            //end temp debug code
        }
        //Display everyone's final scores
        Console.WriteLine("==========FINAL SCORES==========");
        int i = 0;
        foreach(Player p in privateState.Players)
        {
            i++;
            Console.WriteLine("Player " + (i) + ": $" + p.cash.ToString() + " -" + p.buyIns + " buy ins");
            Console.WriteLine("Hand wins: " + rawHandWins[i-1]);
        }


        watch.Stop();
        Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
    }

    //Funtion to deal cards from the deck to the hands and common cards in the gamestate
    public static void Deal(GameState g, List<Card> Deck)
    {
        Deck.Shuffle();

        g.commonCards.Clear();
        for (int i = 0; i < 5; i++)
        {
            g.commonCards.Add(Deck[i]);
        }
        int pIndex = 0;
        foreach (Player p in g.Players)
        {
            var hand = new List<Card>();
            hand.Add(Deck[5 + pIndex * 2]);
            hand.Add(Deck[5 + pIndex * 2 + 1]);
            g.Players[pIndex].hand = hand;
            pIndex++;
        }
    }
}

