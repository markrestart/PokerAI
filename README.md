# PokerAI
The base engine for an AI poker league.

## How to play
The AI Poker League is played by building an AI to compete against other AI. To start building your own AI, create a child class of the AI class. This class must contain a ResolveMove function that takes in a GameData object and returns a string.

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
     
To test your AI, you must modify the constructor of the GameState class. The constructor contains multiple lines to add players to a game that look like this.
     
     Players.Add(new Player(new AI()));
     
Modify one or more of the lines to replace the new AI() with your AI subclass.
Information on entering the AI Poker League coming soon.
