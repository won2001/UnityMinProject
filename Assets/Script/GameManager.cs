using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("# Game Control")]
    public bool isLive;

    public int totalMonster;
    private int remainingMonster;
    public GameObject uiResult;

    private void Awake()
    {
        instance = this;
    }

    public void GameStart()
    {
        isLive = true;
        GameObject[] monster = GameObject.FindGameObjectsWithTag("Pig");

        totalMonster = monster.Length;
        remainingMonster = totalMonster;
    }
    public void GameClear()
    {
        StartCoroutine(GameClearRoutine());
    }
    IEnumerator GameClearRoutine()
    {
        isLive = false;

        yield return new WaitForSeconds(1f);
        uiResult.SetActive(true);
    }
    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }

    public void MonsterDied()
    {
        if (!isLive)
            return;

        remainingMonster--; // 남은 몬스터 수 감소
        Debug.Log($"남은돼지: {remainingMonster}");

        // 모든 몬스터가 죽으면 게임 클리어
        if (remainingMonster <= 0)
        {
            GameClear(); // 게임 클리어
        }
    }

}

