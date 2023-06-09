using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoom : Popup
{
    [SerializeField] private InputField inputName;

    public override void Open(object data = null)
    {
        base.Open();

        inputName.text = string.Empty;
    }

    public override void Close(object data = null)
    {
        base.Close();
    }

    public void OnClickJoin()
    {
        SoundMgr.Instance.Play(SoundType.BUTTON);

        if (string.IsNullOrEmpty(inputName.text))
        {
            PopupMgr.Instance.Open(PopupType.MESSAGE, "룸 이름을 입력해 주세요");
        }
        else
        {
            NetworkMgr.instance.JoinRoom(inputName.text);

            PopupMgr.Instance.Close(popupType);
        }
    }

    public void OnClickClose()
    {
        SoundMgr.Instance.Play(SoundType.BUTTON);
        PopupMgr.Instance.Close(popupType);
    }
}
