using System;
using System.Collections.Generic;

class Program {
  public static void Main (string[] args) {
    List<Card> Deck = new List<Card>();
    List<Player> Players = new List<Player>();
    foreach (Suit suit in (Suit[]) Enum.GetValues(typeof(Suit)))
    {
      foreach (Rank rank in (Rank[]) Enum.GetValues(typeof(Rank)))
        {
          Deck.Add(new Card(suit, rank));
        }
    }
    Deck.Shuffle();

    for(int i = 0; i <5; i++){
       Console.WriteLine (Deck[i]); 
    }
    Console.WriteLine (""); 
    for(int i = 0; i<16; i+=2){
      Console.WriteLine ("Player " + (i/2+1) + " hand:");
      var hand = new List<Card>();
      hand.Add(Deck[5+i]);
      hand.Add(Deck[5+i+1]);
      Console.WriteLine (Deck[5+i]); 
      Console.WriteLine (Deck[5+i+1]); 
      Players.Add(new Player(hand));
    }

    int bestHandIndex = 0;
    int index = 0;
    int bestHandValue = 0;
    foreach(Player p in Players){
      var hand = new List<Card>();
      
      hand.AddRange(Deck.GetRange(0,5));
      hand.AddRange(p.hand);      
      p.handValue = HandUtil.valueAllHands(hand);
      if(p.handValue > bestHandValue){
        bestHandIndex = index;
        bestHandValue = p.handValue;
      }
      index++;
    }

    Console.WriteLine("Player " + (bestHandIndex+1) + " wins");
  }
}

