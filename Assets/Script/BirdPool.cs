using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Bird;

public class BirdPool : MonoBehaviour
{
    public enum BirdType { Red, Yellow, Black } // �� Ÿ�� ����

    public BirdType birdType; // ���� �� Ÿ�� ����
    public PoolManager returnPool; // �ش� ���� Ǯ �Ŵ��� ����
    public GameObject prefab; // �� ������

    public void UseSkill()
    {
        Debug.Log($"{birdType} ��ų ���!"); // ��ų ��� �޽��� ���
        returnPool.ReturnPool(this); // ��� �� Ǯ�� �ݳ�
    }

    private void OnEnable()
    {
        InitializeBird(); // �� �ʱ�ȭ �޼��� ȣ��
    }

    private void OnDisable()
    {
        CleanupBird(); // �� ���� �޼��� ȣ��
    }

    private void InitializeBird()
    {
        transform.position = Vector3.zero; // ���� �ʱ� ��ġ ����
    }

    private void CleanupBird()
    {
        // ��Ȱ��ȭ�� ���� ���� �۾�
    }
}
