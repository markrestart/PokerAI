using System.Collections.Generic;

public class Player{
  public List<Card> hand;
  public int handValue;
  public int cash;
  public int amountBet;
  public bool hasFolded;
  public bool hasCalled;
  public AI manager;

  public Player(List<Card> h){
    hand = h;
  }

  public Player(int c){
    cash = c;
    manager = new AI();
  }

  public void Bet(int amount){
    amountBet +=amount;
    cash -=amount;
    hasCalled = true;
  }

  public void NewHand(){
    amountBet = 0;
    hasFolded = false;
    hasCalled = false;
  }
}