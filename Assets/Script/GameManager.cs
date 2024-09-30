using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int score = 0;
    private int currentMonsterCount = 0;
    private bool monsterAllDie = false;

    [Header("UI")]
    [SerializeField] private GameObject restartButton; // 재시작 버튼 UI
    [SerializeField] private Text scoreText; // 점수 UI
    [SerializeField] private Text bestScoreText; // 최고 점수 UI

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // GameManager를 씬 전환 시 파괴되지 않게 설정
        }
        else
        {
            Destroy(gameObject); // 중복된 GameManager가 있을 경우 파괴
        }

        restartButton.SetActive(false); // 초기에는 재시작 버튼 비활성화
        UpdateScore(); // 초기 점수 업데이트
    }

    public void RegisterMonster()
    {
        currentMonsterCount++; // 현재 몬스터 수 증가
    }

    public void MonsterDied()
    {
        currentMonsterCount--; // 몬스터 사망 시 현재 수 감소
        if (currentMonsterCount <= 0 && !monsterAllDie)
        {
            AllMonstersDead(); // 모든 몬스터가 죽었을 때 호출
        }
    }

    public void AddScore(int points)
    {
        score += points; // 점수 추가
        UpdateScore(); // UI 업데이트
    }

    private void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score; // 점수 UI 업데이트
        }

        // 최고 점수 업데이트 로직 추가
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (score > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", score);
            bestScoreText.text = "Best Score: " + score; // 최고 점수 UI 업데이트
        }
    }

    private void AllMonstersDead()
    {
        monsterAllDie = true; // 모든 몬스터 상태 설정
        ShowRestartButton(); // 재시작 버튼 표시
    }

    private void ShowRestartButton()
    {
        if (restartButton != null)
        {
            restartButton.SetActive(true); // 재시작 버튼 활성화
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬 재시작
        score = 0; // 점수 초기화
        currentMonsterCount = 0; // 몬스터 수 초기화
        monsterAllDie = false; // 몬스터 상태 초기화
        restartButton.SetActive(false); // 재시작 버튼 비활성화
        UpdateScore(); // 점수 UI 초기화
    }
}

