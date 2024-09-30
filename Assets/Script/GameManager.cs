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
    [SerializeField] private GameObject restartButton; // ����� ��ư UI
    [SerializeField] private Text scoreText; // ���� UI
    [SerializeField] private Text bestScoreText; // �ְ� ���� UI

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // GameManager�� �� ��ȯ �� �ı����� �ʰ� ����
        }
        else
        {
            Destroy(gameObject); // �ߺ��� GameManager�� ���� ��� �ı�
        }

        restartButton.SetActive(false); // �ʱ⿡�� ����� ��ư ��Ȱ��ȭ
        UpdateScore(); // �ʱ� ���� ������Ʈ
    }

    public void RegisterMonster()
    {
        currentMonsterCount++; // ���� ���� �� ����
    }

    public void MonsterDied()
    {
        currentMonsterCount--; // ���� ��� �� ���� �� ����
        if (currentMonsterCount <= 0 && !monsterAllDie)
        {
            AllMonstersDead(); // ��� ���Ͱ� �׾��� �� ȣ��
        }
    }

    public void AddScore(int points)
    {
        score += points; // ���� �߰�
        UpdateScore(); // UI ������Ʈ
    }

    private void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score; // ���� UI ������Ʈ
        }

        // �ְ� ���� ������Ʈ ���� �߰�
        int bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (score > bestScore)
        {
            PlayerPrefs.SetInt("BestScore", score);
            bestScoreText.text = "Best Score: " + score; // �ְ� ���� UI ������Ʈ
        }
    }

    private void AllMonstersDead()
    {
        monsterAllDie = true; // ��� ���� ���� ����
        ShowRestartButton(); // ����� ��ư ǥ��
    }

    private void ShowRestartButton()
    {
        if (restartButton != null)
        {
            restartButton.SetActive(true); // ����� ��ư Ȱ��ȭ
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // ���� �� �����
        score = 0; // ���� �ʱ�ȭ
        currentMonsterCount = 0; // ���� �� �ʱ�ȭ
        monsterAllDie = false; // ���� ���� �ʱ�ȭ
        restartButton.SetActive(false); // ����� ��ư ��Ȱ��ȭ
        UpdateScore(); // ���� UI �ʱ�ȭ
    }
}

