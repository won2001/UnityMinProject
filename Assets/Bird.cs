using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 새가 땅으로 떨어졌으면 3초뒤에 삭제시키기
            StartCoroutine(BirdDeletRoutine());
        }
    }
    private IEnumerator BirdDeletRoutine()
    {
        // 3초 대기
        yield return new WaitForSeconds(3f);

        // 새를 제거
        Destroy(gameObject);
    }
}
