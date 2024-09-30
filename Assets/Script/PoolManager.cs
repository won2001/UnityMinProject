using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    [SerializeField] List<BirdPool> pool = new List<BirdPool>(); // 풀 리스트
    [SerializeField] BirdPool prefab; // 기본 새 프리팹
    [SerializeField] int size; // 초기 풀 크기

    private void Awake()
    {
        // 초기 풀 생성
        for (int i = 0; i < size; i++)
        {
            BirdPool instance = Instantiate(prefab);
            instance.gameObject.SetActive(false); // 비활성화
            instance.transform.parent = transform; // 부모 설정
            pool.Add(instance); // 풀에 추가
        }
    }

    public BirdPool GetPool(BirdPool.BirdType birdType, Vector3 position, Quaternion rotation)
    {
        // 특정 타입의 새를 풀에서 가져오기
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].birdType == birdType)
            {
                BirdPool instance = pool[i];
                instance.transform.position = position;
                instance.transform.rotation = rotation;
                instance.transform.parent = null; // 부모에서 분리
                instance.gameObject.SetActive(true); // 활성화
                pool.RemoveAt(i); // 풀에서 제거
                return instance;
            }
        }

        // 풀에 없으면 새로 생성
        BirdPool newInstance = Instantiate(prefab, position, rotation);
        newInstance.birdType = birdType; // 타입 설정
        return newInstance;
    }

    public void ReturnPool(BirdPool instance)
    {
        instance.gameObject.SetActive(false); // 비활성화
        pool.Add(instance); // 풀에 추가
    }
}

