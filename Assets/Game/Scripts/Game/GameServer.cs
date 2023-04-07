using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameServer : MonoBehaviour
{
    public int playerCount;
    public int deathCount;
    public int maxPlayers;
    private bool finish = false;

    private void Start()
    {
        maxPlayers = PhotonNetwork.CurrentRoom.MaxPlayers;
    }

    public void EnterPlayer()
    {
        if (++playerCount == maxPlayers)
        {
            StartGame();
        }
    }

    private void StartGame()
    {
        StartCoroutine(StartCount());
    }

    private void StartFinishCount()
    {
        StartCoroutine(FinishCount());
    }

    private IEnumerator StartCount()
    {
        for (int i = 3; i >= 0; i--)
        {
            SendEvent(NetworkProtocol.START_COUNT, i, ReceiverGroup.All);
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator FinishCount()
    {
        for (int i = 10; i >= 1; i--)
        {
            SendEvent(NetworkProtocol.FINISH_COUNT, i, ReceiverGroup.All);
            yield return new WaitForSeconds(1);
        }

        EndGame();
    }

    private void EndGame()
    {
        SendEvent(NetworkProtocol.END_GAME, null, ReceiverGroup.All);
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        switch ((NetworkProtocol)eventCode)
        {
            case NetworkProtocol.SERVER_ENTER_PLAYER:
                {
                    EnterPlayer();
                }
                break;

            case NetworkProtocol.SERVER_FINISH_GAME:
                {
                    var number = (int)photonEvent.CustomData;

                    if (!finish)
                        StartFinishCount();

                    SendEvent(NetworkProtocol.FINISH_PLAYER, number, ReceiverGroup.All);
                }
                break;

            case NetworkProtocol.SERVER_DEATH_PLAYER:
                {
                    var number = (int)photonEvent.CustomData;

                    SendEvent(NetworkProtocol.DEATH_PLAYER, number, ReceiverGroup.All);

                    if (++deathCount == maxPlayers)
                    {
                        EndGame();
                    }
                }
                break;
        }
    }

    public void SendEvent(NetworkProtocol protocol, object obj, ReceiverGroup receiverGroup)
    {
        PhotonNetwork.RaiseEvent((byte)protocol, obj, new RaiseEventOptions { Receivers = receiverGroup }, SendOptions.SendReliable);
    }
}
