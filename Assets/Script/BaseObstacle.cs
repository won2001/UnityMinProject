using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseObstacle : MonoBehaviour
{
    [SerializeField] protected float maxHealth = 10f;   // 최대 체력
    protected float currentHealth;                       // 현재 체력
    [SerializeField] protected float minDamageSpeed = 2f; // 데미지를 입을 최소 속도
    [SerializeField] protected float mediumDamageSpeed = 5f; // 중간 데미지를 받을 속도 기준
    [SerializeField] protected float maxDamageSpeed = 8f; // 최대 데미지를 받을 속도 기준

    protected SpriteRenderer spriteRenderer;             // 장애물의 스프라이트 렌더러
    [SerializeField] protected Sprite[] damageSprites;   // 데미지에 따른 스프라이트 (금 간 상태)
    protected int currentSpriteIndex = 0;                // 현재 적용된 스프라이트 인덱스

    protected virtual void Start()
    {
        currentHealth = maxHealth;                       // 체력 초기화
        spriteRenderer = GetComponent<SpriteRenderer>(); // 스프라이트 렌더러 설정
        UpdateSprite();                                  // 스프라이트 초기화
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float collisionSpeed = collision.relativeVelocity.magnitude;

        if (collisionSpeed >= minDamageSpeed)
        {
            // 충돌 속도에 따른 데미지 계산
            float damage = CalculateDamage(collisionSpeed);
            ApplyDamage(damage);
        }
    }

    // 충돌 속도에 따른 데미지 계산 로직
    protected virtual float CalculateDamage(float collisionSpeed)
    {
        if (collisionSpeed >= maxDamageSpeed)
        {
            return maxHealth; // 강하게 부딪히면 최대 데미지
        }
        else if (collisionSpeed >= mediumDamageSpeed)
        {
            return maxHealth - 5; // 중간 속도는 절반 데미지
        }
        else
        {
            return maxHealth - 1; // 약한 충돌은 최소 데미지
        }
    }

    // 데미지를 입는 메서드
    protected virtual void ApplyDamage(float damage)
    {
        currentHealth -= damage; // 체력 감소

        // 데미지에 따라 스프라이트 업데이트
        UpdateSprite();

        if (currentHealth <= 0)
        {
            DestroyObstacle(); // 체력이 0이 되면 파괴
        }
    }

    // 장애물 파괴
    protected virtual void DestroyObstacle()
    {
        
        Destroy(gameObject); // 장애물 제거
    }

    // 금이 간 상태의 스프라이트로 변경
    protected virtual void UpdateSprite()
    {
        // 체력에 따른 스프라이트 업데이트
        if (currentHealth <= maxHealth * 0.2f)
        {
            currentSpriteIndex = 2; // 많이 금이 간 상태
        }
        else if (currentHealth <= maxHealth * 0.5f)
        {
            currentSpriteIndex = 1; // 약간 금이 간 상태
        }
        else
        {
            currentSpriteIndex = 0; // 기본 상태
        }

        // 스프라이트 렌더러에 새로운 스프라이트 적용
        if (damageSprites.Length > currentSpriteIndex && spriteRenderer != null)
        {
            spriteRenderer.sprite = damageSprites[currentSpriteIndex];
        }
    }
}
