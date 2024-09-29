using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    private int hp = 2;
    [SerializeField] float defaultSpeed;
    [SerializeField] float fastSpeed;


    private void OnCollisionEnter2D(Collision2D collision)
    {

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
        GameManager.instance.AddScore(100);
        Destroy(gameObject);
    }
}
