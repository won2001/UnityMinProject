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
