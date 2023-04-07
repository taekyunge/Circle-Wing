using ExitGames.Client.Photon;
using Mono.Cecil.Cil;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class GameMgr : Singleton<GameMgr>, IOnEventCallback
{
    public bool GameStart = false;

    public Player myPlayer = null;
    public List<Player> players = new List<Player>();
    public float playTime;

    [SerializeField] private PlayerStatus[] playerStatus;
    [SerializeField] private Transform[] spawnPoins;
    [SerializeField] private Text countText;
    [SerializeField] private Text timeText;
    [SerializeField] private Gradient gradient;
    [SerializeField] private SpriteRenderer spriteRenderer;

    private int masterNumber = -1;
    private float width;

    // 게임 서버
    private GameServer gameServer;

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < playerStatus.Length; i++)
        {
            playerStatus[i].gameObject.SetActive(false);
        }

        countText.gameObject.SetActive(false);

        width = spriteRenderer.size.x;
    }

    private void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.AddCallbackTarget(this);

            if (PhotonNetwork.IsMasterClient)
                gameServer = gameObject.AddComponent<GameServer>();

            CreatePlayer();
        }
    }

    private void Update()
    {
        if (GameStart)
        {
            playTime += Time.deltaTime;

            timeText.text = string.Format("{0}", System.TimeSpan.FromSeconds(playTime).ToString(@"mm\:ss"));
        }

        if (myPlayer != null)
        {
            spriteRenderer.color = gradient.Evaluate(myPlayer.distance / width);

            if (myPlayer.health <= 0 || myPlayer.finish)
            {
                players.Sort((x, y) => y.distance.CompareTo(x.distance));

                Transform target = null;

                for (int i = 0; i < players.Count; i++)
                {
                    var player = players[i];

                    if (player == null || player == myPlayer || player.health <= 0 || player.finish)
                        continue;

                    target = player.transform;
                    break;
                }

                if(target != null)
                    GameCamera.main.SetTarget(target);
            }
            else
            {
                GameCamera.main.SetTarget(myPlayer.transform);
            }
        }
    }

    private void OnDestroy()
    {
        if (PhotonNetwork.IsConnected)
            PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void CreatePlayer()
    {
        masterNumber = PhotonNetwork.MasterClient.ActorNumber;

        int number = PhotonNetwork.LocalPlayer.ActorNumber;
        string nickname = PhotonNetwork.LocalPlayer.NickName;
        int index = number - 1;

        object[] objects = new object[5];

        objects[0] = number;
        objects[1] = nickname;
        objects[2] = Data.r;
        objects[3] = Data.g;
        objects[4] = Data.b;

        PhotonNetwork.Instantiate("Player", spawnPoins[index].position, Quaternion.identity, 0, objects);

        SendEvent(NetworkProtocol.SERVER_ENTER_PLAYER, null, ReceiverGroup.MasterClient);
    }

    public void AddPlayer(Player player)
    {
        players.Add(player);
    }

    public Player GetPlayer(int number)
    {
        return players.Find(x => x.number == number);
    }

    public PlayerStatus GetStatus(int index)
    {
        return playerStatus[index];
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if(eventCode < 100)
        {
            if(gameServer != null)
                gameServer.OnEvent(photonEvent);
        }
        else
        {
            switch ((NetworkProtocol)eventCode)
            {
                case NetworkProtocol.LEAVE:
                    {
                        if(photonEvent.Sender == masterNumber)
                        {
                            PopupMgr.Instance.Open(PopupType.MESSAGE, "호스트와의 연결이 끊겼습니다");
                        }
                    }
                    break;

                case NetworkProtocol.START_COUNT:
                    {
                        var count = (int)photonEvent.CustomData;

                        countText.gameObject.SetActive(true);

                        switch (count)
                        {
                            case 0:
                                countText.gameObject.SetActive(false);
                                GameStart = true;
                                break;

                            case 1:
                                countText.text = "GO!!";
                                break;

                            default:
                                countText.text = "READY";
                                break;
                        }
                    }
                    break;

                case NetworkProtocol.FINISH_COUNT:
                    {
                        var count = (int)photonEvent.CustomData;

                        countText.text = count.ToString();

                        switch (count)
                        {
                            case 0:
                                countText.gameObject.SetActive(false);
                                break;

                            default:
                                countText.gameObject.SetActive(true);
                                break;
                        }
                    }
                    break;

                case NetworkProtocol.FINISH_PLAYER:
                    {
                        var number = (int)photonEvent.CustomData;
                        var player = GetPlayer(number);

                        if(player != null)
                        {
                            player.finish = true;
                            player.gameObject.SetActive(false);
                        }
                    }
                    break;

                case NetworkProtocol.DEATH_PLAYER:
                    {
                        var number = (int)photonEvent.CustomData;
                        var player = GetPlayer(number);

                        if (player != null)
                        {
                            player.health = 0;
                            player.gameObject.SetActive(false);
                        }
                    }
                    break;

                case NetworkProtocol.END_GAME:
                    {
                        players.Sort((x, y) => y.distance.CompareTo(x.distance));

                        PopupMgr.Instance.Open(PopupType.RANKING);
                    }
                    break;
            }
        }
    }

    public void SendEvent(NetworkProtocol protocol, object obj, ReceiverGroup receiverGroup)
    {
        PhotonNetwork.RaiseEvent((byte)protocol, obj, new RaiseEventOptions { Receivers = receiverGroup }, SendOptions.SendReliable);
    }

    public void OnClickBack()
    {
        SoundMgr.Instance.Play(SoundType.BUTTON);

        NetworkMgr.instance.Disconnect();
        PhotonNetwork.LoadLevel(0);
    }
}
