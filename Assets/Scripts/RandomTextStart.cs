using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomTextStart : MonoBehaviour
{
    public Text RandomText;
    // Start is called before the first frame update
    
    private void OnEnable()
    {
        StartCoroutine(Changetext(1));
    }

    IEnumerator Changetext(int value)
    {
        yield return new WaitForSeconds(0.5f);
        switch (value)
        {
            case 1:
                RandomText.text = "Loading Game...";
                break;
            case 2:
                RandomText.text = "Message your friends.. ";
                break;
            case 3:
                RandomText.text = "Create a Game Room and play games with your buddies. ";
                break;
            case 4:
                RandomText.text = "Play KhelTamasha daily. ";
                break;
            case 5:
                RandomText.text = "Beat other players. ";
                break;
            default:
                RandomText.text = "Fetching details...";
                break;
        }
        value++;
        StartCoroutine(Changetext(value));
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
   
}
