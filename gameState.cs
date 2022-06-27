using System;
using System.Collections.Generic;

public class GameState{
  public List<Card> commonCards = new List<Card>();
  public List<Player> Players = new List<Player>();

  public int activePlayerIndex = 0;
  public int ActivePlayerIndex{
    get{ return activePlayerIndex;}
    set{
      activePlayerIndex = value;
      if(activePlayerIndex >= Players.Count){activePlayerIndex = 0;}
    }
  }
  public int currentPot;
  public int currentBet;
  public int cardsRevealed;
  
  public int smallBlindIndex;
    public int SmallBlindIndex{
    get{ return smallBlindIndex;}
    set{
      smallBlindIndex = value;
      if(smallBlindIndex >= Players.Count){smallBlindIndex = 0;}
    }
  }
  public int blindAmount = 50;

  public GameState(){
    for(int i = 0; i<8; i++){
      Players.Add(new Player(10000));
    }
  }

  private int handNumber = 0;
  //Sets up betting for a new hand, returns false if game is over.
  public bool NewHand(){
    handNumber++;
    if(handNumber > 20){return false;}
    
    SmallBlindIndex++;
    currentPot = 0;
    cardsRevealed = 0;
    foreach(Player p in Players){
      p.NewHand();
    }
    Players[smallBlindIndex].Bet(blindAmount);
    Players[smallBlindIndex < Players.Count-1 ? smallBlindIndex+1 : 0].Bet(blindAmount * 2);
    currentPot = blindAmount * 3;
    ActivePlayerIndex = smallBlindIndex +2;

    return true;
  }

  //Requests an action from the active player AI, returns false if hand is over.
  public bool NextMove(){
    if(!Players[ActivePlayerIndex].hasFolded){
      string move = Players[ActivePlayerIndex].manager.ResolveMove(this.PublicState(ActivePlayerIndex));
      switch(move.Split(' ')[0]){
        case "call":
          if(currentBet > Players[ActivePlayerIndex].amountBet){
            Players[ActivePlayerIndex].Bet(currentBet - Players[ActivePlayerIndex].amountBet);
          }
          break;
        case "bet":
          int amount = Int32.Parse(move.Split(' ')[1]);
          foreach(Player p in Players){p.hasCalled = false;}
          Players[ActivePlayerIndex].Bet(currentBet - Players[ActivePlayerIndex].amountBet  + amount);
          break;
        case "fold":
          Players[ActivePlayerIndex].hasFolded = true;
          break;
      }
    }

    int numberNotFolded = 0;
    int numberCalled = 0;
    foreach(Player p in Players){
      if(!p.hasFolded){
        numberNotFolded++;
        numberCalled++;
      }else if(p.hasCalled){
        numberCalled++;
      }
    }

    if(numberNotFolded < 2){
      return false;
    }

    if(numberCalled == Players.Count){
      if(cardsRevealed == 0){
        cardsRevealed +=3;
      }else if(cardsRevealed == 3 || cardsRevealed == 4){
        cardsRevealed++;
      }else{
        return false;
      }
      ActivePlayerIndex = smallBlindIndex;
      return true;
    }else{
      ActivePlayerIndex++;
      return true;
    }
  }

  private GameData PublicState(int index){
    var cash = new List<int>();
    var bets = new List<int>();
    var folds = new List<bool>();

    foreach(Player p in Players){
      cash.Add(p.cash);
      bets.Add(p.amountBet);
      folds.Add(p.hasFolded);
    }
    
     return new GameData(index, Players[index].hand, commonCards.GetRange(0,cardsRevealed), currentPot, currentBet, SmallBlindIndex, blindAmount, cash.ToArray(), bets.ToArray(), folds.ToArray());
  }
}