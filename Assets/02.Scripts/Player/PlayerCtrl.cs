using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCtrl : MonoBehaviour
{
    [Header("Sprites")]
    public Sprite IdleSprite;           // Idle.png
    public Sprite DashSprite;           // Dash.png
    public Sprite DeathSprite;          // Death.png
    public Sprite WalkSprite;           // Walk.png

    public Sprite foot0;
    public Sprite foot1;
    public Sprite foot2;
    public Sprite foot3;
    public Sprite foot4;

    [Header("Stats")]
    public int health = 1;              // ĳ���� ü��
    private bool dead = false;          // ĳ���� ��� ����
    bool isDashing = false;
    public float speed = 2.0f;         // ĳ���� �ӵ�
    public float Dash = 15.0f;          // ĳ���� �뽬 �ӵ�
    public Text DashCoolDownText;       // �뽬 ��Ÿ�� �ؽ�Ʈ

    [Header("Transform")]
    public Transform body;
    public Transform foot;
    private SpriteRenderer bodyRenderer;
    private SpriteRenderer footRenderer;

    Vector2 moveV;                      // ĳ���� ����Ű
    Vector2 dashdir;
    Rigidbody2D rb;                     // ĳ���� ����



    // Start is called before the first frame update
    void Start()
    {
        bodyRenderer = transform.Find("body").GetComponent<SpriteRenderer>();
        bodyRenderer.sprite = IdleSprite;

        footRenderer = transform.Find("foot").GetComponent<SpriteRenderer>();
        footRenderer.sprite = foot0;

        rb = GetComponent<Rigidbody2D>();
        dead = false;

        DashCoolDownText.text = "�뽬 : " + ((int)dashTimer).ToString();
    }

    public float dashCoolDown = 5.0f;           // �뽬�� ����ϱ� ���� ��Ÿ��
    float dashTimer = 0f;
    // Update is called once per frame
    void Update()
    {
        if (dead) return;                       // �׾����� �Է� ����

        if (dashTimer > 0) 
            dashTimer -= Time.deltaTime;

        DashCoolDownText.text = "�뽬 : " + ((int)dashTimer).ToString();

        if (!isDashing)
            ObjMove();

        UpdateSprite();

    }

    public float Walktime = 0.0f;

    void ObjMove()
    {
        if (!isDashing)
        {
            //W, A, S, DŰ �� �����¿�Ű �̵� �Է¹ޱ�
            Vector2 Move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            moveV = Move.normalized * speed;

            // foot �ִϸ��̼� ó��
            if (Move.magnitude > 0)
            {
                Walktime += Time.deltaTime;
                if(Walktime > 0.0f) footRenderer.sprite = foot1;
                if (Walktime > 0.15f) footRenderer.sprite = foot2;
                if (Walktime > 0.25f) { footRenderer.sprite = foot3; Walktime = 0.0f; }
            }
            else
            {
                footRenderer.sprite = foot0;
                Walktime = 0.0f;
            }
        }
        // �����̵� Dash
        if (Input.GetMouseButtonDown(1) && dashTimer <= 0f)
        {
            Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

            if (dir != Vector2.zero) // ���� �Է��� ���� ���� �����̵�
            {
                isDashing = true;
                dashdir = dir.normalized;
                dashTimer = dashCoolDown;
                bodyRenderer.sprite = DashSprite;
                print("Dash!");
                StartCoroutine(DashTimerCoroutine());
                StartCoroutine(ReturnIdle(0.15f));  // ª�� Dash Sprite ����s
            }
        }
    }
    private void FixedUpdate()
    {
        if (dead) return;
        if (isDashing)
        {
            rb.MovePosition(rb.position + dashdir * Dash * Time.fixedDeltaTime);
        }
        else
        {
            {
                rb.MovePosition(rb.position + moveV * Time.fixedDeltaTime);
            }
        }
    }
    IEnumerator DashTimerCoroutine()
    {
        float dashDuration = 0.25f;
        float elapsed = 0f;

        while (elapsed < dashDuration)
        {
            /*rb.MovePosition(rb.position + dir * Dash * Time.fixedDeltaTime);*/
            elapsed += Time.fixedDeltaTime;

            // �ܻ� ���� �ֱ� ����
            StartCoroutine(CreateafterImage(0.25f, 3f));
            yield return new WaitForFixedUpdate();
        }
        isDashing = false;
    }

    // collision Enemy
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Enemy") && !dead)
        {
            health--;
            Dead();
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

    // Dash�� �� �ܻ󳲱��(���� ���� �ð�, ������� �ӵ�)
    IEnumerator CreateafterImage(float duration, float fadespeed)
    {
        GameObject afterImage = new GameObject("AfterImage");
        SpriteRenderer sr = afterImage.AddComponent<SpriteRenderer>();

        sr.sprite = bodyRenderer.sprite;
        sr.transform.position = body.position;
        sr.transform.localScale = body.localScale;
        sr.flipX = bodyRenderer.flipX;
        sr.sortingOrder = bodyRenderer.sortingOrder - 1;        // ��ü���� �ڿ� ��ġ

        Color color = bodyRenderer.color;
        sr.color = new Color(color.r, color.g, color.b, 0.8f);
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            sr.color = new Color(color.r, color.g, color.b, Mathf.Lerp(0.8f, 0f, t));
            elapsed += Time.deltaTime * fadespeed;
            yield return null;
        }

        Destroy(afterImage);
    }

    IEnumerator SpwanImage()
    {
        for(int i = 0; i < 10; i++)
        {
            StartCoroutine(CreateafterImage(0.5f, 5f));
            yield return new WaitForSeconds(0.02f);
        }
    }
}
