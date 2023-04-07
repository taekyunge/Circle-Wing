using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Ranking : Popup
{
    [SerializeField] private Text messageText;

    public override void Open(object data = null)
    {
        base.Open();

        SoundMgr.Instance.Play(SoundType.RESULT);

        if (GameMgr.Instance == null)
        {
            PopupMgr.Instance.Close(popupType);
            return;
        }
        string resultStr = string.Empty;

        for (int i = 0; i < GameMgr.Instance.players.Count; i++)
        {
            var player = GameMgr.Instance.players[i];

            if (player.finish)
                resultStr += string.Format("{0} : {1} ({2}m)\n", i + 1, player.nickname, player.distance);
            else
                resultStr += string.Format("{0} : {1} (Retire)\n", i + 1, player.nickname);
        }

        messageText.text = resultStr;
    }

    public override void Close(object data = null)
    {
        base.Close();
    }

    public void OnClickEnter()
    {
        SoundMgr.Instance.Play(SoundType.BUTTON);
        PopupMgr.Instance.Close(popupType);
        PhotonNetwork.LoadLevel(0);
    }
}
