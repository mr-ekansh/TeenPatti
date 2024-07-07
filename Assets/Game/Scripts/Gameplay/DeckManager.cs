using UnityEngine;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
{

    private List<CardData> currentDeck;
    private List<CardData> customDeck;
    private const int deckCount = 52;
    private List<CardData> Player1Cards;
    private List<CardData> Player2Cards;
    private List<CardData> Player3Cards;
    private List<CardData> Player4Cards;
    private List<CardData> Player5Cards;

    public void NewDeck()
    {
        currentDeck = new List<CardData> ();
        int suit = 1;
        int rank = 2;
        for (int i = 0; i < deckCount; i++)
        {
            if(rank % 15 == 0)
            {
                suit++;
                rank = 2;
            }
            //Debug.Log(" suit "+ (eCardSuit)suit +"rank.. " + rank);
            currentDeck.Add(new CardData(suit, rank, true));          
            rank++;
        }
    }

    public void NewDeck(eTable tableMode)
    {
        currentDeck = new List<CardData>();

        int suit = 1;
        int rank = 2;

        for (int i = 0; i < deckCount; i++)
        {
            if (rank % 15 == 0)
            {
                suit++;
                rank = 2;
            }

            if (tableMode == eTable.Royal)
            {
                if (rank == 1 || rank >= 10)
                    currentDeck.Add(new CardData(suit, rank, true));
                else
                {
                    rank++;
                    continue;
                }
            }
            else
            {
                currentDeck.Add(new CardData(suit, rank, true));
            }

            rank++;
        }
    }
    //public void generateCustomDeck()
    //{
    //    for (int i = 0; i < 3; i++)
    //    {
    //        Player1Cards.Add(GetRandomCard());
    //        Player2Cards.Add(GetRandomCard());
    //        Player3Cards.Add(GetRandomCard());
    //        Player4Cards.Add(GetRandomCard());
    //        Player5Cards.Add(GetRandomCard());
    //    }

    //    // Compare player cards to determine the highest winner
    //    List<List<CardData>> players = new List<List<CardData>> { Player1Cards, Player2Cards, Player3Cards, Player4Cards, Player5Cards };
    //    int winnerIndex = 0;

    //    for (int i = 1; i < players.Count; i++)
    //    {
    //        bool isWin = CardCombination.CompareCards(players[winnerIndex].ToArray(), players[i].ToArray(), eTable.None);
    //        if (!isWin)
    //        {
    //            winnerIndex = i;
    //        }
    //    }

    //    // Swap the winner's cards with Player1 if not already Player1
    //    if (winnerIndex != 0)
    //    {
    //        List<CardData> temp = new List<CardData>(Player1Cards);
    //        Player1Cards = new List<CardData>(players[winnerIndex]);
    //        players[winnerIndex] = temp;
    //    }

    //    // Log the winner
    //    Debug.Log($"The winning player is Player {winnerIndex + 1}");
    //}

    public CardData GetRandomCard()
    {
        int randomNum = Random.Range(0 , currentDeck.Count);
        CardData cardGive = currentDeck[randomNum];
        currentDeck.RemoveAt(randomNum);
        return cardGive;
    }
    //public CardData GetCustomCard(bool )
    //{
    //    int randomNum = Random.Range(0, currentDeck.Count);
    //    CardData cardGive = currentDeck[randomNum];
    //    currentDeck.RemoveAt(randomNum);
    //    return cardGive;
    //}

}
