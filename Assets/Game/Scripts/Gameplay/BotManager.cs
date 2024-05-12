using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Gamelogic.Extensions;
using Photon.Pun;
using UnityEngine;

public class BotManager : Singleton<BotManager>
{
    private struct Data
    {
        public int Id;
        public PlayerManagerPun Network;
        public PlayerData PlayerData;
        public PlayerUI PlayerUI;
        public TeenPattiPhoton Main;
        public TeenPatiHUD HUD;
        public TableInfo Table;
    }

    private float _totalTime;
  
    private Data _data;
    private bool isDebug = true;

    public void SetBotTurn(int id, float totalTime, PlayerManagerPun network, PlayerData data, PlayerUI ui,
        TeenPattiPhoton main, TeenPatiHUD hud, TableInfo table)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            if (isDebug)
            {
                Debug.LogWarning("Only run bot system in MasterClient");
            }
            return;
        }

        _totalTime = totalTime;
     
        _data = new Data
        {
            Id = id,
            Network = network,
            PlayerData = data,
            PlayerUI = ui,
            Main = main,
            HUD = hud,
            Table = table
        };

        var sleepTime = Random.Range(1f, 3f);
        StopCoroutine("_Calculate");
        StartCoroutine("_Calculate", sleepTime);

        
    }
    public int CountPlayersInGame(List<PlayerManagerPun> players)
    {
        int playersInGame = 0;
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] == null)
            {
                players.Remove(players[i]);
                i--;
                continue;
            }

            if (players[i].playerData.playerType == ePlayerType.PlayerStartGame && players[i].playerData.currentCombination != eCombination.Empty && !players[i].playerData.IsPacked)
            {
                playersInGame++;
            }
        }

      

        return playersInGame;
    }
    //bool isIAnyBotWinner = false;
    private IEnumerator _Calculate(float time)
    {
        if (_data.Main.typeTable == eTable.AndarBahar)
        {
            if (isDebug)
            {
                Debug.Log($"[{_data.PlayerData.NamePlayer}]: Wait for {time}s");
            }
            yield return new WaitForSeconds(time);

            var r = Random.Range(0, 1.0f);
            if (r < 0.4f)
            {
                //Debug.Log($"[{_data.PlayerData.NamePlayer}]: Decided to bet andar");
                // _data.Network.photonView.RPC("PackBot", RpcTarget.AllViaServer);
                // _data.Network.photonView.RPC("ProceedPackOnServer", RpcTarget.MasterClient);
                yield break;
            }
            else if (r > 0.6f)
            {
                //Debug.Log($"[{_data.PlayerData.NamePlayer}]: Decided to bet bahar");
                // _data.Network.photonView.RPC("PackBot", RpcTarget.AllViaServer);
                // _data.Network.photonView.RPC("ProceedPackOnServer", RpcTarget.MasterClient);
                yield break;
            }
            else
            {
                //Debug.Log($"[{_data.PlayerData.NamePlayer}]: Decided to skip bet");
                yield break;
            }
        }
        else
        {
            if (_data.PlayerData.IsPacked) yield break;

            //Debug.Log($"[{_data.PlayerData.NamePlayer}]: Wait for {time}s");
            yield return new WaitForSeconds(time);

            var GetValue = _data.PlayerData._RandNext;// CalculateLogicForBots();// Random.Range(0, 1.0f);
            bool isIAmWinner = false;
            

            if (_data.Main.typeTable != eTable.PotBlind)
            {
                if (!_data.PlayerData.IsSeenCard)
                {
                    var r = Random.Range(0f, 1f);
                    //Debug.Log($"[{_data.PlayerData.NamePlayer}]: Before Decided to see the cards" +$"[{GetValue}]: GetValue");
                    if (r <= 0.6f || _data.PlayerData.step >= 3)
                    {
                       // Debug.Log($"[{_data.PlayerData.NamePlayer}]: Decided to see the cards.....1");
                        if (_data.Network != null)
                        {
                            _data.Network.OnSeenCard();
                        }
                    }
                    else
                    {
                        if (_data.Network != null)
                        {
                            _data.PlayerData.BlindCount++;
                        }
                    }
                }

				var r2 = Random.Range(0f, 1f);

				if(r2 <= 0.6f)
				{
					try
					{
						List<PlayerManagerPun> players = new List<PlayerManagerPun>();
						players = FindObjectsOfType<PlayerManagerPun>().ToList();
						if (players != null)
						{
							players = players.OrderBy(x => x.GetIdOrderUI()).ToList();
							if (players.Count > 0)
							{
								var winPlayer = CardCombination.decideWinner(players.ToArray());
								if (winPlayer != null)
								{
									Debug.Log("winPlayer " + winPlayer.playerData.NamePlayer + " winPlayer.playerData.IsBot "+ winPlayer.playerData.IsBot);
									if (winPlayer.playerData.IsBot)
									{
										r2 = 0f;

									}
									else
									{
										r2= 1f;
									}
									 Debug.Log("winPlayer start after " + winPlayer.playerData.NamePlayer + "r2 " + r2);
								}
								else
								{
									r2 = 1f;
								}
							}
							else
							{
								r2 = 1f;
							}
						}
						else
						{
							r2 = 1f;
						}
					}
					catch
					{
						r2 = 1f;
					}

					if(r2 <= 0.6f)
					{
						Debug.Log("winPlayer start after increase bet r2 " + r2);
						if (_data.Network != null)
						{
							_data.Network.photonView.RPC("IncreaseBet", RpcTarget.AllViaServer);

						}
					}
				}
            }
            //Debug.Log($"<color=red>[{_data.PlayerData.NamePlayer}]: Decision Value is : " + $"[{GetValue}]: GetValue</color>");
            //isIAnyBotWinner = false;
            //test();
            if (GetValue < 1)
            {
                //Debug.Log($"[{_data.PlayerData.NamePlayer}]: Decided to pack");
                var r = Random.Range(0f, 1f);
                try
                {
                    List<PlayerManagerPun> players = new List<PlayerManagerPun>();
                    players = FindObjectsOfType<PlayerManagerPun>().ToList();
                    if (players != null)
                    {
                        players = players.OrderBy(x => x.GetIdOrderUI()).ToList();
                        if (players.Count > 0)
                        {
                            var winPlayer = CardCombination.decideWinner(players.ToArray());
                            if (winPlayer != null)
                            {
                                //Debug.Log("winPlayer start before " + winPlayer.playerData.NamePlayer + "r " + r + "winPlayer.playerData._DeviceID "+ winPlayer.playerData._DeviceID + " _data.PlayerData._DeviceID "+ _data.PlayerData._DeviceID);
                                if (winPlayer.playerData._DeviceID == _data.PlayerData._DeviceID)
                                {
                                    r = 0f;
                                    isIAmWinner = true;
                                }
                                else
                                {
                                    r = 1f;
                                }
                               // Debug.Log("winPlayer start after " + winPlayer.playerData.NamePlayer + "r " + r);
                            }
                            else
                            {
                                r = 1f;
                            }
                        }
                        else
                        {
                            r = 1f;
                        }
                    }
                    else
                    {
                        r = 1f;
                    }
                }
                catch
                {
                    r = 1f;
                }
                //Debug.Log($"<color=red>[{_data.PlayerData.NamePlayer}]: Decision Value is : " + $"[{r}]: r</color>");
                if (r > 0.6f)
                {
                    try
                    {
                        List<PlayerManagerPun> players = new List<PlayerManagerPun>();
                        players = FindObjectsOfType<PlayerManagerPun>().ToList();
                        if (players != null)
                        {
                            if (CountPlayersInGame(players) == 2)
                            {
                                if (!_data.PlayerData.IsSeenCard)
                                {
                                    //Debug.Log($"[{_data.PlayerData.NamePlayer}]: Decided to see the cards.....2");
                                    if (_data.Network != null)
                                    {
                                        _data.Network.OnSeenCard();
                                    }
                                }
                                if (_data.Network != null)
                                {
                                    _data.Network.OnPlayerSeenCard();
                                }
                            }
                            else
                            {
                                players = players.OrderBy(x => x.GetIdOrderUI()).ToList();
                                if (players.Count > 0)
                                {
                                    var winPlayer = CardCombination.decideWinner(players.ToArray());
                                    if (winPlayer != null)
                                    {
                                        //Debug.Log("winPlayer start before " + winPlayer.playerData.NamePlayer + "r " + r + "winPlayer.playerData._DeviceID "+ winPlayer.playerData._DeviceID + " _data.PlayerData._DeviceID "+ _data.PlayerData._DeviceID);
                                        if (winPlayer.playerData.IsBot)
                                        {
                                            var newr = Random.Range(0f, 1f);
                                            //Debug.Log("newr " + newr);
                                            if (newr < 0.6f)
                                            {
                                                PlayerManagerPun callPreviousPlayer = FindPreviousPlayer(_data.Network);
                                                if (callPreviousPlayer != null)
                                                {
                                                    if (!_data.PlayerData.IsSeenCard)
                                                    {
                                                       // Debug.Log($"[{_data.PlayerData.NamePlayer}]: Decided to see the cards.....3");
                                                        if (_data.Network != null)
                                                        {
                                                            _data.Network.OnSeenCard();
                                                        }
                                                    }
                                                    if (_data.Network != null)
                                                    {
                                                        _data.Network.OnPlayerSeenCard();
                                                    }
                                                }
                                                else
                                                {
                                                    if (_data.Network != null)
                                                    {
                                                        _data.Network.photonView.RPC("PackBot", RpcTarget.AllViaServer);
                                                        _data.Network.photonView.RPC("ProceedPackOnServer", RpcTarget.MasterClient);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                if (_data.Network != null)
                                                {
                                                    _data.Network.photonView.RPC("PackBot", RpcTarget.AllViaServer);
                                                    _data.Network.photonView.RPC("ProceedPackOnServer", RpcTarget.MasterClient);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //Debug.Log("newr else " );
                                            if (_data.Network != null)
                                            {
                                                _data.Network.photonView.RPC("PackBot", RpcTarget.AllViaServer);
                                                _data.Network.photonView.RPC("ProceedPackOnServer", RpcTarget.MasterClient);
                                            }
                                        }
                                    }
                                    else
                                    {
                                       // Debug.Log("newr else else ");
                                        if (_data.Network != null)
                                        {
                                            _data.Network.photonView.RPC("PackBot", RpcTarget.AllViaServer);
                                            _data.Network.photonView.RPC("ProceedPackOnServer", RpcTarget.MasterClient);
                                        }
                                    }
                                }
                               
                            }
                        }
                    }
                    catch
                    {
                       // Debug.Log("newr else else else");
                        if (_data.Network != null)
                        {
                            _data.Network.photonView.RPC("PackBot", RpcTarget.AllViaServer);
                            _data.Network.photonView.RPC("ProceedPackOnServer", RpcTarget.MasterClient);
                        }
                    }
                    yield break;
                }
            }

           

            //Debug.Log($"<color=red>[{_data.PlayerData.NamePlayer}]: Decided to Chaal Value is : " + $"[{_data.PlayerData.step}]: _data.PlayerData.step</color>");
            if (GetValue == 100)
            {
                if (_data.PlayerData.Money >_data.PlayerData.currentBootPlayer)
                {
                    if (_data.Network != null)
                    {
                        _data.Network.photonView.RPC("ChaalPlayer", RpcTarget.AllViaServer);
                    }
                }
                else//Pack Bot
                {
                    if (_data.Network != null)
                    {
                        _data.Network.photonView.RPC("PackBot", RpcTarget.AllViaServer);
                        _data.Network.photonView.RPC("ProceedPackOnServer", RpcTarget.MasterClient);
                    }
                }
            }
            else if (_data.PlayerData.step < GetValue)
            {
                if (_data.PlayerData.currentBootPlayer < _data.PlayerData.Money)
                {
                    var r = Random.Range(0f, 1f);
                   
                    //Debug.Log($"<color=red>[{_data.PlayerData.NamePlayer}]: Value is : " + $"[{r}]: r</color>" + $"[{GetValue}]: GetValue</color>");
                    try
                    {
                        List<PlayerManagerPun> players = new List<PlayerManagerPun>();
                        players = FindObjectsOfType<PlayerManagerPun>().ToList();
                        if (players != null)
                        {
                            players = players.OrderBy(x => x.GetIdOrderUI()).ToList();
                            if (players.Count > 0)
                            {
                                var winPlayer = CardCombination.decideWinner(players.ToArray());
                                if (winPlayer != null)
                                {
                                    //Debug.Log("winPlayer else before " + winPlayer.playerData.NamePlayer + "r " + r);
                                    if (winPlayer.playerData.IsBot)
                                    {
                                        r = 0f;
                                    }
                                    else
                                    {
                                        r = 1f;
                                    }
                                    //Debug.Log("winPlayer else after " + winPlayer.playerData.NamePlayer + "r " + r);
                                }
                            }
                        }
                    }
                    catch
                    {

                    }

                    //Debug.Log("half(GetValue) " + half(GetValue));
                    if (half(GetValue) > 7)
                    {
                        if (_data.PlayerData.step < half(GetValue))
                        {
                            r = 0f;
                        }
                    }
                    else
                    {
                        r = 0f;
                    }
                    if (r < 0.6f)
                    {
                        if (_data.Network != null)
                        {
                            _data.Network.photonView.RPC("ChaalPlayer", RpcTarget.AllViaServer);
                        }
                    }
                    else
                    {
                        r = Random.Range(0f, 1f);
                        if (r < 0.6f)
                        {
                            if (_data.Network != null)
                            {
                                _data.Network.photonView.RPC("PackBot", RpcTarget.AllViaServer);
                                _data.Network.photonView.RPC("ProceedPackOnServer", RpcTarget.MasterClient);
                            }
                        }
                        else
                        {
                            PlayerManagerPun callPreviousPlayer = FindPreviousPlayer(_data.Network);
                            if (callPreviousPlayer != null)
                            {
                                if (!_data.PlayerData.IsSeenCard)
                                {
                                    //Debug.Log($"[{_data.PlayerData.NamePlayer}]: Decided to see the cards.....4");
                                    if (_data.Network != null)
                                    {
                                        _data.Network.OnSeenCard();
                                    }
                                }
                                if (_data.Network != null)
                                {
                                    _data.Network.OnPlayerSeenCard();
                                }
                            }
                            else
                            {
                                if (_data.Network != null)
                                {
                                    _data.Network.photonView.RPC("PackBot", RpcTarget.AllViaServer);
                                    _data.Network.photonView.RPC("ProceedPackOnServer", RpcTarget.MasterClient);
                                }
                            }
                        }
                    }
                }
                else//Pack Bot
                {
                    //Debug.Log($"<color=red>[{_data.PlayerData.NamePlayer}]: Pack to Chaal Value is 1 : " + $"[{_data.PlayerData.step}]: _data.PlayerData.step</color>");
                    if (_data.Network != null)
                    {
                        _data.Network.photonView.RPC("PackBot", RpcTarget.AllViaServer);
                        _data.Network.photonView.RPC("ProceedPackOnServer", RpcTarget.MasterClient);
                    }
                }
            }
            else
            {
                if (_data.PlayerData.currentBootPlayer < _data.PlayerData.Money)
                {
                    //i have to write show condition

                    var newValue = CalculateLogicForBots();

                    //Debug.Log($"<color=red>[{_data.PlayerData.NamePlayer}]: Value is : " + $"[{_data.PlayerData.step}]: _data.PlayerData.step"  +$"[{newValue}]: newValue" + $"[{ GetValue}]: GetValue</color>");
                    if (_data.PlayerData.step == GetValue)
                    {
                        PlayerManagerPun callPreviousPlayer = FindPreviousPlayer(_data.Network);
                        if (callPreviousPlayer != null)
                        {
                            if (!_data.PlayerData.IsSeenCard)
                            {
                                //Debug.Log($"[{_data.PlayerData.NamePlayer}]: Decided to see the cards.....5");
                                if (_data.Network != null)
                                {
                                    _data.Network.OnSeenCard();
                                }
                            }
                            if (_data.Network != null)
                            {
                                _data.Network.OnPlayerSeenCard();
                            }
                        }
                        else
                        {
                            if (_data.Network != null)
                            {
                                _data.Network.photonView.RPC("PackBot", RpcTarget.AllViaServer);
                                _data.Network.photonView.RPC("ProceedPackOnServer", RpcTarget.MasterClient);
                            }
                        }
                    }
                    else if (_data.PlayerData.step >= 5 && newValue == 50 && !isIAmWinner)//Pack bot means user dont have higher value than jack
                    {
                        //Debug.Log($"<color=red>[{_data.PlayerData.NamePlayer}]: Pack to Chaal Value is 2 : " + $"[{_data.PlayerData.step}]: _data.PlayerData.step</color>");
                        if (_data.Network != null)
                        {
                            _data.Network.photonView.RPC("PackBot", RpcTarget.AllViaServer);
                            _data.Network.photonView.RPC("ProceedPackOnServer", RpcTarget.MasterClient);
                        }
                    }
                    else if(_data.PlayerData.step >= GetValue && _data.PlayerData.step < 45)
                    {
                        if((GetValue + 2) == _data.PlayerData.step && GetValue>0)
                        {
                            PlayerManagerPun callPreviousPlayer = FindPreviousPlayer(_data.Network);
                            if (callPreviousPlayer != null)
                            {
                                if (!_data.PlayerData.IsSeenCard)
                                {
                                    //Debug.Log($"[{_data.PlayerData.NamePlayer}]: Decided to see the cards.....6");
                                    if (_data.Network != null)
                                    {
                                        _data.Network.OnSeenCard();
                                    }
                                }
                                if (_data.Network != null)
                                {
                                    _data.Network.OnPlayerSeenCard();
                                }
                            }
                            else
                            {
                                if (_data.Network != null)
                                {
                                    _data.Network.photonView.RPC("PackBot", RpcTarget.AllViaServer);
                                    _data.Network.photonView.RPC("ProceedPackOnServer", RpcTarget.MasterClient);
                                }
                            }
                        }
                        else if(!isIAmWinner && (newValue == 5 || newValue == 50))//Pack bot
                        {
                            //Debug.Log($"<color=red>[{_data.PlayerData.NamePlayer}]: Pack to Chaal Value is 3 : " + $"[{_data.PlayerData.step}]: _data.PlayerData.step</color>");
                            if (_data.Network != null)
                            {
                                _data.Network.photonView.RPC("PackBot", RpcTarget.AllViaServer);
                                _data.Network.photonView.RPC("ProceedPackOnServer", RpcTarget.MasterClient);
                            }
                        }
                        else
                        {
                            var r = Random.Range(0f, 1f);
                            //Debug.Log($"<color=red>[{_data.PlayerData.NamePlayer}]: Value is : " + $"[{r}]: r</color>" + $"[{newValue}]: newValue</color>");
                            try
                            {
                                List<PlayerManagerPun> players = new List<PlayerManagerPun>();
                                players = FindObjectsOfType<PlayerManagerPun>().ToList();
                                if (players != null)
                                {
                                    players = players.OrderBy(x => x.GetIdOrderUI()).ToList();
                                    if (players.Count > 0)
                                    {
                                        var winPlayer = CardCombination.decideWinner(players.ToArray());
                                        if (winPlayer != null)
                                        {
                                            //Debug.Log("winPlayer else before " + winPlayer.playerData.NamePlayer + "r " + r);
                                            if (winPlayer.playerData._DeviceID == _data.PlayerData._DeviceID)
                                            {
                                                r = 0f;
                                            }
                                            else
                                            {
                                                r = 1f;
                                            }
                                            //Debug.Log("winPlayer else after " + winPlayer.playerData.NamePlayer + "r " + r);
                                        }
                                    }
                                }
                            }
                            catch
                            {

                            }
                            if (r < 0.6f)
                            {
                                if (_data.Network != null)
                                {
                                    _data.Network.photonView.RPC("ChaalPlayer", RpcTarget.AllViaServer);
                                }
                            }
                            else
                            {
                                PlayerManagerPun callPreviousPlayer = FindPreviousPlayer(_data.Network);
                                if (callPreviousPlayer != null)
                                {
                                    if (!_data.PlayerData.IsSeenCard)
                                    {
                                        //Debug.Log($"[{_data.PlayerData.NamePlayer}]: Decided to see the cards.....7");
                                        if (_data.Network != null)
                                        {
                                            _data.Network.OnSeenCard();
                                        }
                                    }
                                    if (_data.Network != null)
                                    {
                                        _data.Network.OnPlayerSeenCard();
                                    }
                                }
                                else
                                {
                                    if (_data.Network != null)
                                    {
                                        _data.Network.photonView.RPC("PackBot", RpcTarget.AllViaServer);
                                        _data.Network.photonView.RPC("ProceedPackOnServer", RpcTarget.MasterClient);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        var r = Random.Range(0f, 1f);
                        //Debug.Log($"<color=red>[{_data.PlayerData.NamePlayer}]: Value is : " + $"[{r}]: r.............</color>" + $"[{newValue}]: newValue</color>");
                        try
                        {
                            List<PlayerManagerPun> players = new List<PlayerManagerPun>();
                            players = FindObjectsOfType<PlayerManagerPun>().ToList();
                            if (players != null)
                            {
                                players = players.OrderBy(x => x.GetIdOrderUI()).ToList();
                                if (players.Count > 0)
                                {
                                    var winPlayer = CardCombination.decideWinner(players.ToArray());
                                    if (winPlayer != null)
                                    {
                                        //Debug.Log("winPlayer else before " + winPlayer.playerData.NamePlayer + "r " + r);
                                        if (winPlayer.playerData._DeviceID == _data.PlayerData._DeviceID)
                                        {
                                            r = 0f;
                                        }
                                        else
                                        {
                                            r = 1f;
                                        }
                                       // Debug.Log("winPlayer else after " + winPlayer.playerData.NamePlayer + "r " + r);
                                    }
                                }
                            }
                        }
                        catch
                        {

                        }
                        //Debug.Log($"<color=red>[{_data.PlayerData.NamePlayer}]: Value is : " + $"[{r}]: rwww.............</color>" + $"[{newValue}]: newValue</color>");
                        if (r < 0.6f)
                        {
                            if (_data.Network != null)
                            {
                                _data.Network.photonView.RPC("ChaalPlayer", RpcTarget.AllViaServer);
                            }
                        }
                        else
                        {
                            r = Random.Range(0f, 1f);
                            if (r < 0.6f)
                            {

                                PlayerManagerPun callPreviousPlayer = FindPreviousPlayer(_data.Network);
                                if (callPreviousPlayer != null)
                                {
                                    if (!_data.PlayerData.IsSeenCard)
                                    {
                                        //Debug.Log($"[{_data.PlayerData.NamePlayer}]: Decided to see the cards.....8");
                                        if (_data.Network != null)
                                        {
                                            _data.Network.OnSeenCard();
                                        }
                                    }
                                    if (_data.Network != null)
                                    {
                                        _data.Network.OnPlayerSeenCard();
                                    }
                                }
                                else
                                {
                                    if (_data.Network != null)
                                    {
                                        _data.Network.photonView.RPC("PackBot", RpcTarget.AllViaServer);
                                        _data.Network.photonView.RPC("ProceedPackOnServer", RpcTarget.MasterClient);
                                    }
                                }
                            }
                            else
                            {
                                if (_data.Network != null)
                                {
                                    _data.Network.photonView.RPC("PackBot", RpcTarget.AllViaServer);
                                    _data.Network.photonView.RPC("ProceedPackOnServer", RpcTarget.MasterClient);
                                }
                            }
                        }
                    }
                }
                else//Pack Bot
                {
                    //Debug.Log($"<color=red>[{_data.PlayerData.NamePlayer}]: Pack to Chaal Value is 4 : " + $"[{_data.PlayerData.step}]: _data.PlayerData.step</color>");
                    if (_data.Network != null)
                    {
                        _data.Network.photonView.RPC("PackBot", RpcTarget.AllViaServer);
                        _data.Network.photonView.RPC("ProceedPackOnServer", RpcTarget.MasterClient);
                    }
                }
            }
        }
    }
    private PlayerManagerPun FindPreviousPlayer(PlayerManagerPun firstPlayer)
    {
        List<PlayerManagerPun> players = new List<PlayerManagerPun>();
        players = FindObjectsOfType<PlayerManagerPun>().ToList();
        if (players != null)
        {
            players = players.OrderBy(x => x.GetIdOrderUI()).ToList();
            if (players.Count > 0)
            {
                int previousPlayer = players.FindIndex(a => a.playerData._DeviceID.Equals(firstPlayer.playerData._DeviceID));
                //////Debug.Log("previousPlayer " + previousPlayer);
                for (int i = 0; i < players.Count; i++)
                {
                    previousPlayer--;

                    if (previousPlayer < 0)
                        previousPlayer = players.Count - 1;

                    if (previousPlayer >= 0 && previousPlayer < players.Count)
                    {
                        if (players[previousPlayer] != null)
                        {
                            if (players[previousPlayer].playerData.playerType == ePlayerType.PlayerStartGame && !players[previousPlayer].playerData.IsPacked)
                            {
                                //////Debug.Log("previousPlayer " + previousPlayer + "players[previousPlayer] "+ players[previousPlayer].myUI.MyPositionID);
                                if (players[previousPlayer].playerData.IsSeenCard)
                                {
                                    int PlayerIndexCheck = players.FindIndex(a => a.playerData._DeviceID.Equals(firstPlayer.playerData._DeviceID));
                                    if (PlayerIndexCheck != previousPlayer)
                                    {
                                        return players[previousPlayer];
                                    }
                                    else
                                    {
                                        return null;
                                    }
                                }
                                else if (!players[previousPlayer].playerData.IsSeenCard && players[previousPlayer].playerData.IsBot)
                                {
                                    int PlayerIndexCheck = players.FindIndex(a => a.playerData._DeviceID.Equals(firstPlayer.playerData._DeviceID));
                                    if (PlayerIndexCheck != previousPlayer)
                                    {
                                        return players[previousPlayer];
                                    }
                                    else
                                    {
                                        return null;
                                    }

                                }
                                else
                                {
                                    return null;
                                }

                            }
                        }
                    }
                }
            }
        }

        return null;
    }
    private void test()
    {
        try
        {
            List<PlayerManagerPun> players = new List<PlayerManagerPun>();
            players = FindObjectsOfType<PlayerManagerPun>().ToList();
            if (players != null)
            {
                players = players.OrderBy(x => x.GetIdOrderUI()).ToList();
                if (players.Count > 0)
                {
                    var winPlayer = CardCombination.decideWinner(players.ToArray());
                    if (winPlayer != null)
                    {
                        if (winPlayer.playerData.IsBot)
                        {
                            //isIAnyBotWinner = true;
                        }
                    }
                }
            }
        }
        catch
        {

        }
    }
    private double half(double number)
    {
        if (number > 0f)
        {
            number = number / 2.0f;
        }
        return number;
    }
    public void OnReceiveSideShow(int id, PlayerManagerPun network, PlayerData data, PlayerUI ui,
        TeenPattiPhoton main, TeenPatiHUD hud, TableInfo table)
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            if (isDebug)
            {
                Debug.LogWarning("Only run bot system in MasterClient");

            }
            return;
        }
        if (isDebug)
        {
            Debug.Log($"[{data.NamePlayer}]: OnReceiveRequestSideShow");
        }

        _data = new Data
        {
            Id = id,
            Network = network,
            PlayerData = data,
            PlayerUI = ui,
            Main = main,
            HUD = hud,
            Table = table
        };

        try
        {
            if (_data.Network != null)
            {
                if (_data.Main.typeTable != eTable.PotBlind)
                {
                    if (!_data.PlayerData.IsSeenCard)
                    {

                        //Debug.Log($"[{_data.PlayerData.NamePlayer}]: Decided to see the cards.....9");
                        if (_data.Network != null)
                        {
                            _data.Network.OnSeenCard();
                        }

                    }
                }
            }
        }
        catch
        {

        }
        var sleepTime = Random.Range(2f, 5f);// 10f, 15f);
        StopCoroutine("_CalculateSideShow");
        StartCoroutine("_CalculateSideShow", sleepTime);
        
    }
    private IEnumerator _CalculateSideShow(float time)
    {
           
            if (_data.PlayerData.IsPacked) yield break;

            if (isDebug)
            {
                Debug.Log($"[{_data.PlayerData.NamePlayer}]: Wait for SideShow {time}s");
            }
            yield return new WaitForSeconds(time);

            var GetValueSS = _data.PlayerData._RandNext;// RecievedSideShowLogicBots();// Random.Range(0, 1.0f);
            var r = Random.Range(0, 10);
            //Debug.Log("r   ...." + r);
            //Debug.Log((GetValueSS != 100 || _data.PlayerData.step >= GetValueSS) ? $"[{_data.PlayerData.NamePlayer}]: Accept SideShow" : $"[{_data.PlayerData.NamePlayer}]: Decline SideShow");
            if (GetValueSS == 100)
            {
                if (r < 5)
                {
                    _data.Network.photonView.RPC("DeclineSideShow", RpcTarget.All);
                    _data.Network.photonView.RPC("DeclineSideShowOnlyMaster", RpcTarget.MasterClient);
                }
                else
                {
                    _data.Network.photonView.RPC("AcceptSideShow", RpcTarget.All);
                    _data.Network.photonView.RPC("AcceptSideShowOnlyMaster", RpcTarget.MasterClient);
                }
            }
            else if (_data.PlayerData.step >= GetValueSS)
            {
                _data.Network.photonView.RPC("AcceptSideShow", RpcTarget.All);
                _data.Network.photonView.RPC("AcceptSideShowOnlyMaster", RpcTarget.MasterClient);
            }
            else
            {
                if (r < 5)
                {
                    _data.Network.photonView.RPC("DeclineSideShow", RpcTarget.All);
                    _data.Network.photonView.RPC("DeclineSideShowOnlyMaster", RpcTarget.MasterClient);
                }
                else
                {
                    _data.Network.photonView.RPC("AcceptSideShow", RpcTarget.All);
                    _data.Network.photonView.RPC("AcceptSideShowOnlyMaster", RpcTarget.MasterClient);
                }
            }
            //Debug.Log(r <= 30 ? $"[{_data.PlayerData.NamePlayer}]: Accept SideShow" : $"[{_data.PlayerData.NamePlayer}]: Decline SideShow");
            //_data.Network.photonView.RPC(r <= 30 ? "AcceptSideShow" : "DeclineSideShow", RpcTarget.All);
            //_data.Network.photonView.RPC(r <= 30 ? "AcceptSideShowOnlyMaster" : "DeclineSideShowOnlyMaster", RpcTarget.MasterClient);
    }
    private int CalculateLogicForBots()
    {
        int rankCard_1 = 0, rankCard_2 = 0, rankCard_3 = 0;
        int suitCard_1 = 0, suitCard_2 = 0, suitCard_3 = 0;
        rankCard_1 = _data.PlayerData.currentCards[0].rankCard;
        rankCard_2 = _data.PlayerData.currentCards[1].rankCard;
        rankCard_3 = _data.PlayerData.currentCards[2].rankCard;
        suitCard_1 = (int)_data.PlayerData.currentCards[0].suitCard;
        suitCard_2 = (int)_data.PlayerData.currentCards[1].suitCard;
        suitCard_3 = (int)_data.PlayerData.currentCards[2].suitCard;
      
        string priority_game = _data.PlayerData.currentCombination.ToString();

       if (priority_game == "Pair")
        {
            return Random.Range(7, 15);// " Give me chaal number in range of 7-15 Show"
        }
        else if (priority_game == "Color")
        {
            return Random.Range(12, 20);// " Give me chaal number in range of 12-20 Show"
        }
        else if (priority_game == "Sequence")
        {
            return Random.Range(15, 22);// " Give me chaal number in range of 15-22 Show"
        }
        else if (priority_game == "PureSequence")
        {
            return Random.Range(20, 30);// " Give me chaal number in range of 20-30 with Show"
        }
        else if (priority_game == "Trail")
        {
            return 100;// "Continuous playing without pack and without getting any chaal number for show Don't Show"
        }
        else if (rankCard_1 <= 11 && rankCard_2 <= 11 && rankCard_3 <= 11)
        {
            return 50;// "Pack"
        }
        else if (rankCard_1 > 11 || rankCard_2 > 11 || rankCard_3 > 11)
        {
            return 5;// " Give me chaal number in range of 3-7 Show"
        }
      
        return 0;//Pack
    }
    private int RecievedSideShowLogicBots()
    {
        int rankCard_1 = 0, rankCard_2 = 0, rankCard_3 = 0;
        int suitCard_1 = 0, suitCard_2 = 0, suitCard_3 = 0;
        rankCard_1 = _data.PlayerData.currentCards[0].rankCard;
        rankCard_2 = _data.PlayerData.currentCards[1].rankCard;
        rankCard_3 = _data.PlayerData.currentCards[2].rankCard;
        suitCard_1 = (int)_data.PlayerData.currentCards[0].suitCard;
        suitCard_2 = (int)_data.PlayerData.currentCards[1].suitCard;
        suitCard_3 = (int)_data.PlayerData.currentCards[2].suitCard;

        string priority_game = _data.PlayerData.currentCombination.ToString();

        if (priority_game == "Pair")
        {

            return _data.PlayerData._RandNext;// Random.Range(7, 15);// "Accept Side Show"
        }
        else if (priority_game == "Color")
        {
            return _data.PlayerData._RandNext;// Random.Range(12, 20);// "Accept Side Show"
        }
        else if (priority_game == "Sequence")
        {
            return _data.PlayerData._RandNext;// Random.Range(15, 22);// "Accept Side Show"
        }
        else if (priority_game == "PureSequence")
        {
            return _data.PlayerData._RandNext;// Random.Range(20, 30);// "Accept Side Show"
        }
        else if (priority_game == "Trail")
        {
            return 100;// "Decline Side Show"
        }
        else if(rankCard_1 <= 11 && rankCard_2 <= 11 && rankCard_3 <= 11)
        {
            return _data.PlayerData._RandNext; //0;// "Accept Side Show"
        }
        else if (rankCard_1 > 11 || rankCard_2 > 11 || rankCard_3 > 11)
        {
            return _data.PlayerData._RandNext; //0;// "Accept Side Show"
        }
        
        return 0; //"Accept Side Show"
    }
}