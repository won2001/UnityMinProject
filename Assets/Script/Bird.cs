using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bird : MonoBehaviour
{
    public enum BirdType { Normal, Fast, Bomb, Size }
    [SerializeField] BirdType curState = BirdType.Normal;// 현재 상태(평소엔 무능력으로)
    private BaseState[] states = new BaseState[(int)BirdType.Size];

    private Rigidbody2D birdRigd;
    private bool skillUsed;
    public bool Skill {  get; private set; }

    private void Awake()
    {
        birdRigd = GetComponent<Rigidbody2D>();

        states[(int)BirdType.Normal] = null;
        states[(int)BirdType.Fast] = new FastBird();
        states[(int)BirdType.Bomb] = new BombBird();
    }
    public void SetState(BirdType stateType)
    {
        curState = stateType; //현재 상태 바꿔주기
        skillUsed = false;
    }
    public void UseSkill()
    {
        if (states[(int)curState] != null) // 바꾼 상태에 맞는 스킬 활성화
        {
            states[(int)curState].BirdSkill(this); // 스킬 사용
            skillUsed = true; // 이미 쓴 스킬을 못쓰도록하기
        }
    }


    public void Explode()
    {
        Debug.Log("폭발");
        Destroy(gameObject);  // 새 제거
    }
    public void ResetBird()
    {
        birdRigd.isKinematic = true;
        birdRigd.velocity = Vector2.zero;
        transform.position = Vector2.zero;
        transform.rotation = Quaternion.identity;
        skillUsed = false;  // 스킬 초기화
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
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
public class FastBird : BaseState
{
    private float speedUp = 2f;
    // 갑자기 빨라지게 하기
    public override void BirdSkill(Bird bird)
    {
        Rigidbody2D rb = bird.GetRigidbody();
        rb.velocity *= speedUp;

    }
}
public class BombBird : BaseState
{
    private float explosionRadius = 5f;
    private float explosionPower = 500f;
    private float explosionDelay = 0.5f;
    private bool hasExploded = false;
    public override void BirdSkill(Bird bird)
    {
        // 폭발 지연 후 실행
        bird.StartCoroutine(ExplodeAfterDelay(bird));
    }

    private IEnumerator ExplodeAfterDelay(Bird bird)
    {
        // 지연 시간 기다리기
        yield return new WaitForSeconds(explosionDelay);

        // 폭발 대기 중 새가 여전히 공중에 있는지 확인
        while (!hasExploded)
        {
            // 새가 공중에 있을 경우
            if (bird.transform.position.y > 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Explode(bird);
                    break;
                }
            }
            yield return null;
        }
    }

    private void Explode(Bird bird)
    {
        if (!hasExploded) // 폭발이 아직 안 됐으면
        {
            hasExploded = true; // 폭발 상태로 변경

            // 폭발 실행
            Collider2D[] objects = Physics2D.OverlapCircleAll(bird.transform.position, explosionRadius);
            foreach (Collider2D obj in objects)
            {
                Rigidbody2D rigd = obj.GetComponent<Rigidbody2D>();
                if (rigd != null)
                {
                    Vector2 direction = rigd.transform.position - bird.transform.position;
                    rigd.AddForce(direction.normalized * explosionPower);
                }
            }

            bird.Explode(); // 폭발 후 새 제거
        }
    }
}