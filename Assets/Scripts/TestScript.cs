using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Text newText;
    public CustomInputField inputField;

    public CardData[] NewCurrentCards;
    public CardData[] OldCurrentCards;
    void Start()
    {
        NewCurrentCards = new CardData[3];
        OldCurrentCards = new CardData[3];
        NewCurrentCards[0] = new CardData(4, 6, false);
        NewCurrentCards[1] = new CardData(4, 8, false);
        NewCurrentCards[2] = new CardData(2, 8, false);

        OldCurrentCards[0] = new CardData(2, 2, false);
        OldCurrentCards[1] = new CardData(3, 2, false);
        OldCurrentCards[2] = new CardData(2, 7, false);
        bool firstWin = CardCombination.CompareCards(OldCurrentCards, NewCurrentCards, eTable.Standard);

        eCombination player1 = CardCombination.GetCombinationFromCard(NewCurrentCards);
        eCombination player2 = CardCombination.GetCombinationFromCard(OldCurrentCards);
        Debug.Log("firstWin " + firstWin + " player1 "+ player1 +" player2 "+player2);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    string newLine = "";
    public void SendMessage_Button()
    {
        if(!string.IsNullOrEmpty(inputField.text))
        {
            newLine += inputField.text;
            inputField.text = "";
            newText.text = newLine;
        }
    }
    public void Clear()
    {
        newLine = "";
        newText.text = "";
    }
}
