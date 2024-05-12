using System;

[Serializable]
public struct CardData
{
    public eCardSuit suitCard;
    public int rankCard;
    public bool isClose;
    public int originalRankCard;
    public eCardSuit originalSuitCard;
    public CardData(int suit, int rank, bool close)
    {
        suitCard = (eCardSuit)suit;
        rankCard = rank;
        isClose = close;

        originalRankCard = rank;
        originalSuitCard = suitCard;

    }
}

[Serializable]
public struct DeckInfo
{
    public CardData[] DeckData;
}

