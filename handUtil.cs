using System.Collections.Generic;

public static class HandUtil{
  public static readonly int STRAIGHT_FLUSH = 8000000;// + valueHighCard()
  public static readonly int FOUR_OF_A_KIND = 7000000;// + Quads Card Rank
  public static readonly int FULL_HOUSE     = 6000000;// + SET card rank
  public static readonly int FLUSH          = 5000000;// + valueHighCard()
  public static readonly int STRAIGHT       = 4000000;// + valueHighCard()
  public static readonly int SET            = 3000000;// + Set card value
  public static readonly int TWO_PAIRS      = 2000000;// + High2*14^4+ Low2*14^2 + card
  public static readonly int ONE_PAIR       = 1000000;// + high*14^2 + high2*14^1 + low


  public static int valueAllHands( List<Card> h){

    if(h.Count <= 5){
      return valueHand(h);
    }
    
    int highValue = 0;
    for(int i = 0; i <=h.Count-2; i++){
      for(int j = i+1; j <=h.Count-1; j++){
        if(j != i){
          var subH = h.ConvertAll(c => new Card(c.suit,c.rank));
          subH.RemoveAt(j);
          subH.RemoveAt(i);        
          int subValue = valueHand(subH);
          if(subValue > highValue){
            highValue = subValue;
          }
        }
      }
    }
    return highValue;
  }
  
  public static int valueHand( List<Card> h )
   {
      if ( isFlush(h) && isStraight(h) )
         return valueStraightFlush(h);
      else if ( is4s(h) )
         return valueFourOfAKind(h);
      else if ( isFullHouse(h) )
         return valueFullHouse(h);
      else if ( isFlush(h) )
         return valueFlush(h);
      else if ( isStraight(h) )
         return valueStraight(h);
      else if ( is3s(h) )
         return valueSet(h);
      else if ( is22s(h) )
         return valueTwoPairs(h);
      else if ( is2s(h) )
         return valueOnePair(h);
      else
         return valueHighCard(h);
   }


     /* -----------------------------------------------------
      valueFlush(): return value of a Flush hand

            value = FLUSH + valueHighCard()
      ----------------------------------------------------- */
   public static int valueStraightFlush( List<Card> h )
   {
      return STRAIGHT_FLUSH + valueHighCard(h);
   }

   /* -----------------------------------------------------
      valueFlush(): return value of a Flush hand

            value = FLUSH + valueHighCard()
      ----------------------------------------------------- */
   public static int valueFlush( List<Card> h )
   {
      return FLUSH + valueHighCard(h);
   }

   /* -----------------------------------------------------
      valueStraight(): return value of a Straight hand

            value = STRAIGHT + valueHighCard()
      ----------------------------------------------------- */
   public static int valueStraight( List<Card> h )
   {
      return STRAIGHT + valueHighCard(h);
   }

   /* ---------------------------------------------------------
      valueFourOfAKind(): return value of a 4 of a kind hand

            value = FOUR_OF_A_KIND + 4sCardRank

      Trick: card h[2] is always a card that is part of 
             the 4-of-a-kind hand
	     There is ONLY ONE hand with a quads of a given rank.
      --------------------------------------------------------- */
   public static int valueFourOfAKind( List<Card> h )
   {
      sortByRank(h);

      return FOUR_OF_A_KIND + (int)h[2].rank;
   }

   /* -----------------------------------------------------------
      valueFullHouse(): return value of a Full House hand

            value = FULL_HOUSE + SetCardRank

      Trick: card h[2] is always a card that is part of
             the 3-of-a-kind in the full house hand
	     There is ONLY ONE hand with a FH of a given set.
      ----------------------------------------------------------- */
   public static int valueFullHouse( List<Card> h )
   {
      sortByRank(h);

      return FULL_HOUSE + (int)h[2].rank;
   }

   /* ---------------------------------------------------------------
      valueSet(): return value of a Set hand

            value = SET + SetCardRank

      Trick: card h[2] is always a card that is part of the set hand
	     There is ONLY ONE hand with a set of a given rank.
      --------------------------------------------------------------- */
   public static int valueSet( List<Card> h )
   {
      sortByRank(h);

      return SET + (int)h[2].rank;
   }

   /* -----------------------------------------------------
      valueTwoPairs(): return value of a Two-Pairs hand

            value = TWO_PAIRS
                   + 14*14*HighPairCard
                   + 14*LowPairCard
                   + UnmatchedCard
      ----------------------------------------------------- */
   public static int valueTwoPairs( List<Card> h )
   {
      int val = 0;

      sortByRank(h);

      if ( h[0].rank == h[1].rank &&
           h[2].rank == h[3].rank )
         val = 14*14*(int)h[2].rank + 14*(int)h[0].rank + (int)h[4].rank;
      else if ( h[0].rank == h[1].rank &&
                h[3].rank == h[4].rank )
         val = 14*14*(int)h[3].rank + 14*(int)h[0].rank + (int)h[2].rank;
      else 
         val = 14*14*(int)h[3].rank + 14*(int)h[1].rank + (int)h[0].rank;

      return TWO_PAIRS + val;
   }

   /* -----------------------------------------------------
      valueOnePair(): return value of a One-Pair hand

            value = ONE_PAIR 
                   + 14^3*PairCard
                   + 14^2*HighestCard
                   + 14*MiddleCard
                   + LowestCard
      ----------------------------------------------------- */
   public static int valueOnePair( List<Card> h )
   {
      int val = 0;

      sortByRank(h);

      if ( h[0].rank == h[1].rank )
         val = 14*14*14*(int)h[0].rank +  
                + (int)h[2].rank + 14*(int)h[3].rank + 14*14*(int)h[4].rank;
      else if ( h[1].rank == h[2].rank )
         val = 14*14*14*(int)h[1].rank +  
                + (int)h[0].rank + 14*(int)h[3].rank + 14*14*(int)h[4].rank;
      else if ( h[2].rank == h[3].rank )
         val = 14*14*14*(int)h[2].rank +  
                + (int)h[0].rank + 14*(int)h[1].rank + 14*14*(int)h[4].rank;
      else
         val = 14*14*14*(int)h[3].rank +  
                + (int)h[0].rank + 14*(int)h[1].rank + 14*14*(int)h[2].rank;

      return ONE_PAIR + val;
   }

   /* -----------------------------------------------------
      valueHighCard(): return value of a high card hand

            value =  14^4*highestCard 
                   + 14^3*2ndHighestCard
                   + 14^2*3rdHighestCard
                   + 14^1*4thHighestCard
                   + LowestCard
      ----------------------------------------------------- */
   public static int valueHighCard( List<Card> h )
   {
      int val;

      sortByRank(h);

      val = (int)h[0].rank + 14* (int)h[1].rank + 14*14* (int)h[2].rank
            + 14*14*14* (int)h[3].rank + 14*14*14*14* (int)h[4].rank;

      return val;
   }


   /***********************************************************
     Methods used to determine a certain Poker hand
    ***********************************************************/


   /* ---------------------------------------------
      is4s(): true if h has 4 of a kind
              false otherwise
      --------------------------------------------- */
   public static bool is4s( List<Card> h )
   {
    var counts = new Dictionary<Rank, int>();
     foreach(Card c in h){
       if(counts.ContainsKey(c.rank)){
         counts[c.rank]++;
       }else{
         counts.Add(c.rank, 1);
       }
     }
     foreach(KeyValuePair<Rank, int> entry in counts){
       if(entry.Value >= 4){
         return true;
       }
     }

    return false;
   }


   /* ----------------------------------------------------
      isFullHouse(): true if h has Full House
                     false otherwise
      ---------------------------------------------------- */
   public static bool isFullHouse( List<Card> h )
   {
      bool a1, a2;

      if ( h.Count != 5 )
         return(false);

      sortByRank(h);

      a1 = h[0].rank == h[1].rank &&  //  x x x y y
           h[1].rank == h[2].rank &&
           h[3].rank == h[4].rank;

      a2 = h[0].rank == h[1].rank &&  //  x x y y y
           h[2].rank == h[3].rank &&
           h[3].rank == h[4].rank;

      return( a1 || a2 );
   }



   /* ----------------------------------------------------
      is3s(): true if h has 3 of a kind
              false otherwise

      **** Note: use is3s() ONLY if you know the hand
                 does not have 4 of a kind 
      ---------------------------------------------------- */
   public static bool is3s( List<Card> h )
   {
    var counts = new Dictionary<Rank, int>();
     foreach(Card c in h){
       if(counts.ContainsKey(c.rank)){
         counts[c.rank]++;
       }else{
         counts.Add(c.rank, 1);
       }
     }
     foreach(KeyValuePair<Rank, int> entry in counts){
       if(entry.Value >= 3){
         return true;
       }
     }

    return false;
   }

   /* -----------------------------------------------------
      is22s(): true if h has 2 pairs
               false otherwise

      **** Note: use is22s() ONLY if you know the hand
                 does not have 3 of a kind or better
      ----------------------------------------------------- */
   public static bool is22s( List<Card> h )
   {
      bool a1, a2, a3;

      if ( h.Count != 5 )
         return(false);

      if ( is4s(h) || isFullHouse(h) || is3s(h) )
         return(false);        // The hand is not 2 pairs (but better)

      sortByRank(h);

      a1 = h[0].rank == h[1].rank &&
           h[2].rank == h[3].rank ;

      a2 = h[0].rank == h[1].rank &&
           h[3].rank == h[4].rank ;

      a3 = h[1].rank == h[2].rank &&
           h[3].rank == h[4].rank ;

      return( a1 || a2 || a3 );
   }


   /* -----------------------------------------------------
      is2s(): true if h has one pair
              false otherwise

      **** Note: use is22s() ONLY if you know the hand
                 does not have 2 pairs or better
      ----------------------------------------------------- */
   public static bool is2s( List<Card> h )
   {
    var counts = new Dictionary<Rank, int>();
     foreach(Card c in h){
       if(counts.ContainsKey(c.rank)){
         counts[c.rank]++;
       }else{
         counts.Add(c.rank, 1);
       }
     }
     foreach(KeyValuePair<Rank, int> entry in counts){
       if(entry.Value >= 2){
         return true;
       }
     }

    return false;
   }


   /* ---------------------------------------------
      isFlush(): true if h has a flush
                 false otherwise
      --------------------------------------------- */
   public static bool isFlush( List<Card> h )
   {
      if ( h.Count != 5 )
         return(false);

      sortBySuit(h);

      return( h[0].suit == h[4].suit );   // All cards has same suit
   }


   /* ---------------------------------------------
      isStraight(): true if h is a Straight
                    false otherwise
      --------------------------------------------- */
   public static bool isStraight( List<Card> h )
   {
      int i, testRank;

      if ( h.Count != 5 )
        return(false);

      sortByRank(h);

      /* ===========================
         Check if hand has an Ace
         =========================== */
      if ( h[4].rank == Rank.ACE )
      {
         /* =================================
            Check straight using an Ace
            ================================= */
         bool a = h[0].rank == Rank.TWO && h[1].rank == Rank.THREE &&
                     h[2].rank == Rank.FOUR && h[3].rank == Rank.FIVE ;
         bool b = h[0].rank == Rank.TEN && h[1].rank == Rank.JACK &&
                     h[2].rank == Rank.QUEEN && h[3].rank == Rank.KING ;

         return ( a || b );
      }
      else
      {
         /* ===========================================
            General case: check for increasing values
            =========================================== */
         testRank = (int)h[0].rank + 1;

         for ( i = 1; i < 5; i++ )
         {
            if ( (int)h[i].rank != testRank )
               return(false);        // Straight failed...

            testRank++;
         }

         return(true);        // Straight found !
      }
   }

   /* ===========================================================
      Helper methods
      =========================================================== */

   /* ---------------------------------------------
      Sort hand by rank:

          smallest ranked card first .... 

      (Finding a straight is eaiser that way)
      --------------------------------------------- */
   public static void sortByRank( List<Card> h )
   {
      int i, j, min_j;

      /* ---------------------------------------------------
         The selection sort algorithm
         --------------------------------------------------- */
      for ( i = 0 ; i < h.Count ; i ++ )
      {
         /* ---------------------------------------------------
            Find array element with min. value among
            h[i], h[i+1], ..., h[n-1]
            --------------------------------------------------- */
         min_j = i;   // Assume elem i (h[i]) is the minimum
 
         for ( j = i+1 ; j < h.Count ; j++ )
         {
            if ( h[j].rank < h[min_j].rank )
            {
               min_j = j;    // We found a smaller minimum, update min_j     
            }
         }
 
         /* ---------------------------------------------------
            Swap a[i] and a[min_j]
            --------------------------------------------------- */
         Card help = h[i];
         h[i] = h[min_j];
         h[min_j] = help;
      }
   }

   /* ---------------------------------------------
      Sort hand by suit:

          smallest suit card first .... 

      (Finding a flush is eaiser that way)
      --------------------------------------------- */
   public static void sortBySuit( List<Card> h )
   {
      int i, j, min_j;

      /* ---------------------------------------------------
         The selection sort algorithm
         --------------------------------------------------- */
      for ( i = 0 ; i < h.Count ; i ++ )
      {
         /* ---------------------------------------------------
            Find array element with min. value among
            h[i], h[i+1], ..., h[n-1]
            --------------------------------------------------- */
         min_j = i;   // Assume elem i (h[i]) is the minimum
 
         for ( j = i+1 ; j < h.Count ; j++ )
         {
            if ( h[j].suit < h[min_j].suit )
            {
               min_j = j;    // We found a smaller minimum, update min_j     
            }
         }
 
         /* ---------------------------------------------------
            Swap a[i] and a[min_j]
            --------------------------------------------------- */
         Card help = h[i];
         h[i] = h[min_j];
         h[min_j] = help;
      }
   }
}