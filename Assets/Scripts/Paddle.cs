using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public Ball ball;
    public float baseSpeed = 6f;

    private LevelManager levelManager;
    private float speed;
    private BoxCollider2D cl;
    private float width;
    private bool isMouseActive;
    private float oldMouseX;
    private AudioSource sound;

    private void Awake()
    {
        levelManager = GameObject.FindObjectOfType<LevelManager>();
        sound = GetComponent<AudioSource>();
    }

    // Use this for initialization
    void Start()
    {
        cl = this.GetComponent<BoxCollider2D>();
        width = cl.size.x;
        isMouseActive = false;
        oldMouseX = Input.mousePosition.x;
    }

    public void Reset()
    {
        speed = (levelManager.getLevel() / 8f + 1f) * baseSpeed;

        GrabBall();
    }

    void HandleKeyBoardInput(float ds)
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (isMouseActive)      // To prevent moving with both mouse and keyboard resulting in double speed
            {
                isMouseActive = false;
            }
            else
            {
                Move(ds);
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (isMouseActive)
            {
                isMouseActive = false;
            }
            else
            {
                Move(-ds);
            }
        }

        if (!ball.IsMoving() && 
            (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)))
        {
            ball.BeginMove();
        }
    }

    void HandleMouseInput(float ds)
    {
        float mousePosX = Input.mousePosition.x;

        if (oldMouseX != mousePosX)
        {
            isMouseActive = true;
            oldMouseX = mousePosX;
        }

        if (isMouseActive)
        {
            float mouseX = mousePosX / Screen.width * 9f - 4.5f;

            if (mouseX > transform.position.x)
            {
                Move(ds);

                if (transform.position.x > mouseX)
                {
                    transform.position = new Vector3(
                        mouseX, transform.position.y, transform.position.z
                        );
                }
            }
            else if (mouseX < transform.position.x)
            {
                Move(-ds);

                if (transform.position.x < mouseX)
                {
                    transform.position = new Vector3(
                        mouseX, transform.position.y, transform.position.z
                        );
                }
            }
        }

        if (!ball.IsMoving() && Input.GetMouseButtonDown(0))
        {
            ball.BeginMove();
        }
    }

    void HandleTouchInput(float ds)
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                var touchX = touch.position.x / Screen.width * 9f - 4.5f;
                var touchY = touch.position.y / Screen.height * 16f - 8f;

                if (!ball.IsMoving() && cl.bounds.Intersects(new Bounds(new Vector3(touchX, touchY, 0),
                    new Vector3(1, 1, 1))))
                {
                    ball.BeginMove();
                }
                else
                {
                    if (touchX > transform.position.x)
                    {
                        Move(ds);

                        if (transform.position.x > touchX)
                        {
                            transform.position = new Vector3(
                                touchX, transform.position.y, transform.position.z
                                );
                        }
                    }
                    else if (touchX < transform.position.x)
                    {
                        Move(-ds);

                        if (transform.position.x < touchX)
                        {
                            transform.position = new Vector3(
                                touchX, transform.position.y, transform.position.z
                                );
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;
        float ds = speed * dt;

        HandleKeyBoardInput(ds);
        HandleMouseInput(ds);
        HandleTouchInput(ds);        
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            levelManager.ComboBreak();

            var halfWidth = width / 2;

            var relX = Mathf.Clamp(
                collision.contacts[0].point.x - this.transform.position.x,
                -halfWidth, halfWidth
                );

            var xDir = (relX / halfWidth) * 0.9f;
            var yDir = 1 - xDir * xDir;

            sound.Play();

            ball.Launch(new Vector2(xDir, yDir));
        }
    }

    private void Move(float ds)
    {
        var maxX = 4.5f - (width / 2);

        transform.position = new Vector3(
               Mathf.Clamp(transform.position.x + ds, -maxX, maxX),
               transform.position.y,
               transform.position.z
               );

        if (!ball.IsMoving())
        {
            ball.transform.position = new Vector3(
                   transform.position.x + cl.size.x / 3,
                   ball.transform.position.y,
                   ball.transform.position.z
                   );
        }
    } 

    private void GrabBall()
    {
        ball.StopMove();

        var r = ball.GetRadius();
        var halfHeight = cl.size.y / 2;

        ball.transform.position = new Vector3(
            transform.position.x + cl.size.x / 3,
            transform.position.y + halfHeight + r * ball.transform.localScale.y,
            ball.transform.position.z
        );
    }
}
