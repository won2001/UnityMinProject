using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public enum ObstacleType { Wood, Stone, Glass, Size } // ��ֹ� Ÿ��
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
                float currSpeed = collision.relativeVelocity.magnitude; // �浹�ӵ� ���

                obstacleState[(int)curType].TypeCrash(this, currSpeed); // Ÿ�Ը��� �ٸ� �ӵ��� ���� �浹���
            }
        }
    }
    public void TakeDamage(int damage)
    {
        hp -= damage;

        if (hp <= 0)
        {
            obstacleState[(int)curType].OnDestroyed(this); // Ÿ�Կ� ���� �ı� ó��
        }
    }
}

public abstract class BaseObstacle 
{
    //��ֹ� ���� ���� ü��
    public abstract int GetHp();
    //��ֹ� ���� �ٸ� �ӵ��� ���� ������
    public abstract void TypeCrash(Obstacle obstacle, float speed);
    //��ֹ� ���� �ٸ� �ı�ó��
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


