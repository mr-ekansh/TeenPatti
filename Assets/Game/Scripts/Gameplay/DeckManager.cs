using UnityEngine;
using System.Collections.Generic;

public class DeckManager : MonoBehaviour
{

    private List<CardData> currentDeck;
    private const int deckCount = 52;    

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

        //currentDeck.Add(new CardData(2, 2, true));
        //currentDeck.Add(new CardData(4, 8, true));
        //currentDeck.Add(new CardData(2, 5, true));
        //currentDeck.Add(new CardData(2, 10, true));
        //currentDeck.Add(new CardData(4, 14, true));
        //currentDeck.Add(new CardData(1, 14, true));
        
       
       
        
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
    public CardData GetRandomCard()
    {
        int randomNum = Random.Range(0 , currentDeck.Count);
        CardData cardGive = currentDeck[randomNum];
        currentDeck.RemoveAt(randomNum);
        return cardGive;
    }
    
}
