using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("基础属性")]
    public float speed;
    public Transform pointA, pointB;
    public float distenceToTarget;

    [Header("攻击相关")]
    public float AttackRate;
    public float SkillRate;

    [Header("基础组件")]
    public Animator anim;
    public Rigidbody2D rb;
    public Collider2D col;

    [Header("状态指示器")]
    public BaseState state;
    public int animState;
    public float LastAttack;
    public float LastSkill;
    public Transform target;
    public List<Transform> targetList;


    /* 动画状态：
     * 0 - idle
     * 1 - run
     * 2 - warn
     * 3 - attack-run
     */

    //基础状态
    public PatrolState patrol = new PatrolState();
    public AttackState attack = new AttackState();

    //系统方法
    void Awake()
    {
        Init();
    }
    void Start()
    {
        StateTransforTo(patrol);
    }
    void Update()
    {
        //更新动画&状态机
        anim.SetInteger("state", animState);
        state.StateUpdate(this);
        //更新面朝方向
        FixFacingDirection();
    }

    //自定义方法
    public virtual void Init()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public void StateTransforTo(BaseState newstate)
    {
        state = newstate;
        state.EnterState(this);
    }

    //更改面朝方向
    public void FixFacingDirection()
    {
        if(target.position.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }else
        {
            transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }  
    }

    //敌人移动
    public void MoveToPoint(Transform point)
    {
        transform.position = Vector2.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
    }
    public void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    //选择目标
    public void SwitchPatrolPoint()
    {
        if (Math.Abs(transform.position.x - pointA.position.x) > Math.Abs(transform.position.x - pointB.position.x))
        {
            target = pointA;
        }else
        {
            target = pointB;
        }
    }
 
    //获取目标列表
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (!targetList.Contains(collision.transform))
        {
            targetList.Add(collision.transform);
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        targetList.Remove(collision.transform);
    }

    //攻击相关
    public virtual void Attack()
    {
        anim.SetTrigger("attack");
        LastAttack = Time.time;
    }

    public virtual void Skill()
    {
        anim.SetTrigger("skill");
        LastSkill = Time.time;
    }
}
