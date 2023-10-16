using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("��������")]
    public float speed;
    public Transform pointA, pointB;
    public float distenceToTarget;

    [Header("�������")]
    public float AttackRate;
    public float SkillRate;

    [Header("�������")]
    public Animator anim;
    public Rigidbody2D rb;
    public Collider2D col;

    [Header("״ָ̬ʾ��")]
    public BaseState state;
    public int animState;
    public float LastAttack;
    public float LastSkill;
    public Transform target;
    public List<Transform> targetList;


    /* ����״̬��
     * 0 - idle
     * 1 - run
     * 2 - warn
     * 3 - attack-run
     */

    //����״̬
    public PatrolState patrol = new PatrolState();
    public AttackState attack = new AttackState();

    //ϵͳ����
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
        //���¶���&״̬��
        anim.SetInteger("state", animState);
        state.StateUpdate(this);
        //�����泯����
        FixFacingDirection();
    }

    //�Զ��巽��
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

    //�����泯����
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

    //�����ƶ�
    public void MoveToPoint(Transform point)
    {
        transform.position = Vector2.MoveTowards(transform.position, point.position, speed * Time.deltaTime);
    }
    public void MoveToTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }

    //ѡ��Ŀ��
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
 
    //��ȡĿ���б�
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

    //�������
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
