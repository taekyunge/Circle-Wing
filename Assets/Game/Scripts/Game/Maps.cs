using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maps : MonoBehaviour
{
    private float damage = 10;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            var player = collision.gameObject.GetComponent<Player>();

            if (player != null)
            {
                player.Hit(damage);
            }
        }
    }
}
