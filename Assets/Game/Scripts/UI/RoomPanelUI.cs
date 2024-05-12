using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RoomPanelUI : MonoBehaviourPunCallbacks
{
    private List<GameObject> content=new List<GameObject>();
    private bool isTup;
    public GameObject panelLoad;
    public GameObject cloneInfo;
    public GameObject contentParent;
   
    // Use this for initialization
   

    
    
    public void ShowIntegrate(List<RoomInfo> roomInfos)
    {
        if (isTup)
            return;

       Invoke("CanTup",3);
        panelLoad.SetActive(true);
        contentParent.gameObject.SetActive(true);
        isTup = true;
        

        
        if(roomInfos!=null)
        {
            if(roomInfos.Count>0)
            {
                CreteBoard(roomInfos);
            }
            else
            {
                if(PhotonNetwork.InRoom)
                {
                    CreteBoard(1, PhotonNetwork.CurrentRoom.Name, PhotonNetwork.CurrentRoom.PlayerCount.ToString(), PhotonNetwork.CurrentRoom.MaxPlayers.ToString());
                }
            }
        }
        else
        {
            if (PhotonNetwork.InRoom)
            {
                CreteBoard(1, PhotonNetwork.CurrentRoom.Name, PhotonNetwork.CurrentRoom.PlayerCount.ToString(), PhotonNetwork.CurrentRoom.MaxPlayers.ToString());
            }
        }
    }

    private void CanTup()
    {
        panelLoad.SetActive(false);
        isTup = false;
    }

    private void CreteBoard(List<RoomInfo> roomInfos)
    {
        //GameObject cloneInfo = transform.Find("BgWorld/Content/RoomInfo").gameObject;

        if(content!=null)
        {
            for (int i = 0; i < content.Count; i++)
            {
                Destroy(content[0].gameObject);
            }
        }
        content.Clear();
        content.TrimExcess();
        for (int i = 0; i < roomInfos.Count; i++)
        {
                if (roomInfos[i].Name.Contains(PlayerSave.roomName))
                {
                    GameObject _clone = Instantiate(cloneInfo, cloneInfo.transform.parent);
                    content.Add(_clone);
                    _clone.gameObject.SetActive(true);
                    _clone.transform.Find("TextNum").GetComponent<Text>().text = (i + 1).ToString();
                    _clone.transform.Find("TextName").GetComponent<Text>().text = roomInfos[i].Name;
                    _clone.transform.Find("TextScore").GetComponent<Text>().text = roomInfos[i].PlayerCount.ToString() + "/" + roomInfos[i].MaxPlayers.ToString();
                }
                Debug.Log("Server roomInfos: " + roomInfos[i].Name);
        }
        
        cloneInfo.SetActive(false);      
    }
    
    private void CreteBoard(int Count,string roomName,string roomPlayerCount,string roomMaxPlayers)
    {
        //GameObject cloneInfo = transform.Find("BgWorld/Content/RoomInfo").gameObject;

        if (content != null)
        {
            for (int i = 0; i < content.Count; i++)
            {
                Destroy(content[0].gameObject);
            }
        }
        content.Clear();
        content.TrimExcess();
        for (int i = 0; i < Count; i++)
        {
            GameObject _clone = Instantiate(cloneInfo, cloneInfo.transform.parent);
            content.Add(_clone);
            _clone.gameObject.SetActive(true);
            _clone.transform.Find("TextNum").GetComponent<Text>().text = (i + 1).ToString();
            _clone.transform.Find("TextName").GetComponent<Text>().text = roomName;
            _clone.transform.Find("TextScore").GetComponent<Text>().text = roomPlayerCount.ToString() + "/" + roomMaxPlayers.ToString();
            Debug.Log("Server roomInfos: " + roomName);
        }

        cloneInfo.SetActive(false);
    }
    
    public void OnBackButton()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            PlayerSave.roomName = "";
        }
    }
    public void OnStartButton()
    {
        if(PhotonNetwork.InRoom)
        {
            //PlayerSave.singleton.ShowErrorMessage("Join Room!!!");
            PhotonNetwork.LoadLevel(2);
        }
        else
        {
            if (MatchMakingPhoton.makingPhoton != null)
            {
                
            }
                
               
        }
    }
    public void OnRoomResponse(bool isRoom)
    {
        Debug.Log("isRoom " + isRoom + "PhotonNetwork.inRoom "+ PhotonNetwork.InRoom);
        if (isRoom)
        {
            if(PhotonNetwork.InRoom)
            {
                PhotonNetwork.LoadLevel(2);
            }
        }
    }
}
