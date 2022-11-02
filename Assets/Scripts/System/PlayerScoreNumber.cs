using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScoreNumber : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    private Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int totalScore = gameManager.GetScore();
        text.text = "Score: " + totalScore.ToString();
    }
}
