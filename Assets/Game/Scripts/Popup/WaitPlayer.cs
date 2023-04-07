using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitPlayer : Popup
{
    [SerializeField] private Image[] playerImages;

    private void Update()
    {
        if (PhotonNetwork.CurrentRoom == null)
            return;

        for (int i = 0; i < playerImages.Length; i++)
        {
            var image = playerImages[i];

            image.color = (i < PhotonNetwork.CurrentRoom.PlayerCount) ? Color.green : Color.white;
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount >= Data.maxPlayers)
        {
            PhotonNetwork.LoadLevel(1);
            PopupMgr.Instance.CloseAll();
        }
    }

    public override void Open(object data = null)
    {
        base.Open();
    }

    public override void Close(object data = null)
    {
        base.Close();
    }

    public void OnClickCancel()
    {
        SoundMgr.Instance.Play(SoundType.BUTTON);
        PopupMgr.Instance.Close(popupType);

        NetworkMgr.instance.Disconnect();
    }
}
