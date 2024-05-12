using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class CardCombination
{
    public static eCombination GetCombinationFromCard(CardData[] cardsPlayer)
    {
        eCombination currentCombination = eCombination.HighCard;

        var joker = cardsPlayer.Any(cardData => cardData.suitCard == eCardSuit.Joker);
        if (!joker)
        {
            if (cardsPlayer[0].rankCard == cardsPlayer[1].rankCard || cardsPlayer[1].rankCard == cardsPlayer[2].rankCard || cardsPlayer[0].rankCard == cardsPlayer[2].rankCard)
                currentCombination = eCombination.Pair;
            if (cardsPlayer[0].rankCard == cardsPlayer[1].rankCard && cardsPlayer[1].rankCard == cardsPlayer[2].rankCard && cardsPlayer[0].rankCard == cardsPlayer[2].rankCard)
                currentCombination = eCombination.Trail;
            if (cardsPlayer[0].suitCard == cardsPlayer[1].suitCard && cardsPlayer[1].suitCard == cardsPlayer[2].suitCard && cardsPlayer[0].suitCard == cardsPlayer[2].suitCard)
                currentCombination = eCombination.Color;
            if (cardsPlayer[0].rankCard == cardsPlayer[1].rankCard + 1 && cardsPlayer[0].rankCard == cardsPlayer[2].rankCard + 2)
                currentCombination = eCombination.Sequence;
            if (cardsPlayer[0].rankCard == cardsPlayer[1].rankCard - 1 && cardsPlayer[0].rankCard == cardsPlayer[2].rankCard - 2)
                currentCombination = eCombination.Sequence;
            if(currentCombination!= eCombination.Sequence)
            {
                if (cardsPlayer[0].rankCard == 14 && cardsPlayer[1].rankCard == 3 && cardsPlayer[2].rankCard == 2)
                    currentCombination = eCombination.Sequence;
                if (cardsPlayer[0].rankCard == 2 && cardsPlayer[1].rankCard == 3 && cardsPlayer[2].rankCard == 14)
                    currentCombination = eCombination.Sequence;

            }
            if (cardsPlayer[0].rankCard == cardsPlayer[1].rankCard - 1 && cardsPlayer[0].rankCard == cardsPlayer[2].rankCard - 2 &&
                cardsPlayer[0].suitCard == cardsPlayer[1].suitCard && cardsPlayer[1].suitCard == cardsPlayer[2].suitCard && cardsPlayer[0].suitCard == cardsPlayer[2].suitCard)
                currentCombination = eCombination.PureSequence;
            if (cardsPlayer[0].rankCard == cardsPlayer[1].rankCard + 1 && cardsPlayer[0].rankCard == cardsPlayer[2].rankCard + 2 &&
               cardsPlayer[0].suitCard == cardsPlayer[1].suitCard && cardsPlayer[1].suitCard == cardsPlayer[2].suitCard && cardsPlayer[0].suitCard == cardsPlayer[2].suitCard)
                currentCombination = eCombination.PureSequence;

            if (currentCombination != eCombination.PureSequence)
            {
                if (cardsPlayer[0].rankCard == 14 && cardsPlayer[1].rankCard == 3 && cardsPlayer[2].rankCard == 2  && cardsPlayer[0].suitCard == cardsPlayer[1].suitCard && cardsPlayer[1].suitCard == cardsPlayer[2].suitCard && cardsPlayer[0].suitCard == cardsPlayer[2].suitCard)
                    currentCombination = eCombination.PureSequence;
                if (cardsPlayer[0].rankCard == 2 && cardsPlayer[1].rankCard == 3 && cardsPlayer[2].rankCard == 14 && cardsPlayer[0].suitCard == cardsPlayer[1].suitCard && cardsPlayer[1].suitCard == cardsPlayer[2].suitCard && cardsPlayer[0].suitCard == cardsPlayer[2].suitCard)
                    currentCombination = eCombination.PureSequence;

            }
        }
        else
        {           
            currentCombination = eCombination.Pair;
            if (cardsPlayer[0].rankCard == cardsPlayer[1].rankCard)
                currentCombination = eCombination.Trail;
            if (cardsPlayer[0].suitCard == cardsPlayer[1].suitCard)
                currentCombination = eCombination.Color;
            if (cardsPlayer[0].rankCard == cardsPlayer[1].rankCard + 1)
                currentCombination = eCombination.Sequence;
            if (cardsPlayer[0].rankCard == cardsPlayer[1].rankCard - 1)
                currentCombination = eCombination.Sequence;
            if (currentCombination != eCombination.Sequence)
            {
                if (cardsPlayer[0].rankCard == 14 && cardsPlayer[1].rankCard == 3)
                    currentCombination = eCombination.Sequence;
                if (cardsPlayer[0].rankCard == 3 && cardsPlayer[1].rankCard == 14)
                    currentCombination = eCombination.Sequence;

            }
            if (cardsPlayer[0].rankCard == cardsPlayer[1].rankCard - 1  &&  cardsPlayer[0].suitCard == cardsPlayer[1].suitCard)
                currentCombination = eCombination.PureSequence;
            if (cardsPlayer[0].rankCard == cardsPlayer[1].rankCard + 1 &&  cardsPlayer[0].suitCard == cardsPlayer[1].suitCard )
                currentCombination = eCombination.PureSequence;

            if (currentCombination != eCombination.PureSequence)
            {
                if (cardsPlayer[0].rankCard == 14 && cardsPlayer[1].rankCard == 3 && cardsPlayer[0].suitCard == cardsPlayer[1].suitCard)
                    currentCombination = eCombination.PureSequence;
                if (cardsPlayer[0].rankCard == 3 && cardsPlayer[1].rankCard == 14 && cardsPlayer[0].suitCard == cardsPlayer[1].suitCard)
                    currentCombination = eCombination.PureSequence;

            }
        }
        return currentCombination;
    }

    public static int GetHighCard(CardData[] cards)
    {
        int numCard = 0;
        for (int i=0;i< cards.Length;i++)
        {

            if (cards[i].rankCard > numCard)
               numCard = cards[i].rankCard;

            
        }
        return numCard;
    }
    public static int GetTotalCard(CardData[] cards)
    {
        int numCard = 0;
        for (int i = 0; i < cards.Length; i++)
        {

            numCard += cards[i].rankCard;
        }
        return numCard;
    }
    public static PlayerManagerPun decideWinner(PlayerManagerPun[] players)
    {
        eCombination[] playerInGameCombination = new eCombination[players.Length];
        int[] playerInGameHigh = new int[players.Length];
        int[] playerInGameTotal = new int[players.Length];

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].playerData.playerType != ePlayerType.Empty  && players[i].playerData.currentCombination != eCombination.Empty && !players[i].playerData.IsPacked)
            {
                playerInGameCombination[i] = GetCombinationFromCard(players[i].GetCurrentCards());
                playerInGameHigh[i] = GetHighCard(players[i].GetCurrentCards());
                playerInGameTotal[i] = GetTotalCard(players[i].GetCurrentCards());
            }
        }

        int maxCombinaion = 0;
        PlayerManagerPun numWinPlayer = null;
        bool whichPlayerWin = false;

        for (int i = 0; i < players.Length; i++)
            if ((int)playerInGameCombination[i] > maxCombinaion)
                maxCombinaion = (int)playerInGameCombination[i];

        List<PlayerManagerPun> finalist = new List<PlayerManagerPun>();
        for (int i = 0; i < players.Length; i++)
            if ((int)playerInGameCombination[i] == maxCombinaion)
                finalist.Add(players[i]);

        if (finalist.Count == 2)
        {
            switch (maxCombinaion)
            {
               
                case 2:
                    //Collections.sort(finalist, new sortByPairCard());
                    whichPlayerWin = CompareCards(finalist[0].GetCurrentCards(), finalist[1].GetCurrentCards(), eTable.None);

                    if (whichPlayerWin)
                    {
                        numWinPlayer = finalist[0];
                    }
                    else
                    {
                        numWinPlayer = finalist[1];
                    }
                    break;
                default:
                    //Collections.sort(finalist, new sortByHighCard());
                    whichPlayerWin = CompareCards(finalist[0].GetCurrentCards(), finalist[1].GetCurrentCards(), eTable.None);

                    if (whichPlayerWin)
                    {
                        numWinPlayer = finalist[0];
                    }
                    else
                    {
                        numWinPlayer = finalist[1];
                    }
                    break;
            }
        }
        else if (finalist.Count == 3)
        {
            switch (maxCombinaion)
            {
                case 2:
                    //Collections.sort(finalist, new sortByPairCard());
                    whichPlayerWin = CompareCards(finalist[0].GetCurrentCards(), finalist[1].GetCurrentCards(), eTable.None);

                    if (whichPlayerWin)
                    {
                        numWinPlayer = finalist[0];

                        whichPlayerWin = CompareCards(finalist[0].GetCurrentCards(), finalist[2].GetCurrentCards(), eTable.None);

                        if (whichPlayerWin)
                        {
                            numWinPlayer = finalist[0];

                        }
                        else
                        {
                            numWinPlayer = finalist[2];
                        }
                    }
                    else
                    {
                        numWinPlayer = finalist[1];

                        whichPlayerWin = CompareCards(finalist[1].GetCurrentCards(), finalist[2].GetCurrentCards(), eTable.None);

                        if (whichPlayerWin)
                        {
                            numWinPlayer = finalist[1];

                        }
                        else
                        {
                            numWinPlayer = finalist[2];
                        }
                    }
                    break;
               
                default:
                    //Collections.sort(finalist, new sortByHighCard());
                    whichPlayerWin = CompareCards(finalist[0].GetCurrentCards(), finalist[1].GetCurrentCards(), eTable.None);

                    if (whichPlayerWin)
                    {
                        numWinPlayer = finalist[0];

                        whichPlayerWin = CompareCards(finalist[0].GetCurrentCards(), finalist[2].GetCurrentCards(), eTable.None);

                        if (whichPlayerWin)
                        {
                            numWinPlayer = finalist[0];

                        }
                        else
                        {
                            numWinPlayer = finalist[2];
                        }
                    }
                    else
                    {
                        numWinPlayer = finalist[1];

                        whichPlayerWin = CompareCards(finalist[1].GetCurrentCards(), finalist[2].GetCurrentCards(), eTable.None);

                        if (whichPlayerWin)
                        {
                            numWinPlayer = finalist[1];

                        }
                        else
                        {
                            numWinPlayer = finalist[2];
                        }
                    }
                    break;
            }
        }
        else if (finalist.Count == 4)
        {
            switch (maxCombinaion)
            {
                case 2:
                    //Collections.sort(finalist, new sortByPairCard());
                    whichPlayerWin = CompareCards(finalist[0].GetCurrentCards(), finalist[1].GetCurrentCards(), eTable.None);

                    if (whichPlayerWin)
                    {
                        numWinPlayer = finalist[0];

                        whichPlayerWin = CompareCards(finalist[0].GetCurrentCards(), finalist[2].GetCurrentCards(), eTable.None);

                        if (whichPlayerWin)
                        {
                            numWinPlayer = finalist[0];

                            whichPlayerWin = CompareCards(finalist[0].GetCurrentCards(), finalist[3].GetCurrentCards(), eTable.None);

                            if (whichPlayerWin)
                            {
                                numWinPlayer = finalist[0];

                            }
                            else
                            {
                                numWinPlayer = finalist[3];
                            }
                        }
                        else
                        {
                            numWinPlayer = finalist[2];

                            whichPlayerWin = CompareCards(finalist[2].GetCurrentCards(), finalist[3].GetCurrentCards(), eTable.None);

                            if (whichPlayerWin)
                            {
                                numWinPlayer = finalist[2];

                            }
                            else
                            {
                                numWinPlayer = finalist[3];
                            }
                        }
                    }
                    else
                    {
                        numWinPlayer = finalist[1];

                        whichPlayerWin = CompareCards(finalist[1].GetCurrentCards(), finalist[2].GetCurrentCards(), eTable.None);

                        if (whichPlayerWin)
                        {
                            numWinPlayer = finalist[1];

                            whichPlayerWin = CompareCards(finalist[1].GetCurrentCards(), finalist[3].GetCurrentCards(), eTable.None);

                            if (whichPlayerWin)
                            {
                                numWinPlayer = finalist[1];

                            }
                            else
                            {
                                numWinPlayer = finalist[3];
                            }

                        }
                        else
                        {
                            numWinPlayer = finalist[2];

                            whichPlayerWin = CompareCards(finalist[2].GetCurrentCards(), finalist[3].GetCurrentCards(), eTable.None);

                            if (whichPlayerWin)
                            {
                                numWinPlayer = finalist[2];

                            }
                            else
                            {
                                numWinPlayer = finalist[3];
                            }
                        }
                    }
                    break;

                default:
                    //Collections.sort(finalist, new sortByHighCard());
                    whichPlayerWin = CompareCards(finalist[0].GetCurrentCards(), finalist[1].GetCurrentCards(), eTable.None);

                    if (whichPlayerWin)
                    {
                        numWinPlayer = finalist[0];

                        whichPlayerWin = CompareCards(finalist[0].GetCurrentCards(), finalist[2].GetCurrentCards(), eTable.None);

                        if (whichPlayerWin)
                        {
                            numWinPlayer = finalist[0];

                            whichPlayerWin = CompareCards(finalist[0].GetCurrentCards(), finalist[3].GetCurrentCards(), eTable.None);

                            if (whichPlayerWin)
                            {
                                numWinPlayer = finalist[0];

                            }
                            else
                            {
                                numWinPlayer = finalist[3];
                            }
                        }
                        else
                        {
                            numWinPlayer = finalist[2];

                            whichPlayerWin = CompareCards(finalist[2].GetCurrentCards(), finalist[3].GetCurrentCards(), eTable.None);

                            if (whichPlayerWin)
                            {
                                numWinPlayer = finalist[2];

                            }
                            else
                            {
                                numWinPlayer = finalist[3];
                            }
                        }
                    }
                    else
                    {
                        numWinPlayer = finalist[1];

                        whichPlayerWin = CompareCards(finalist[1].GetCurrentCards(), finalist[2].GetCurrentCards(), eTable.None);

                        if (whichPlayerWin)
                        {
                            numWinPlayer = finalist[1];

                            whichPlayerWin = CompareCards(finalist[1].GetCurrentCards(), finalist[3].GetCurrentCards(), eTable.None);

                            if (whichPlayerWin)
                            {
                                numWinPlayer = finalist[1];

                            }
                            else
                            {
                                numWinPlayer = finalist[3];
                            }

                        }
                        else
                        {
                            numWinPlayer = finalist[2];

                            whichPlayerWin = CompareCards(finalist[2].GetCurrentCards(), finalist[3].GetCurrentCards(), eTable.None);

                            if (whichPlayerWin)
                            {
                                numWinPlayer = finalist[2];

                            }
                            else
                            {
                                numWinPlayer = finalist[3];
                            }
                        }
                    }
                    break;
            }
        }
        else if (finalist.Count == 5)
        {
            switch (maxCombinaion)
            {
                case 2:
                    //Collections.sort(finalist, new sortByPairCard());
                    whichPlayerWin = CompareCards(finalist[0].GetCurrentCards(), finalist[1].GetCurrentCards(), eTable.None);

                    if (whichPlayerWin)
                    {
                        numWinPlayer = finalist[0];

                        whichPlayerWin = CompareCards(finalist[0].GetCurrentCards(), finalist[2].GetCurrentCards(), eTable.None);

                        if (whichPlayerWin)
                        {
                            numWinPlayer = finalist[0];

                            whichPlayerWin = CompareCards(finalist[0].GetCurrentCards(), finalist[3].GetCurrentCards(), eTable.None);

                            if (whichPlayerWin)
                            {
                                numWinPlayer = finalist[0];

                                whichPlayerWin = CompareCards(finalist[0].GetCurrentCards(), finalist[4].GetCurrentCards(), eTable.None);

                                if (whichPlayerWin)
                                {
                                    numWinPlayer = finalist[0];

                                }
                                else
                                {
                                    numWinPlayer = finalist[4];
                                }

                            }
                            else
                            {
                                numWinPlayer = finalist[3];

                                whichPlayerWin = CompareCards(finalist[3].GetCurrentCards(), finalist[4].GetCurrentCards(), eTable.None);

                                if (whichPlayerWin)
                                {
                                    numWinPlayer = finalist[3];

                                }
                                else
                                {
                                    numWinPlayer = finalist[4];
                                }
                            }
                        }
                        else
                        {
                            numWinPlayer = finalist[2];

                            whichPlayerWin = CompareCards(finalist[2].GetCurrentCards(), finalist[3].GetCurrentCards(), eTable.None);

                            if (whichPlayerWin)
                            {
                                numWinPlayer = finalist[2];

                                whichPlayerWin = CompareCards(finalist[2].GetCurrentCards(), finalist[4].GetCurrentCards(), eTable.None);

                                if (whichPlayerWin)
                                {
                                    numWinPlayer = finalist[2];

                                }
                                else
                                {
                                    numWinPlayer = finalist[4];
                                }

                            }
                            else
                            {
                                numWinPlayer = finalist[3];

                                whichPlayerWin = CompareCards(finalist[3].GetCurrentCards(), finalist[4].GetCurrentCards(), eTable.None);

                                if (whichPlayerWin)
                                {
                                    numWinPlayer = finalist[3];

                                }
                                else
                                {
                                    numWinPlayer = finalist[4];
                                }
                            }
                        }
                    }
                    else
                    {
                        numWinPlayer = finalist[1];

                        whichPlayerWin = CompareCards(finalist[1].GetCurrentCards(), finalist[2].GetCurrentCards(), eTable.None);

                        if (whichPlayerWin)
                        {
                            numWinPlayer = finalist[1];

                            whichPlayerWin = CompareCards(finalist[1].GetCurrentCards(), finalist[3].GetCurrentCards(), eTable.None);

                            if (whichPlayerWin)
                            {
                                numWinPlayer = finalist[1];

                                whichPlayerWin = CompareCards(finalist[1].GetCurrentCards(), finalist[4].GetCurrentCards(), eTable.None);

                                if (whichPlayerWin)
                                {
                                    numWinPlayer = finalist[1];

                                }
                                else
                                {
                                    numWinPlayer = finalist[4];
                                }

                            }
                            else
                            {
                                numWinPlayer = finalist[3];

                                whichPlayerWin = CompareCards(finalist[3].GetCurrentCards(), finalist[4].GetCurrentCards(), eTable.None);

                                if (whichPlayerWin)
                                {
                                    numWinPlayer = finalist[3];

                                }
                                else
                                {
                                    numWinPlayer = finalist[4];
                                }
                            }

                        }
                        else
                        {
                            numWinPlayer = finalist[2];

                            whichPlayerWin = CompareCards(finalist[2].GetCurrentCards(), finalist[3].GetCurrentCards(), eTable.None);

                            if (whichPlayerWin)
                            {
                                numWinPlayer = finalist[2];

                                whichPlayerWin = CompareCards(finalist[2].GetCurrentCards(), finalist[4].GetCurrentCards(), eTable.None);

                                if (whichPlayerWin)
                                {
                                    numWinPlayer = finalist[2];

                                }
                                else
                                {
                                    numWinPlayer = finalist[4];
                                }

                            }
                            else
                            {
                                numWinPlayer = finalist[3];

                                whichPlayerWin = CompareCards(finalist[3].GetCurrentCards(), finalist[4].GetCurrentCards(), eTable.None);

                                if (whichPlayerWin)
                                {
                                    numWinPlayer = finalist[3];

                                }
                                else
                                {
                                    numWinPlayer = finalist[4];
                                }
                            }
                        }
                    }
                    break;

                default:
                    //Collections.sort(finalist, new sortByHighCard());
                    whichPlayerWin = CompareCards(finalist[0].GetCurrentCards(), finalist[1].GetCurrentCards(), eTable.None);

                    if (whichPlayerWin)
                    {
                        numWinPlayer = finalist[0];

                        whichPlayerWin = CompareCards(finalist[0].GetCurrentCards(), finalist[2].GetCurrentCards(), eTable.None);

                        if (whichPlayerWin)
                        {
                            numWinPlayer = finalist[0];

                            whichPlayerWin = CompareCards(finalist[0].GetCurrentCards(), finalist[3].GetCurrentCards(), eTable.None);

                            if (whichPlayerWin)
                            {
                                numWinPlayer = finalist[0];

                                whichPlayerWin = CompareCards(finalist[0].GetCurrentCards(), finalist[4].GetCurrentCards(), eTable.None);

                                if (whichPlayerWin)
                                {
                                    numWinPlayer = finalist[0];

                                }
                                else
                                {
                                    numWinPlayer = finalist[4];
                                }

                            }
                            else
                            {
                                numWinPlayer = finalist[3];

                                whichPlayerWin = CompareCards(finalist[3].GetCurrentCards(), finalist[4].GetCurrentCards(), eTable.None);

                                if (whichPlayerWin)
                                {
                                    numWinPlayer = finalist[3];

                                }
                                else
                                {
                                    numWinPlayer = finalist[4];
                                }
                            }
                        }
                        else
                        {
                            numWinPlayer = finalist[2];

                            whichPlayerWin = CompareCards(finalist[2].GetCurrentCards(), finalist[3].GetCurrentCards(), eTable.None);

                            if (whichPlayerWin)
                            {
                                numWinPlayer = finalist[2];

                                whichPlayerWin = CompareCards(finalist[2].GetCurrentCards(), finalist[4].GetCurrentCards(), eTable.None);

                                if (whichPlayerWin)
                                {
                                    numWinPlayer = finalist[2];

                                }
                                else
                                {
                                    numWinPlayer = finalist[4];
                                }

                            }
                            else
                            {
                                numWinPlayer = finalist[3];

                                whichPlayerWin = CompareCards(finalist[3].GetCurrentCards(), finalist[4].GetCurrentCards(), eTable.None);

                                if (whichPlayerWin)
                                {
                                    numWinPlayer = finalist[3];

                                }
                                else
                                {
                                    numWinPlayer = finalist[4];
                                }
                            }
                        }
                    }
                    else
                    {
                        numWinPlayer = finalist[1];

                        whichPlayerWin = CompareCards(finalist[1].GetCurrentCards(), finalist[2].GetCurrentCards(), eTable.None);

                        if (whichPlayerWin)
                        {
                            numWinPlayer = finalist[1];

                            whichPlayerWin = CompareCards(finalist[1].GetCurrentCards(), finalist[3].GetCurrentCards(), eTable.None);

                            if (whichPlayerWin)
                            {
                                numWinPlayer = finalist[1];

                                whichPlayerWin = CompareCards(finalist[1].GetCurrentCards(), finalist[4].GetCurrentCards(), eTable.None);

                                if (whichPlayerWin)
                                {
                                    numWinPlayer = finalist[1];

                                }
                                else
                                {
                                    numWinPlayer = finalist[4];
                                }

                            }
                            else
                            {
                                numWinPlayer = finalist[3];

                                whichPlayerWin = CompareCards(finalist[3].GetCurrentCards(), finalist[4].GetCurrentCards(), eTable.None);

                                if (whichPlayerWin)
                                {
                                    numWinPlayer = finalist[3];

                                }
                                else
                                {
                                    numWinPlayer = finalist[4];
                                }
                            }

                        }
                        else
                        {
                            numWinPlayer = finalist[2];

                            whichPlayerWin = CompareCards(finalist[2].GetCurrentCards(), finalist[3].GetCurrentCards(), eTable.None);

                            if (whichPlayerWin)
                            {
                                numWinPlayer = finalist[2];

                                whichPlayerWin = CompareCards(finalist[2].GetCurrentCards(), finalist[4].GetCurrentCards(), eTable.None);

                                if (whichPlayerWin)
                                {
                                    numWinPlayer = finalist[2];

                                }
                                else
                                {
                                    numWinPlayer = finalist[4];
                                }

                            }
                            else
                            {
                                numWinPlayer = finalist[3];

                                whichPlayerWin = CompareCards(finalist[3].GetCurrentCards(), finalist[4].GetCurrentCards(), eTable.None);

                                if (whichPlayerWin)
                                {
                                    numWinPlayer = finalist[3];

                                }
                                else
                                {
                                    numWinPlayer = finalist[4];
                                }
                            }
                        }
                    }
                    break;
            }
        }
        else if (finalist.Count == 1)
        {
            numWinPlayer = finalist[0];
        }
        else
        {
            if (numWinPlayer == null)
            {
                numWinPlayer = FindWinnerFromAll(players);
            }
        }
        return numWinPlayer;
    }
    public static PlayerManagerPun FindWinnerFromAll(PlayerManagerPun[] players)
    {
        eCombination[] playerInGameCombination = new eCombination[players.Length];
        int[] playerInGameHigh = new int[players.Length];
        int[] playerInGameTotal = new int[players.Length];

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].playerData.playerType != ePlayerType.Empty && players[i].playerData.currentCombination != eCombination.Empty && !players[i].playerData.IsPacked)
            {
                playerInGameCombination[i] = GetCombinationFromCard(players[i].GetCurrentCards());
                playerInGameHigh[i] = GetHighCard(players[i].GetCurrentCards());
                playerInGameTotal[i] = GetTotalCard(players[i].GetCurrentCards());
            }
        }

        int maxCombinaion = 0;
        int numWinPlayer = 0;
        for (int i = 0; i < players.Length; i++)
        {
            if (maxCombinaion < (int)playerInGameCombination[i])
            {
                maxCombinaion = (int)playerInGameCombination[i];
                numWinPlayer = i;
                //players[numWinPlayer].DebugLog("if con " + maxCombinaion + "numWinPlayer " + numWinPlayer);
            }
            else if (maxCombinaion == (int)playerInGameCombination[i])
            {
                //players[numWinPlayer].DebugLog("else if con " + maxCombinaion + "numWinPlayer " + numWinPlayer);
                if (GetHighCard(players[numWinPlayer].GetCurrentCards()) < playerInGameHigh[i])
                {
                    maxCombinaion = (int)playerInGameCombination[i];
                    numWinPlayer = i;
                    //players[numWinPlayer].DebugLog("else if1if con " + maxCombinaion + "numWinPlayer " + numWinPlayer);
                }
                else if (GetHighCard(players[numWinPlayer].GetCurrentCards()) == playerInGameHigh[i])
                {
                    //players[numWinPlayer].DebugLog("else if2if con " + maxCombinaion + "numWinPlayer " + numWinPlayer);
                    if (GetTotalCard(players[numWinPlayer].GetCurrentCards()) < playerInGameTotal[i])
                    {
                        maxCombinaion = (int)playerInGameCombination[i];
                        numWinPlayer = i;
                        //players[numWinPlayer].DebugLog("else if3if con " + maxCombinaion + "numWinPlayer " + numWinPlayer);
                    }
                }
            }
        }
        return players[numWinPlayer];
    }

   public static bool CompareCards(CardData[] cardsPlayer1, CardData[] cardsPlayer2,eTable table)
   {
        eCombination player1 = GetCombinationFromCard(cardsPlayer1);
        eCombination player2 = GetCombinationFromCard(cardsPlayer2);
        if((int)player1 > (int)player2)
        {
            return true;
        }
        else if((int)player1 == (int)player2)
        {
            if ((int)player1 == 6)//for trail condition
            {
               
                        int totalCard1 = GetTotalCard(cardsPlayer1);
                        int totalCard2 = GetTotalCard(cardsPlayer2);
                        if (totalCard1 > totalCard2)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                   
               
            }
            else if ((int)player1 == 5 || (int)player1 == 4)//for PureSequence condition or sequence
            {

                int highCard1 = GetHighCard(cardsPlayer1);
                int highCard2 = GetHighCard(cardsPlayer2);
                if (highCard1 == highCard2)
                {
                    if (highCard1 == 14)
                    {
                        if((cardsPlayer1[1].rankCard ==2 || cardsPlayer2[1].rankCard==2) && (cardsPlayer1[1].rankCard == 3 || cardsPlayer2[1].rankCard == 3))
                        {
                            if(cardsPlayer1[1].rankCard == 2 || cardsPlayer1[1].rankCard == 3)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if (cardsPlayer1[1].rankCard == cardsPlayer2[1].rankCard)
                        {
                            if (cardsPlayer1[0].rankCard > cardsPlayer2[0].rankCard)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if (cardsPlayer1[1].rankCard > cardsPlayer2[1].rankCard)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (cardsPlayer1[1].rankCard == cardsPlayer2[1].rankCard)
                        {
                            if (cardsPlayer1[0].rankCard > cardsPlayer2[0].rankCard)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else if (cardsPlayer1[1].rankCard > cardsPlayer2[1].rankCard)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }

                }
                else
                {
                    if (highCard1 > highCard2)
                        return true;
                    else
                        return false;
                }


            }
            else if ((int)player1 == 2)//for pair condition
            {
                if(cardsPlayer1[1].rankCard == cardsPlayer2[1].rankCard)
                {
                    int highCard1 = GetHighCard(cardsPlayer1);
                    int highCard2 = GetHighCard(cardsPlayer2);
                    if (highCard1 == highCard2)
                    {
                        int totalCard1 = GetTotalCard(cardsPlayer1);
                        int totalCard2 = GetTotalCard(cardsPlayer2);
                        if (totalCard1 > totalCard2)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        if (highCard1 > highCard2)
                            return true;
                        else
                            return false;
                    }
                }
                else if (cardsPlayer1[1].rankCard > cardsPlayer2[1].rankCard)
                {
                    return true;
                }
                return false;
            }
            else if ((int)player1 == 1)//for high condition // If two players share a common high card, the next highest card is used to determine the winner and so on The best high card hand would be an AKJ of different suits and the worst is 5-3-2.
            {
                int highCard1 = GetHighCard(cardsPlayer1);
                int highCard2 = GetHighCard(cardsPlayer2);
                Debug.Log("highCard " + highCard1 + " " + highCard2);
                if (highCard1 == highCard2)
                {
                    Debug.Log("cardsPlayer1[1].rankCard " + cardsPlayer1[1].rankCard + " " + cardsPlayer2[1].rankCard);
                    if (cardsPlayer1[1].rankCard == cardsPlayer2[1].rankCard)
                    {
                        Debug.Log("cardsPlayer1[0].rankCard " + cardsPlayer1[0].rankCard + " " + cardsPlayer2[0].rankCard);
                        if (cardsPlayer1[0].rankCard > cardsPlayer2[0].rankCard)
                        {
                            Debug.Log("cardsPlayer1[1].rankCard " + true);
                            return true;
                        }
                        else
                        {
                            Debug.Log("cardsPlayer1[1].rankCard " + false);
                            return false;
                        }
                    }
                    else if (cardsPlayer1[1].rankCard > cardsPlayer2[1].rankCard)
                    {
                        Debug.Log("cardsPlayer1[1].rankCard.... " + true);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    
                }
                else
                {
                    if (highCard1 > highCard2)
                        return true;
                    else
                        return false;
                }
            }
            else if ((int)player1 == 3)//Three cards of the same suit that are not in sequence. When comparing two colors, first compare the highest card. If these are equal, compare the second and if these are equal compare the lowest. Highest flush is A-K-J and the lowest flush is 5-3-2..
            {
                int highCard1 = GetHighCard(cardsPlayer1);
                int highCard2 = GetHighCard(cardsPlayer2);
                if (highCard1 == highCard2)
                {
                    if (cardsPlayer1[1].rankCard == cardsPlayer2[1].rankCard)
                    {
                        if (cardsPlayer1[0].rankCard > cardsPlayer2[0].rankCard)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (cardsPlayer1[1].rankCard > cardsPlayer2[1].rankCard)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    if (highCard1 > highCard2)
                        return true;
                    else
                        return false;
                }
            }
            else
            {
                int highCard1 = GetHighCard(cardsPlayer1);
                int highCard2 = GetHighCard(cardsPlayer2);
                if (highCard1 == highCard2)
                {
                   
                    int totalCard1 = GetTotalCard(cardsPlayer1);
                    int totalCard2 = GetTotalCard(cardsPlayer2);
                    if (totalCard1 > totalCard2)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (highCard1 > highCard2)
                        return true;
                    else
                        return false;
                }
            }
        }
        else
        {
            return false;
        }
    }
}


