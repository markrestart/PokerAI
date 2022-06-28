public class Card
{
    public Suit suit;
    public Rank rank;

    public Card(Suit s, Rank r)
    {
        suit = s;
        rank = r;
    }

    public override string ToString()
    {
        return rank + " OF " + suit;
    }
}

public enum Suit
{
    SPADES,
    HEARTS,
    DIAMONDS,
    CLUBS
}

public enum Rank
{
    TWO,
    THREE,
    FOUR,
    FIVE,
    SIX,
    SEVEN,
    EIGHT,
    NINE,
    TEN,
    JACK,
    QUEEN,
    KING,
    ACE
}