using ExitGames.Client.Photon;
using ExitGames.Client.Photon.StructWrapping;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IPunObservable, IPunInstantiateMagicCallback
{    
    [SerializeField] private float moveSpeed;
    [SerializeField] private GaugeBar gaugeBar;
    [SerializeField] private TextMesh textMesh;
    
    private SpriteRenderer spriteRenderer;
    private new Rigidbody2D rigidbody;
    private PhotonView photonView;
    private PlayerStatus playerStatus;

    public int number;
    public string nickname;
    public Color color;
    public float health = 100;
    public float distance;
    public bool finish = false;

    public bool IsMine { get { return photonView.IsMine; } }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        photonView = GetComponent<PhotonView>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            GameMgr.Instance.myPlayer = this;
        }

        GameMgr.Instance.AddPlayer(this);
    }

    private void Update()
    {
        gaugeBar.value = health;
        distance = transform.position.x;
    }

    private void FixedUpdate()
    {
        if (GameMgr.Instance.GameStart)
        {
            if (photonView.IsMine)
                Move();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Finish")
        {
            if (photonView.IsMine)
            {
                SendEvent(NetworkProtocol.SERVER_FINISH_GAME, number, ReceiverGroup.MasterClient);

                gameObject.SetActive(false);
            }
        }
    }

    private void Move()
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

            pos.Normalize();

            rigidbody.velocity += pos * moveSpeed * Time.fixedDeltaTime;
        }
#else
        if (Input.touchCount > 0)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position) - transform.position;

            pos.Normalize();

            rigidbody.velocity += pos * moveSpeed * Time.fixedDeltaTime;
        }
#endif
    }

    public void Hit(float damage)
    {
        health = Mathf.Clamp(health - damage, 0, 100);

        if (photonView.IsMine)
        {
            SoundMgr.Instance.Play(SoundType.HIT);

            if (health <= 0)
            {
                SendEvent(NetworkProtocol.SERVER_DEATH_PLAYER, number, ReceiverGroup.MasterClient);

                gameObject.SetActive(false);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
            stream.SendNext(distance);
        }
        else
        {
            health = (float)stream.ReceiveNext();
            distance = (float)stream.ReceiveNext();
        }
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] data = info.photonView.InstantiationData;

        number = (int)data[0];
        nickname = (string)data[1];
        color = new Color((float)data[2], (float)data[3], (float)data[4]);

        playerStatus = GameMgr.Instance.GetStatus(number - 1);
        playerStatus.SetPlayer(this);
        playerStatus.gameObject.SetActive(true);

        textMesh.text = nickname;
        spriteRenderer.color = color;
    }

    public void SendEvent(NetworkProtocol protocol, object obj, ReceiverGroup receiverGroup)
    {
        PhotonNetwork.RaiseEvent((byte)protocol, obj, new RaiseEventOptions { Receivers = receiverGroup }, SendOptions.SendReliable);
    }
}
