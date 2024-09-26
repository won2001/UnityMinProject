using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // ���� ������ ���������� 3�ʵڿ� ������Ű��
            StartCoroutine(BirdDeletRoutine());
        }
    }
    private IEnumerator BirdDeletRoutine()
    {
        // 3�� ���
        yield return new WaitForSeconds(3f);

        // ���� ����
        Destroy(gameObject);
    }
}
