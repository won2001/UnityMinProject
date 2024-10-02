using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private int hp = 2;
    [SerializeField] float defaultSpeed;
    [SerializeField] float fastSpeed;
    private static int monsterCount = 0;
    [SerializeField] Animator animator;

    private void Start()
    {
        //GameManager.instance.RegisterMonster();
        //monsterCount++;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 몬스터가 새, 장애물에 부딪쳤을때 발동
        if (collision.collider.CompareTag("Bird") || collision.collider.CompareTag("Obstacle"))
        {
            Rigidbody2D collisionRigidbody = collision.collider.GetComponent<Rigidbody2D>();
            //Destroy(gameObject);
            if (collisionRigidbody != null)
            {
                float currSpeed = collision.relativeVelocity.magnitude; // 충돌속도 계산

                if (currSpeed >= fastSpeed)
                {
                    Debug.Log("데미지 2받음");
                    TakeDamge(2);
                }
                else if (currSpeed >= defaultSpeed)
                {
                    Debug.Log("데미지 1받음");
                    TakeDamge(1);
                    animator.Play("SickPig");
                }
                else
                {
                    Debug.Log("데미지 무시");
                }
            }
        }
    }
    private void TakeDamge(int damge)
    {
        hp -= damge;
        Debug.Log($"돼지체력 {hp}");

        if (hp <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        //GameManager.instance.AddScore(100);
        GameManager.instance.MonsterDied();
        Destroy(gameObject);
    }
}
