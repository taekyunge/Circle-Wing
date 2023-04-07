using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleMgr : Singleton<TitleMgr>
{
    [SerializeField] private InputField inputName;

    private void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        if (PlayerPrefs.HasKey("NickName"))
        {
            inputName.text = PlayerPrefs.GetString("NickName");
        }

        NetworkMgr.instance.Disconnect();
    }

    public void OnChangeToggle(Image image)
    {
        Data.r = image.color.r;
        Data.g = image.color.g;
        Data.b = image.color.b; 
        
        SoundMgr.Instance.Play(SoundType.BUTTON);
    }

    public void OnClickCreateRoom()
    {
        SoundMgr.Instance.Play(SoundType.BUTTON);

        if (string.IsNullOrEmpty(inputName.text))
        {
            PopupMgr.Instance.Open(PopupType.MESSAGE, "닉네임을 입력해주세요");
            return;
        }

        PlayerPrefs.SetString("NickName", inputName.text);
        Data.nickName = inputName.text;
        PopupMgr.Instance.Open(PopupType.CREATE_ROOM);
    }

    public void OnClickJoinRandomRoom()
    {
        SoundMgr.Instance.Play(SoundType.BUTTON);

        if (string.IsNullOrEmpty(inputName.text))
        {
            PopupMgr.Instance.Open(PopupType.MESSAGE, "닉네임을 입력해주세요");
            return;
        }

        PlayerPrefs.SetString("NickName", inputName.text);
        Data.nickName = inputName.text;
        NetworkMgr.instance.JoinRandomRoom();
    }

    public void OnClickJoinRoom()
    {
        SoundMgr.Instance.Play(SoundType.BUTTON);

        if (string.IsNullOrEmpty(inputName.text))
        {
            PopupMgr.Instance.Open(PopupType.MESSAGE, "닉네임을 입력해주세요");
            return;
        }

        PlayerPrefs.SetString("NickName", inputName.text);
        Data.nickName = inputName.text;
        PopupMgr.Instance.Open(PopupType.JOIN_ROOM);
    }

    public void OnClickGameClose()
    {
        SoundMgr.Instance.Play(SoundType.BUTTON);

        Application.Quit();
    }
}
