using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCtrl : MonoBehaviour
{
    public Sprite IdleSprite;           // Idle.png
    public Sprite DashSprite;           // Dash.png
    public Sprite DeathSprite;          // Death.png
    public Sprite WalkSprite;           // Walk.png

    public int health = 1;              // 캐릭터 체력
    private bool dead = false;          // 캐릭터 사망 여부
    public float speed = 10.0f;         // 캐릭터 속도
    private float Dash = 5.0f;          // 캐릭터 대쉬 속도

    public Transform body;
    private SpriteRenderer bodyRenderer;

    Vector2 moveV;                      // 캐릭터 조작키
    Rigidbody2D rb;                     // 캐릭터 물리



    // Start is called before the first frame update
    void Start()
    {
        bodyRenderer = transform.Find("body").GetComponent<SpriteRenderer>();
        bodyRenderer.sprite = IdleSprite;
        rb = GetComponent<Rigidbody2D>();
        dead = false;
    }

    public float dashCoolDown = 5.0f;           // 대쉬를 사용하기 위한 쿨타임
    float dashTimer = 0f;
    // Update is called once per frame
    void Update()
    {
        if (dead) return;                       // 죽었으면 입력 막기

        ObjMove();

        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }

        UpdateSprite();

        if (Input.GetKeyDown(KeyCode.F)){
            Dead();
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position +  moveV * Time.fixedDeltaTime);
    }

    void ObjMove()
    {
        //W, A, S, D키 및 상하좌우키 이동 입력받기
        Vector2 Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer <= 0f)
        {
            //Dash
            float dashDir = Input.GetAxisRaw("Horizontal"); // -1 (왼쪽), 0, 1 (오른쪽)

            if (dashDir != 0) // 좌우 입력 있을 때만 대쉬
            {
                rb.AddForce(Vector2.right * dashDir * Dash, ForceMode2D.Impulse);
                print("Dash!");
                bodyRenderer.sprite = DashSprite;
                StartCoroutine(ReturnIdle(0.2f));
                dashTimer = dashCoolDown;
            }
        }
        else
        {
            moveV = Move.normalized * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") && !dead)
        {
            health--;
            if(health <= 0)
            {
                dead = true;
                Dead();
            }
        }
    }

    void Dead()
    {
        dead = true;
        speed = 0.0f;
        Dash = 0.0f;
        bodyRenderer.sprite = DeathSprite;
    }

    void UpdateSprite()
    {
        if (dead) return;

        if(moveV.magnitude > 0.1f)
        {
            bodyRenderer.sprite = WalkSprite;
        }
        else
        {
            bodyRenderer.sprite = IdleSprite;
        }
    }

    IEnumerator ReturnIdle(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!dead) bodyRenderer.sprite = IdleSprite;
    }
}
