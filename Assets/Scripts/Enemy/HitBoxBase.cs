using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBoxBase : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player get hit!");
        }
        if (collision.CompareTag("Bomb"))
        {
            collision.GetComponent<BombController>()?.BombOff();
        }
    }
}
