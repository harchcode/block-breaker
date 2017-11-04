using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Paddle paddle;
    public Ball ball;
    public Brick[] bricks;
    public Text scoreText;
    public Image heartImage;
    public TextAsset[] levelData;

    public static int score = 0;

    private int lives;
    private int level;
    private List<Brick> invincibleBricks;
    private Stack<Image> liveImages;
    private AudioSource sound;

    private int combo;

    private void Awake()
    {
        sound = GetComponent<AudioSource>();
    }

    private void Start()
    {
        SetScore(0);
        combo = 0;
        lives = 3;
        
        invincibleBricks = new List<Brick>();
        liveImages = new Stack<Image>();
        liveImages.Push(heartImage);

        InitLives();
        NextLevel();
    }

    private void InitLives()
    {
        for(int i = 1; i < lives; i++)
        {
            var li = Instantiate(heartImage);
            var liWidth = li.rectTransform.sizeDelta.x;

            li.rectTransform.anchoredPosition = new Vector2(
                heartImage.rectTransform.anchoredPosition.x + i * (liWidth + 5), 
                li.rectTransform.anchoredPosition.y
                );

            li.transform.SetParent(heartImage.canvas.transform, false);

            liveImages.Push(li);
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Lose");
    }

    public void Fall()
    {
        lives--;

        var ls = liveImages.Pop();
        Destroy(ls);

        combo = 0;

        if (lives <= 0)
        {
            GameOver();
        }
        else
        {
            sound.Play();
        }

        ball.Reset();
        paddle.Reset();
    }

    public void ComboBreak()
    {
        combo = 0;
    }

    public void AddScore()
    {
        int baseScore = 10 + (level - 1) * (level - 1);
        score += baseScore + (int)(combo * combo * level);

        combo++;

        SetScore(score);
    }

    public void SetScore(int value)
    {
        score = value;
        scoreText.text = value.ToString();
    }

    public void NextLevel()
    {
        level++;
        combo = 0;

        paddle.Reset();
        ball.Reset();

        LoadLevel();
    }

    public int getLevel()
    {
        return level;
    }

    void LoadLevel()
    {
        foreach(var brick in invincibleBricks)
        {
            Destroy(brick.gameObject);
        }

        invincibleBricks.Clear();

        var brickCollider = bricks[0].GetComponent<BoxCollider2D>();
        float brickWidth = brickCollider.size.x;
        float brickHeight = brickCollider.size.y;

        Brick.brickCount = 0;

        int levelIndex = (level - 1) % levelData.Length;
        string levelText = levelData[levelIndex].text;

        string[] lines = levelText.Split('\n');

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];

            for (int j = 0; j < line.Length; j++)
            {
                char c = line[j];

                if (Char.IsDigit(c))
                {
                    int idx = c - '0';
                    Brick newBrick = Instantiate(bricks[idx]);

                    newBrick.transform.position = new Vector3(
                        -4.0f + brickWidth * j,
                        5.75f - brickHeight * i,
                        0
                        );

                    if (idx > 0)
                    {
                        Brick.brickCount++;
                    }
                    else
                    {
                        invincibleBricks.Add(newBrick);
                    }
                }
            }
        }
    }
}
