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
                float currSpeed = collision.relativeVelocity.magnitude; // �浹�ӵ� ���

                if (currSpeed >= fastSpeed)
                {
                    Debug.Log("������ 2����");
                    TakeDamge(2);
                }
                else if (currSpeed >= defaultSpeed)
                {
                    Debug.Log("������ 1����");
                    TakeDamge(1);
                }
                else
                {
                    Debug.Log("������ ����");
                }
            }
        }
    }
    private void TakeDamge(int damge)
    {
        hp -= damge;
        Debug.Log($"����ü�� {hp}");

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
