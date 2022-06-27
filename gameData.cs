using System.Collections.Generic;

public class GameData{
  public int playerIndex;      //Your place in the player arrays
  public List<Card> hand;   //Your hand
  public List<Card> common; //The common cards that have been revealed
  public int currentPot;       //The amount currently in the pot
  public int currentBet;       //The total individual current bet for the hand
  public int smallBlindIndex;  //The index of the player with the small blind
  public int blindAmount;      //The small blind for the hand, big blind is double

  public int[] playerCash;     //Array of all players curent cash
  public int[] playerBets;     //Array of all players currently paid bets
  public bool[] playersFolded; //Array of all players fold status

  public GameData(int i, List<Card> h, List<Card> c, int pot, int bet, int iBlind, int aBlind, int[] cash, int[] bets, bool[] fold){
  int playerIndex = i;;      
  List<Card> hand = h;   
  List<Card> common = c; 
  int currentPot = pot;       
  int currentBet = bet;       
  int smallBlindIndex = iBlind;  
  int blindAmount = aBlind;      

  int[] playerCash = cash;     
  int[] playerBets = bets;     
  bool[] playersFolded = fold; 
  }
}