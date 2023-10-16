using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    public float CreateTime, WaitTime, BoomRadius, BoomForce, UpRate;
    public bool isOff;
    public Collider2D[] aroundItems;

    private Animator anim;
    private Rigidbody2D rb;
    private Collider2D col;

    void Start()
    {
        CreateTime = Time.time;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (CreateTime + WaitTime <= Time.time && !isOff)
        {
            rb.gravityScale = 0;
            col.enabled = false;
            anim.Play("Bomb_explosion");
            aroundItems = Physics2D.OverlapCircleAll(transform.position, BoomRadius);
            foreach (var item in aroundItems)
            {
                //ÒýÈ¼»úÖÆ
                if (item.CompareTag("Bomb") && item.GetComponent<BombController>().isOff)
                {
                    item.GetComponent<BombController>().BombOn();
                }
                if (!item.CompareTag("Trigger"))
                {
                    Vector2 dist = transform.position - item.transform.position;
                    Vector2 radcur = new Vector2(BoomRadius, BoomRadius + UpRate);
                    item.GetComponent<Rigidbody2D>().AddForce((radcur - dist) * BoomForce);
                }
            }
        }
    }

    private void Destroyboom()
    {
        Destroy(gameObject);
    }
    
    public void BombOff()
    {
        anim.Play("Bomb_off");
        isOff = true;
        gameObject.layer = LayerMask.NameToLayer("Env-Items");
    }

    public void BombOn()
    {
        anim.Play("Bomb_on");
        gameObject.layer = LayerMask.NameToLayer("Bomb");
        isOff = false;
        CreateTime = Time.time;
    }
}
