using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerController controller;
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        controller = GetComponent<PlayerController>();
            
     }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("SpeedX", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("SpeedY", rb.velocity.y);
        anim.SetBool("Ground", controller.isGround);
        anim.SetBool("Jump", controller.jumpPressed);
    }
}
