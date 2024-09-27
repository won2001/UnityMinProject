using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] GameObject monsterPrepap;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bird"))
        {
            Destroy(gameObject);
        }
        else if (collision.contacts[0].normal.y < 0.5)
        {
            //onDamaged(collision.transform.position);
            Destroy(gameObject);
        }
    }
    private void onDamaged(Vector2 targetPos)
    {

    }
    private void offDamaged()
    {

    }
}
