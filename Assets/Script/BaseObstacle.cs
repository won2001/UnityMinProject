using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObstacle : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 10f;   // �ִ� ü��
    protected float currentHealth;                       // ���� ü��
    [SerializeField] protected float minDamageSpeed = 2f; // �������� ���� �ּ� �ӵ�
    [SerializeField] protected float mediumDamageSpeed = 5f; // �߰� �������� ���� �ӵ� ����
    [SerializeField] protected float maxDamageSpeed = 8f; // �ִ� �������� ���� �ӵ� ����

    protected SpriteRenderer spriteRenderer;             // ��ֹ��� ��������Ʈ ������
    [SerializeField] protected Sprite[] damageSprites;   // �������� ���� ��������Ʈ (�� �� ����)
    protected int currentSpriteIndex = 0;                // ���� ����� ��������Ʈ �ε���

    protected virtual void Start()
    {
        currentHealth = maxHealth;                       // ü�� �ʱ�ȭ
        spriteRenderer = GetComponent<SpriteRenderer>(); // ��������Ʈ ������ ����
        UpdateSprite();                                  // ��������Ʈ �ʱ�ȭ
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float collisionSpeed = collision.relativeVelocity.magnitude;

        if (collisionSpeed >= minDamageSpeed)
        {
            // �浹 �ӵ��� ���� ������ ���
            float damage = CalculateDamage(collisionSpeed);
            ApplyDamage(damage);
        }
    }

    // �浹 �ӵ��� ���� ������ ��� ����
    protected virtual float CalculateDamage(float collisionSpeed)
    {
        if (collisionSpeed >= maxDamageSpeed)
        {
            return maxHealth; // ���ϰ� �ε����� �ִ� ������
        }
        else if (collisionSpeed >= mediumDamageSpeed)
        {
            return maxHealth - 5; // �߰� �ӵ��� ���� ������
        }
        else
        {
            return maxHealth - 1; // ���� �浹�� �ּ� ������
        }
    }

    // �������� �Դ� �޼���
    protected virtual void ApplyDamage(float damage)
    {
        currentHealth -= damage; // ü�� ����

        // �������� ���� ��������Ʈ ������Ʈ
        UpdateSprite();

        if (currentHealth <= 0)
        {
            DestroyObstacle(); // ü���� 0�� �Ǹ� �ı�
        }
    }

    // ��ֹ� �ı�
    protected virtual void DestroyObstacle()
    {
        
        Destroy(gameObject); // ��ֹ� ����
    }

    // ���� �� ������ ��������Ʈ�� ����
    protected virtual void UpdateSprite()
    {
        // ü�¿� ���� ��������Ʈ ������Ʈ
        if (currentHealth <= maxHealth * 0.2f)
        {
            currentSpriteIndex = 2; // ���� ���� �� ����
        }
        else if (currentHealth <= maxHealth * 0.5f)
        {
            currentSpriteIndex = 1; // �ణ ���� �� ����
        }
        else
        {
            currentSpriteIndex = 0; // �⺻ ����
        }

        // ��������Ʈ �������� ���ο� ��������Ʈ ����
        if (damageSprites.Length > currentSpriteIndex && spriteRenderer != null)
        {
            spriteRenderer.sprite = damageSprites[currentSpriteIndex];
        }
    }
}
