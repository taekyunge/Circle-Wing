using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : Popup
{
    [SerializeField] private Text messageText; 

    public override void Open(object data = null)
    {
        base.Open();

        messageText.text = data.ToString();
    }

    public override void Close(object data = null)
    {
        base.Close();
    }

    public void OnClickEnter()
    {
        SoundMgr.Instance.Play(SoundType.BUTTON);
        PopupMgr.Instance.Close(popupType);
    }
}
