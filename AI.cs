using System.Collections.Generic;

public class AI
{
    /*Given an instance of GameData, ResolveMove should return a string with the desired action.
     * Options: call, check, bet, raise, fold
     * 
     * fold will remove the player from the current hand
     * 
     * call & check may be used interchangably, even in situations were the other is accurate.
     * This action will always match the player to the current bet, or go all in.
     * 
     * bet & raise may be used interchangably, even in situations were the other is accurate.
     * This action will always raise the current bet by the amount specified.
     * If no amount is specified or the amount is below the minimum, it will default to the minimum bet.
     * The minimum bet can be calculated as 4 times the blind amount.
     * If the amount specified is above the players cash, it will default to all in.
     */
    public virtual string ResolveMove(GameData state)
    {
        var hand = new List<Card>();
        hand.AddRange(state.hand);
        hand.AddRange(state.common);
        int num = HandUtil.valueAllHands(hand);

        if (num < 1000000)
        {
            if (state.common.Count > 0 && state.currentBet > state.playerBets[state.playerIndex])
            {
                return "fold";
            }
            else
            {
                return "call";
            }
        }
        else
        {
            int maxBet = num / 10000;
            if(state.currentBet > maxBet * 4)
            {
                return "fold";
            }
            else if(state.currentBet < maxBet)
            {
                return "bet " + (maxBet - state.currentBet);
            }
            else
            {
                return "call";
            }
        }
    }
}