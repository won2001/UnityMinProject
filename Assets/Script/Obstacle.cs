using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public enum ObstacleType { Wood, Stone, Glass, Size } // 장애물 타입
    [SerializeField] ObstacleType curType = ObstacleType.Wood;
    private BaseObstacle[] obstacleState = new BaseObstacle[(int)ObstacleType.Size];
    [SerializeField] Animator animator;

    private int hp;

    private void Awake()
    {
        obstacleState[(int)ObstacleType.Wood] = new WoodObstacle();
        obstacleState[(int)ObstacleType.Stone] = new StoneObstacle();
        obstacleState[(int)ObstacleType.Glass] = new GlassObstacle();

        SetObstacleType(curType);
    }
    public void SetObstacleType(ObstacleType type)
    {
        curType = type;
        hp = obstacleState[(int)type].GetHp();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bird") || collision.collider.CompareTag("Obstacle"))
        {
            Rigidbody2D collisionRigidbody = collision.collider.GetComponent<Rigidbody2D>();

            if (collisionRigidbody != null)
            {
                float currSpeed = collision.relativeVelocity.magnitude; // 충돌속도 계산

                obstacleState[(int)curType].TypeCrash(this, currSpeed); // 타입마다 다른 속도에 따른 충돌계산
            }
        }
    }
    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            obstacleState[(int)curType].OnDestroyed(this); // 타입에 따른 파괴 처리
        }
    }
}

public abstract class BaseObstacle 
{
    //장애물 마다 가진 체력
    public abstract int GetHp();
    //장애물 마다 다른 속도에 따른 데미지
    public abstract void TypeCrash(Obstacle obstacle, float speed);
    //장애물 마다 다른 파괴처리
    public abstract void OnDestroyed(Obstacle obstacle);

}
public class WoodObstacle : BaseObstacle
{
    private float fastSpeed = 8f;
    private AudioSource audioSource;
    public override int GetHp()
    {
        return 2;
    }
    public override void TypeCrash(Obstacle obstacle, float speed)
    {
        Animator animator = obstacle.GetComponent<Animator>();
        if (speed >= fastSpeed)
        {
            obstacle.TakeDamage(2);
        }
        else if (speed >= 5f)
        {
            obstacle.TakeDamage(1);
            animator.Play("Woodhit");
            animator.Play("Woodwallhit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Wood);
        }
        else
        {

        }
    }
    public override void OnDestroyed(Obstacle obstacle)
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.WoodDestroy);
        Object.Destroy(obstacle.gameObject);
    }
}
public class StoneObstacle : BaseObstacle 
{
    private float fastSpeed = 12f;
    private AudioSource audioSource;
    public override int GetHp()
    {
        return 4;
    }
    public override void TypeCrash(Obstacle obstacle, float speed)
    {
        Animator animator = obstacle.GetComponent<Animator>();
        if (speed >= fastSpeed)
        {
            obstacle.TakeDamage(2);
        }
        else if (speed >= 3f)
        {
            obstacle.TakeDamage(1);
            animator.Play("Stonhit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Ston);
        }
        else
        {

        }
    }
    public override void OnDestroyed(Obstacle obstacle)
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.StonDestroy);
        Object.Destroy(obstacle.gameObject);
    }
}
public class GlassObstacle : BaseObstacle
{
    private float fastSpeed = 5f;
    private AudioSource audioSource;
    public override int GetHp()
    {
        return 2;
    }

    public override void TypeCrash(Obstacle obstacle, float speed)
    {
        Animator animator = obstacle.GetComponent<Animator>();
        if (speed >= fastSpeed)
        {
            obstacle.TakeDamage(2);
        }
        else if (speed >= 2f)
        {
            obstacle.TakeDamage(1);
            animator.Play("Glasshit");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Glass);
        }
        else
        {

        }
    }
    public override void OnDestroyed(Obstacle obstacle)
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.GlassDestory);
        Object.Destroy(obstacle.gameObject);
    }
}


