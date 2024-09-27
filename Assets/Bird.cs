using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bird : MonoBehaviour
{
    public enum BirdType { Normal, Fast, Bomb, Size }
    [SerializeField] BirdType curState = BirdType.Normal;// 현재 상태(평소엔 무능력으로)
    private BaseState[] states = new BaseState[(int)BirdType.Size];

    private Rigidbody2D birdRigd;
    public bool Skill {  get; private set; }

    private void Awake()
    {
        birdRigd = GetComponent<Rigidbody2D>();

        states[(int)BirdType.Normal] = null;
        states[(int)BirdType.Fast] = null;
        states[(int)BirdType.Bomb] = null;
    }
    public void SetState(BirdType stateType)
    {
        curState = stateType; //현재 상태 바꿔주기
    }
    public void UseSkill()
    {
        if (states[(int)curState] != null) // 바꾼 상태에 맞는 능력 활성화
        {
            states[(int)curState].BirdSkill(this);
        }
    }
    public Rigidbody2D GetRigidbody()
    {
        return birdRigd;
    }
    
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
public class FastBird : BaseState
{
    private float speedUp = 2f;

    public override void BirdSkill(Bird bird)
    {
        if (bird.Skill)
        {
            bird.GetRigidbody().velocity *= speedUp;
        }

    }
}
public class BombBird : BaseState
{
    private float explosionRadius = 5f;
    private float explosionPower = 700f;

    public override void BirdSkill(Bird bird)
    {
        Collider2D[] objects = Physics2D.OverlapCircleAll(bird.transform.position, explosionRadius);
        foreach (Collider2D obj in objects)
        {
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = rb.transform.position - bird.transform.position;
                rb.AddForce(direction.normalized * explosionPower);
            }
        }
        Object.Destroy(bird.gameObject); // 폭발 후 새 제거
    }
}