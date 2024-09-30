using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    [SerializeField] List<BirdPool> pool = new List<BirdPool>(); // Ǯ ����Ʈ
    [SerializeField] BirdPool prefab; // �⺻ �� ������
    [SerializeField] int size; // �ʱ� Ǯ ũ��

    private void Awake()
    {
        // �ʱ� Ǯ ����
        for (int i = 0; i < size; i++)
        {
            BirdPool instance = Instantiate(prefab);
            instance.gameObject.SetActive(false); // ��Ȱ��ȭ
            instance.transform.parent = transform; // �θ� ����
            pool.Add(instance); // Ǯ�� �߰�
        }
    }

    public BirdPool GetPool(BirdPool.BirdType birdType, Vector3 position, Quaternion rotation)
    {
        // Ư�� Ÿ���� ���� Ǯ���� ��������
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].birdType == birdType)
            {
                BirdPool instance = pool[i];
                instance.transform.position = position;
                instance.transform.rotation = rotation;
                instance.transform.parent = null; // �θ𿡼� �и�
                instance.gameObject.SetActive(true); // Ȱ��ȭ
                pool.RemoveAt(i); // Ǯ���� ����
                return instance;
            }
        }

        // Ǯ�� ������ ���� ����
        BirdPool newInstance = Instantiate(prefab, position, rotation);
        newInstance.birdType = birdType; // Ÿ�� ����
        return newInstance;
    }

    public void ReturnPool(BirdPool instance)
    {
        instance.gameObject.SetActive(false); // ��Ȱ��ȭ
        pool.Add(instance); // Ǯ�� �߰�
    }
}

