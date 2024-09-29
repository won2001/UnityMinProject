using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] Text scoreText;

    private void Update()
    {
        scoreText.text = "Á¡¼ö : " + GameManager.instance.score.ToString();
    }
}
