public enum eCardSuit
{
    None = 0,
    Clubs = 1,
    Diamonds = 2,
    Hearts = 3,
    Spade = 4,
    Joker = 5
}

public enum ePlayerType
{
    Empty =0,
    Bot = 1,
    PlayerStartGame = 2,
    PlayerInRoom = 3,
	PlayerOutOfLimit=4
}

public enum eCombination
{
    Empty = 0,
    HighCard = 1,//High Card: A hand in which the three cards are not in sequence, not all the same suit and no two cards have the same rank. If two players share a common high card, the next highest card is used to determine the winner. The best high card hand would be an AKJ of different suits and the worst is 5-3-2.
    Pair = 2,//Pair (two of a kind): Two cards of the same rank. Between two pairs, the one with the higher value is the winner. If the pairs are of equal value then the kicker card determines the winner. The highest pair is A-A-K and the lowest is 2-2-3.
    Color = 3,//Color (Flush): Three cards of the same suit that are not in sequence. When comparing two colors, first compare the highest card. If these are equal, compare the second and if these are equal compare the lowest. Highest flush is A-K-J and the lowest flush is 5-3-2.
    Sequence = 4,//Sequence (Straight): Three consecutive cards not all in the same suit.
    PureSequence = 5,//Pure Sequence (Straight Flush): Three consecutive cards of the same suit.
    Trail = 6//Trail (three of a kind): Three cards of the same rank. Three aces are the highest and three 2’s are the lowest.
}


public enum eTable
{
    None,
    Standard,
    Free,
    NoLimit,
    Joker,
    Private,
    Muflis,
    Ak47,
    Royal,
    PotBlind,
    AndarBahar
}

