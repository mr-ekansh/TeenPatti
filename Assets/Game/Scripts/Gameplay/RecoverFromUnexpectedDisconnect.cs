using System;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

public class RecoverFromUnexpectedDisconnect : IConnectionCallbacks
{
    private LoadBalancingClient loadBalancingClient;
    private AppSettings appSettings;

    public RecoverFromUnexpectedDisconnect(LoadBalancingClient loadBalancingClient, AppSettings appSettings)
    {
        this.loadBalancingClient = loadBalancingClient;
        this.appSettings = appSettings;
        this.loadBalancingClient.AddCallbackTarget(this);
    }

    ~RecoverFromUnexpectedDisconnect()
    {
        this.loadBalancingClient.RemoveCallbackTarget(this);
        
    }

    void IConnectionCallbacks.OnDisconnected(DisconnectCause cause)
    {
        if (this.CanRecoverFromDisconnect(cause))
        {
            this.Recover();
        }
    }

    private bool CanRecoverFromDisconnect(DisconnectCause cause)
    {
        Debug.Log("cause " + cause.ToString());
        switch (cause)
        {
            // the list here may be non exhaustive and is subject to review
            case DisconnectCause.Exception:
            case DisconnectCause.ServerTimeout:
            case DisconnectCause.ClientTimeout:
            case DisconnectCause.DisconnectByServerLogic:
            case DisconnectCause.DisconnectByServerReasonUnknown:
                return true;
        }
        return false;
    }

    private void Recover()
    {
        if (!loadBalancingClient.ReconnectAndRejoin())
        {
            Debug.LogError("ReconnectAndRejoin failed, trying Reconnect");
            if (!loadBalancingClient.ReconnectToMaster())
            {
                Debug.LogError("Reconnect failed, trying ConnectUsingSettings");
                if (!loadBalancingClient.ConnectUsingSettings(appSettings))
                {
                    Debug.LogError("ConnectUsingSettings failed");
                }
            }
        }
    }

    #region Unused Methods

    void IConnectionCallbacks.OnConnected()
    {
        Debug.Log("OnConnected");
    }

    void IConnectionCallbacks.OnConnectedToMaster()
    {
        Debug.Log("OnConnectedToMaster");
    }

    void IConnectionCallbacks.OnRegionListReceived(RegionHandler regionHandler)
    {
        Debug.Log("OnRegionListReceived");
    }

    void IConnectionCallbacks.OnCustomAuthenticationResponse(Dictionary<string, object> data)
    {
        Debug.Log("OnCustomAuthenticationResponse ");
    }

    void IConnectionCallbacks.OnCustomAuthenticationFailed(string debugMessage)
    {
        Debug.Log("OnCustomAuthenticationFailed " + debugMessage);
    }

    #endregion
}