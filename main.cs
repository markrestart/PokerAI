using System;
using System.Collections.Generic;

class Program {
  public static void Main (string[] args) {

    //Setup
    List<Card> Deck = new List<Card>();
    foreach (Suit suit in (Suit[]) Enum.GetValues(typeof(Suit)))
    {
      foreach (Rank rank in (Rank[]) Enum.GetValues(typeof(Rank)))
        {
          Deck.Add(new Card(suit, rank));
        }
    }
    GameState privateState = new GameState();
    //End Setup

    bool gameOver = false;
    while(!gameOver){
      bool handOver = false;
      Deal(privateState, Deck);
      gameOver = !privateState.NewHand();
      while(!handOver){
        handOver = !privateState.NextMove();
      }

      int bestHandIndex = 0;
      int index = 0;
      int bestHandValue = 0;
      foreach(Player p in privateState.Players){
        var hand = new List<Card>();
        
        hand.AddRange(privateState.commonCards);
        hand.AddRange(p.hand);
        p.handValue = HandUtil.valueAllHands(hand);
        if(!p.hasFolded && p.handValue > bestHandValue){
          bestHandIndex = index;
          bestHandValue = p.handValue;
        }
        index++;
      }
      Console.WriteLine("Player " + (bestHandIndex+1) + " wins");
    }
  }

  public static void Deal(GameState g, List<Card> Deck){
    Deck.Shuffle();
    
    for(int i = 0; i <5; i++){
      g.commonCards.Add(Deck[i]);
    }
    int pIndex = 0;
    foreach(Player p in g.Players){
      var hand = new List<Card>();
      hand.Add(Deck[5+pIndex*2]);
      hand.Add(Deck[5+pIndex*2+1]);
      g.Players[pIndex].hand = hand;
      pIndex++;
    }
  }
}

