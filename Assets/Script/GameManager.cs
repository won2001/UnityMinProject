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

        remainingMonster--; // ���� ���� �� ����
        Debug.Log($"��������: {remainingMonster}");

        // ��� ���Ͱ� ������ ���� Ŭ����
        if (remainingMonster <= 0)
        {
            GameClear(); // ���� Ŭ����
        }
    }

}

