using System.Collections.Generic;

public class AI{
  public virtual string ResolveMove(GameData state){
    var hand = new List<Card>();
    hand.AddRange(state.hand);
    hand.AddRange(state.common);
    int num = HandUtil.valueAllHands(hand);

    if(num < 4000000){
      if(state.currentBet > state.playerBets[state.playerIndex]){
        return "fold";
      }else{
        return "call";
      }
    }else{
      return "bet 20";
    }
  }
}