using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] Text scoreText;

    private void Start()
    {
        scoreText = GetComponent<Text>();
    }
   // private void Update()
   // {
   //     if (scoreText != null && GameManager.instance != null)
   //     {
   //         scoreText.text = "Score : " + GameManager.instance.score.ToString();
   //     }
   //     
   // }

    
}
