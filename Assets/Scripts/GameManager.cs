using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI pauseButtonText;
    [SerializeField] private TextMeshProUGUI gameSpeedText;
    [SerializeField] private float[] gameSpeeds;

    private float previousGameSpeed;
    private int currentGameSpeedIndex;
    private bool gamePaused;


    // Start is called before the first frame update
    void Start()
    {

        //find the index corresponding to a gameSpeed of 1
        for (int i = 0; i < gameSpeeds.Length; i++)
        {
            if (gameSpeeds[i] == 1)
            {
                currentGameSpeedIndex = i;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PauseBattle()
    {
        previousGameSpeed = Time.timeScale;
        Time.timeScale = 0f;
        gamePaused = true;
    }

    private void UnpauseBattle()
    {
        Time.timeScale = previousGameSpeed;
        gamePaused = false;
    }

    private void AccelerateBattle()
    {
        currentGameSpeedIndex = (currentGameSpeedIndex == gameSpeeds.Length-1)? gameSpeeds.Length - 1 : currentGameSpeedIndex + 1;
        Time.timeScale = gameSpeeds[currentGameSpeedIndex];
    }

    private void SlowBattle()
    {
        currentGameSpeedIndex = (currentGameSpeedIndex == 0) ? 0 : currentGameSpeedIndex - 1;
        Time.timeScale = gameSpeeds[currentGameSpeedIndex];
    }

    public void PauseButton()
    {
        if (gamePaused)
        {
            UnpauseBattle();
            pauseButtonText.text = "||";
        }
        else
        {
            PauseBattle();
            pauseButtonText.text = "|>";
        }
        gameSpeedText.text = "x" + Time.timeScale.ToString();
    }

    public void AccelerateButton()
    {
        AccelerateBattle();
        gameSpeedText.text = "x" + Time.timeScale.ToString();
    }

    public void SlowButton()
    {
        SlowBattle();
        gameSpeedText.text = "x" + Time.timeScale.ToString();
    }
}
