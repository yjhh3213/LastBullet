using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    public Sprite IdleSprite;           // Idle.png
    public Sprite DashSprite;           // Dash.png
    public Sprite DeathSprite;          // Death.png
    public Sprite WalkSprite;           // Walk.png

    public Sprite foot0;
    public Sprite foot1;
    public Sprite foot2;
    public Sprite foot3;
    public Sprite foot4;

    public int health = 1;              // 캐릭터 체력
    private bool dead = false;          // 캐릭터 사망 여부
    public float speed = 10.0f;         // 캐릭터 속도
    public float Dash = 15.0f;          // 캐릭터 대쉬 속도
    public Text DashCoolDownText;       // 대쉬 쿨타임 텍스트

    public Transform body;
    public Transform foot;
    private SpriteRenderer bodyRenderer;
    private SpriteRenderer footRenderer;

    Vector2 moveV;                      // 캐릭터 조작키
    Rigidbody2D rb;                     // 캐릭터 물리



    // Start is called before the first frame update
    void Start()
    {
        bodyRenderer = transform.Find("body").GetComponent<SpriteRenderer>();
        bodyRenderer.sprite = IdleSprite;

        footRenderer = transform.Find("foot").GetComponent<SpriteRenderer>();
        footRenderer.sprite = foot0;

        rb = GetComponent<Rigidbody2D>();
        dead = false;

        DashCoolDownText.text = "대쉬 : " + ((int)dashTimer).ToString();
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

    public float Walktime = 0.0f;

    void ObjMove()
    {
        //W, A, S, D키 및 상하좌우키 이동 입력받기
        Vector2 Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Move.x > 0 || Move.x < 0 || Move.y < 0 || Move.y > 0)
        {
            Walktime += Time.deltaTime;

            if (Walktime > 0.0f)
            {
                footRenderer.sprite = foot1;
            }
            else if (Walktime > 0.15f)
            {
                footRenderer.sprite = foot2;
            }
            else if (Walktime > 0.25f)
            {
                footRenderer.sprite = foot3;
            }
            Walktime = 0.0f;
        }
        else
        {
            footRenderer.sprite = foot0;
            Walktime = 0.0f;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer <= 0f)
        {
            //Dash
            float dashDir = Input.GetAxisRaw("Horizontal"); // -1 (왼쪽), 0, 1 (오른쪽)

            if (dashDir != 0) // 좌우 입력 있을 때만 대쉬
            {
                rb.velocity = Move.normalized * speed * Dash * Time.deltaTime;
                /*moveV = Move.normalized * speed * Dash;*/
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
        DashCoolDownText.text = "대쉬 : " + ((int)dashTimer).ToString();
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
