using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bird : MonoBehaviour
{
    public enum BirdType { Normal, Fast, Bomb, Size }
    [SerializeField] BirdType curState = BirdType.Normal;// ���� ����(��ҿ� ���ɷ�����)
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
        curState = stateType; //���� ���� �ٲ��ֱ�
        skillUsed = false;
    }
    public void UseSkill()
    {
        if (states[(int)curState] != null) // �ٲ� ���¿� �´� ��ų Ȱ��ȭ
        {
            states[(int)curState].BirdSkill(this); // ��ų ���
            skillUsed = true; // �̹� �� ��ų�� ���������ϱ�
        }
    }


    public void Explode()
    {
        Debug.Log("����");
        Destroy(gameObject);  // �� ����
    }
    public void ResetBird()
    {
        birdRigd.isKinematic = true;
        birdRigd.velocity = Vector2.zero;
        transform.position = Vector2.zero;
        transform.rotation = Quaternion.identity;
        skillUsed = false;  // ��ų �ʱ�ȭ
    }
    public Rigidbody2D GetRigidbody()
    {
        return birdRigd;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // ���� ������ ���������� 3�ʵڿ� ������Ű��
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
    // ���ڱ� �������� �ϱ�
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
        // ���� ���� �� ����
        bird.StartCoroutine(ExplodeAfterDelay(bird));
    }

    private IEnumerator ExplodeAfterDelay(Bird bird)
    {
        // ���� �ð� ��ٸ���
        yield return new WaitForSeconds(explosionDelay);

        // ���� ��� �� ���� ������ ���߿� �ִ��� Ȯ��
        while (!hasExploded)
        {
            // ���� ���߿� ���� ���
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
        if (!hasExploded) // ������ ���� �� ������
        {
            hasExploded = true; // ���� ���·� ����

            // ���� ����
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

            bird.Explode(); // ���� �� �� ����
        }
    }
}