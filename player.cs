using System.Collections.Generic;

public class Player{
  public List<Card> hand;
  public int handValue;
  public int cash;

  public Player(List<Card> h){
    hand = h;
  }

  public Player(int c){
    cash = c;
  }
}