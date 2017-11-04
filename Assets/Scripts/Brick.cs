using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public Sprite[] sprites;
    public bool isBreakable = true;
    public ParticleSystem smoke;
    public AudioClip clip;

    public static int brickCount = 0;

    private int hp;
    private int hit;
    private SpriteRenderer sr;
    private LevelManager levelManager;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        levelManager = GameObject.FindObjectOfType<LevelManager>();
    }

    // Use this for initialization
    void Start ()
    {
        hp = sprites.Length + 1;
        hit = 0;
	}

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (isBreakable)
        {
            hit++;

            levelManager.AddScore();

            if (hit >= hp)
            {
                AudioSource.PlayClipAtPoint(clip, new Vector3(0, 0, -10));

                brickCount--;

                var newSmoke = Instantiate(smoke, transform.position, Quaternion.identity);
                ParticleSystem.MainModule newMain = newSmoke.main;
                newMain.startColor = sr.color;

                Destroy(gameObject);

                if (brickCount <= 0)
                {
                    levelManager.NextLevel();
                }
            }
            else
            {
                sr.sprite = sprites[hit - 1];
            }
        }
    }
}
