using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public enum ObstacleType { Wood, Stone, Glass, Size } // ��ֹ� Ÿ��
    [SerializeField] ObstacleType curType = ObstacleType.Wood;
    private BaseObstacle[] obstacleState = new BaseObstacle[(int)ObstacleType.Size];

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

    public override int GetHp()
    {
        return 2;
    }
    public override void TypeCrash(Obstacle obstacle, float speed)
    {
        if (speed >= fastSpeed)
        {
            obstacle.TakeDamage(2);
        }
        else if (speed >= 5f)
        {
            obstacle.TakeDamage(1);
        }
        else
        {

        }
    }
    public override void OnDestroyed(Obstacle obstacle)
    {
        Object.Destroy(obstacle.gameObject);
    }
}
public class StoneObstacle : BaseObstacle 
{
    private float fastSpeed = 12f;

    public override int GetHp()
    {
        return 4;
    }
    public override void TypeCrash(Obstacle obstacle, float speed)
    {
        if (speed >= fastSpeed)
        {
            obstacle.TakeDamage(2);
        }
        else if (speed >= 3f)
        {
            obstacle.TakeDamage(1);
        }
        else
        {

        }
    }
    public override void OnDestroyed(Obstacle obstacle)
    {
        Object.Destroy(obstacle.gameObject);
    }
}
public class GlassObstacle : BaseObstacle
{
    private float fastSpeed = 5f;

    public override int GetHp()
    {
        return 2;
    }

    public override void TypeCrash(Obstacle obstacle, float speed)
    {
        if (speed >= fastSpeed)
        {
            obstacle.TakeDamage(2);
        }
        else if (speed >= 2f)
        {
            obstacle.TakeDamage(1);
        }
        else
        {

        }
    }
    public override void OnDestroyed(Obstacle obstacle)
    {
        Object.Destroy(obstacle.gameObject);
    }
}


