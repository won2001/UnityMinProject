using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Bird;

public class BirdPool : MonoBehaviour
{
    public enum BirdType { Red, Yellow, Black } // 새 타입 정의

    public BirdType birdType; // 현재 새 타입 저장
    public PoolManager returnPool; // 해당 새의 풀 매니저 저장
    public GameObject prefab; // 새 프리팹

    public void UseSkill()
    {
        Debug.Log($"{birdType} 스킬 사용!"); // 스킬 사용 메시지 출력
        returnPool.ReturnPool(this); // 사용 후 풀에 반납
    }

    private void OnEnable()
    {
        InitializeBird(); // 새 초기화 메서드 호출
    }

    private void OnDisable()
    {
        CleanupBird(); // 새 정리 메서드 호출
    }

    private void InitializeBird()
    {
        transform.position = Vector3.zero; // 새의 초기 위치 설정
    }

    private void CleanupBird()
    {
        // 비활성화될 때의 정리 작업
    }
}
