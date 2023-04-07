using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private Image playerImage;
    [SerializeField] private Text playerText;

    private Player player = null;
    private string nickName;

    private void Update()
    {
        if (player == null)
            return;

        if (player == null)
            playerText.text = string.Format("{0} 0m", nickName);
        else if (player.health <= 0)
            playerText.text = string.Format("{0} Retire", nickName);
        else if (player.finish)
            playerText.text = string.Format("{0} Finish", nickName);
        else
            playerText.text = string.Format("{0} {1:#,##0}m", nickName, Mathf.Abs(player.distance));
    }

    public void SetPlayer(Player player)
    {
        this.player = player;

        playerImage.color = player.color;

        this.nickName = player.nickname;
    }
}
