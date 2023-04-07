using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMgr : MonoBehaviourPunCallbacks
{
    public static NetworkMgr instance;

    private ConnectType connectType;
    private string roomName;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void OnApplicationQuit()
    {
        Disconnect();
    }

    public void CreateRoom(string roomName)
    {
        PopupMgr.Instance.Open(PopupType.LOADING);

        this.roomName = roomName;

        connectType = ConnectType.CREATE_ROOM;
        PhotonNetwork.GameVersion = Application.version;
        PhotonNetwork.NickName = Data.nickName;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void JoinRoom(string roomName)
    {
        PopupMgr.Instance.Open(PopupType.LOADING);

        this.roomName = roomName;

        connectType = ConnectType.JOIN;
        PhotonNetwork.GameVersion = Application.version;
        PhotonNetwork.NickName = Data.nickName;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void JoinRandomRoom()
    {
        PopupMgr.Instance.Open(PopupType.LOADING);

        connectType = ConnectType.RANDOM_JOIN;
        PhotonNetwork.GameVersion = Application.version;
        PhotonNetwork.NickName = Data.nickName;
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();

        Debug.Log("OnCreatedRoom");
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();

        Debug.Log("OnConnectedToMaster");

        switch (connectType)
        {
            case ConnectType.CREATE_ROOM:
                PhotonNetwork.CreateRoom(roomName, new RoomOptions { MaxPlayers = Data.maxPlayers });
                break;

            case ConnectType.RANDOM_JOIN:
                PhotonNetwork.JoinRandomRoom();
                break;

            case ConnectType.JOIN:
                PhotonNetwork.JoinRoom(roomName);
                break;

            default:
                break;
        }
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        Debug.Log("OnDisconnected");
    }

    public override void OnConnected()
    {
        base.OnConnected();

        Debug.Log("OnConnected");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);

        Debug.LogFormat("OnCreateRoomFailed : {0}({1})", message, returnCode);

        Disconnect();

        PopupMgr.Instance.Close(PopupType.LOADING);
        PopupMgr.Instance.Open(PopupType.MESSAGE, "방 생성 실패");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);

        Debug.LogFormat("OnJoinRandomFailed : {0}({1})", message, returnCode);

        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = Data.maxPlayers });
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);

        Debug.LogFormat("OnJoinRoomFailed : {0}({1})", message, returnCode);

        Disconnect();

        PopupMgr.Instance.Close(PopupType.LOADING);
        PopupMgr.Instance.Open(PopupType.MESSAGE, "방 입장 실패");
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        Debug.LogFormat("OnJoinedRoom : {0}", PhotonNetwork.PlayerList.Length);

        PopupMgr.Instance.Close(PopupType.LOADING);
        PopupMgr.Instance.Open(PopupType.WAIT_PLAYER);
    }
}
