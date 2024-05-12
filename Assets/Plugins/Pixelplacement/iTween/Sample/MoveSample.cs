using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MoveSample : MonoBehaviour
{

    public GameObject gameObjectK;

    public GameObject Target;

    void Start(){
		//iTween.MoveBy(gameObject, iTween.Hash("x", 2, "easeType", "easeInOutExpo", "loopType", "pingPong", "delay", .1));

        iTween.ValueTo(gameObjectK, iTween.Hash(
      "from", gameObjectK.transform.position,
      "to", Target.transform.position,
      "time", 1f,
      "delay", 0.2f,
      "looptype", iTween.LoopType.loop,
      "easeType", iTween.EaseType.linear,
      "onupdatetarget", this.gameObject,
        "onupdate", "MoveSideShowTitle",
        "onstart","StartSideShowTitle",
        "onstarttarget",this.gameObject,
        "oncompletetarget", this.gameObject, "oncomplete", "GoBackToOriginal"
      ));//add id for reference
    }
    public void MoveSideShowTitle(Vector3 Tposition)
    {

        gameObjectK.SetActive(true);
        gameObjectK.transform.position = Tposition;
    }
    public void StartSideShowTitle()
    {
        Debug.Log("StartShow");
        gameObjectK.GetComponent<Image>().enabled = true;

    }
    public void GoBackToOriginal()
    {
        Debug.Log("GoBack");
        gameObjectK.GetComponent<Image>().enabled = false;
    }
}

