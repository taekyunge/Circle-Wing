using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    public PopupType popupType;

    public bool IsOpen;

    public virtual void Open(object data = null)
    {
        IsOpen = true;
    }

    public virtual void Close(object data = null)
    {
        IsOpen = false;
    }
}
