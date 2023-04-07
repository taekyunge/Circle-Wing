using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupMgr : Singleton<PopupMgr>
{
    private Dictionary<PopupType, Popup> popups = new Dictionary<PopupType, Popup>();

    protected override void Awake()
    {
        base.Awake();

        var popups = GetComponentsInChildren<Popup>(true);

        for (int i = 0; i < popups.Length; i++)
        {
            var popup = popups[i];

            this.popups[popup.popupType] = popup;
        }
    }

    public void Open(PopupType popupType, object data = null)
    {
        if (!popups.ContainsKey(popupType))
            return;

        var popup = popups[popupType];

        popup.gameObject.SetActive(true);
        popup.Open(data);
    }

    public void Close(PopupType popupType, object data = null)
    {
        if (!popups.ContainsKey(popupType))
            return;

        var popup = popups[popupType];

        if(popup.IsOpen)
        {
            popup.Close(data);
            popup.gameObject.SetActive(false);
        }
    }

    public void CloseAll()
    {
        foreach (var popup in popups.Values)
        {
            if (popup.IsOpen)
            {
                popup.Close();
                popup.gameObject.SetActive(false);
            }
        }
    }
}
