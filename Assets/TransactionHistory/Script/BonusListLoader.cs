using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mopsicus.InfiniteScroll;
using UnityEngine.UI;

public class BonusListLoader : MonoBehaviour
{

   
    

    [SerializeField]
    private List<BonusList> TransactionKeys = new List<BonusList>();
    private int TransactionLoaded = 0;
    public Text MessageLabel;

    private bool TransactionWasLoaded = false;
    [SerializeField]
    private InfiniteScroll Scroll;
    private string MessageLabelValue;

    private int pageIndex = 0;
    private int pageSize = 12;
    [SerializeField]
    private int PullCount = 12;
    [SerializeField]
    private int TotalRecordCount = 0;
    public GameObject BottomLoading;



    void OnFillItem(int index, GameObject item)
    {
        //Debug.Log("index " + index );
        //Debug.Log("item " + item.name);
        if (item.GetComponent<BonusListViewController>() != null)
        {
            if(TransactionKeys != null)
            {
                if (index < TransactionKeys.Count)
                {
                    item.GetComponent<BonusListViewController>().DisplayInfo(TransactionKeys[index]);
                }
            }
           
            
        }
    }
    void Update()
    {
        if (TotalRecordCount > 0)
        {
            if (TransactionKeys != null)
            {
                if (TransactionKeys.Count >= TotalRecordCount)
                {
                    if (BottomLoading)
                    {
                        BottomLoading.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
    void OnPullItem(InfiniteScroll.Direction direction)
    {
        int index = TransactionKeys.Count;
        Debug.Log("index " + index);
        if (direction == InfiniteScroll.Direction.Top)
        {

        }
        else
        {
            Debug.Log("index in else " + index);
            if (index < TotalRecordCount)
            {
                APICall_Second();
            }
            else
            {
                Debug.Log("index in else else " + index);
                Scroll.OnPull -= OnPullItem;
            }
        }


        Scroll.ApplyDataTo(TransactionKeys.Count, PullCount, direction);

    }
    int OnHeightItem(int index)
    {
        return 100;
    }
    private void OnEnable()
    {
        TransactionWasLoaded = false;
       
        ResetLoader();
        
        
    }
    private void OnDisable()
    {
        Debug.Log("OnDisable ");
        Scroll.OnFill -= OnFillItem;
        Scroll.OnHeight -= OnHeightItem;
        Scroll.OnPull -= OnPullItem;
    }
    public void ResetLoader()
    {
        TransactionWasLoaded = false;
        TransactionLoaded = 0;
        TransactionKeys.Clear();
        TransactionKeys.TrimExcess();

        APICall_First();
    }
    public void APICall_First()
    {
        if (PlayerSave.singleton != null)
        {
            pageIndex = 1;
            //Debug.Log("pageIndex " + pageIndex);

            PlayerSave.singleton.GetBonusDetails(PlayerSave.singleton.newID(), pageIndex.ToString(), pageSize.ToString(), OnListLoaded); 

        }

    }
    public void APICall_Second()
    {
        if (PlayerSave.singleton != null)
        {
            pageIndex++;
            Debug.Log("pageIndex " + pageIndex);

            PlayerSave.singleton.GetBonusDetails(PlayerSave.singleton.newID(), pageIndex.ToString(), pageSize.ToString(), OnListLoadedSecond);
        }
    }
    private void RemoveKeyAt(int _index)
    {
        TransactionKeys.RemoveAt(_index);
        TransactionKeys.TrimExcess();
    }
    public void OnListLoaded(GetBonusDetails _callback)
    {
        
        if(_callback!=null)
        {
            if (_callback.status.Equals("200"))
            {
                TransactionWasLoaded = true;
                //Debug.Log("OnListLoaded " + _callback);
                int payMentCount = 0;
                TotalRecordCount = Convert.ToInt32(_callback.data.RecordCount);
                if (_callback.data!=null)
                {
                    if(_callback.data.bonusList!=null)
                    {
                        payMentCount = _callback.data.bonusList.Length;
                    }
                  
                }

             
                


                for (int i = 0; i < payMentCount; i++)
                {

                    if (_callback.message.Equals("OK"))
                    {
                        TransactionLoaded++;
                        //Debug.Log("MessagesLoaded " + TransactionLoaded);

                        AddTransactionKey(_callback.data.bonusList[i]);
                    }
                    else
                    {
                        TransactionLoaded--;
                        //Debug.Log("MessagesLoaded minus " + TransactionLoaded);
                    }
                }

               
                if (payMentCount > 0)
                {
                    Debug.Log("payMentCount gretaer 0 ");
                    Scroll.OnFill += OnFillItem;
                    Scroll.OnHeight += OnHeightItem;
                    Scroll.OnPull += OnPullItem;
                    Scroll.InitData(payMentCount);
                }

                if (payMentCount >0)
                {
                    if (MessageLabel)
                    {
                        MessageLabel.gameObject.SetActive(false);
                    }

                }
                else if (payMentCount <= 0)
                {
                    if (MessageLabel)
                    {
                        MessageLabelValue = "No transactions found in last 30 days....";
                        MessageLabel.text = MessageLabelValue.ToString();
                        MessageLabel.gameObject.SetActive(true);
                    }



                }
            }
            else if (_callback.status.Equals("404"))
            {
                //Debug.Log("OnListLoaded no transaction found ");
                if (MessageLabel)
                {
                    MessageLabelValue = "No transactions found in last 30 days....";
                    MessageLabel.text = MessageLabelValue.ToString();
                    MessageLabel.gameObject.SetActive(true);
                }


            }
            else
            {
                //Debug.Log("else OnListLoaded no  ");
                if (MessageLabel)
                {
                    MessageLabelValue = "In case, if your transaction history is not updating or showing any previous transactions, this might be because of a bug or outdated version of the application.Your transaction history will be back online once you update the applications or when all the issues are resolved.";
                    MessageLabel.text = MessageLabelValue.ToString();
                    MessageLabel.gameObject.SetActive(true);
                }
            }
        }
        else
        {
            //Debug.Log("else else OnListLoaded no  ");
            if (MessageLabel)
            {
                MessageLabelValue = "In case, if your transaction history is not updating or showing any previous transactions, this might be because of a bug or outdated version of the application. Your transaction history will be back online once you update the applications or when all the issues are resolved.";
                MessageLabel.text = MessageLabelValue.ToString();
                MessageLabel.gameObject.SetActive(true);
            }
        }

    }
    public void OnListLoadedSecond(GetBonusDetails _callback)
    {

        if (_callback != null)
        {
            //Debug.Log("_callback.status " + _callback.status);
            if (_callback.status.Equals("200"))
            {
                TransactionWasLoaded = true;
                //Debug.Log("OnListLoaded " + _callback.message);
                int payMentCount = 0;

                
                
                if (_callback.data != null)
                {
                    if (_callback.data.bonusList != null)
                    {
                        payMentCount = _callback.data.bonusList.Length;
                    }

                }

                for (int i = 0; i < payMentCount; i++)
                {

                    if (_callback.message.Equals("OK"))
                    {
                        TransactionLoaded++;
                        //Debug.Log("MessagesLoaded " + TransactionLoaded);

                        AddTransactionKey(_callback.data.bonusList[i]);
                    }
                    else
                    {
                        TransactionLoaded--;
                        //Debug.Log("MessagesLoaded minus " + TransactionLoaded);
                    }
                }



                if (TransactionKeys != null)
                {
                    if (TransactionKeys.Count > 0)
                    {
                        if (MessageLabel)
                        {
                            MessageLabel.gameObject.SetActive(false);
                        }

                    }
                    else if (TransactionKeys.Count <= 0)
                    {
                        if (MessageLabel)
                        {
                            MessageLabelValue = "No transactions found in last 30 days...." +
                                "";
                            MessageLabel.text = MessageLabelValue.ToString();
                            MessageLabel.gameObject.SetActive(true);
                        }



                    }
                }

            }
        }
    }

    private void AddTransactionKey(BonusList _key)
    {
        if (!TransactionKeys.Contains(_key))
        {
            TransactionKeys.Add(_key);
        }
    }
    
}

