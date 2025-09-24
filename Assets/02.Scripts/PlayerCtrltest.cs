using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCtrltest : MonoBehaviour
{
    public int health = 1;              // 캐릭터 체력
    private bool dead = false;          // 캐릭터 사망 여부
    public float speed = 10.0f;         // 캐릭터 속도
    private float Dash = 5.0f;          // 캐릭터 대쉬 속도
    Vector2 moveV;                      // 캐릭터 조작키
    Rigidbody2D rb;                     // 캐릭터 물리

    Animation anim;                     // 캐릭터 애니메이션


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        dead = false;
    }

    public float dashCoolDown = 5.0f;           // 대쉬를 사용하기 위한 쿨타임
    float dashTimer = 0f;
    // Update is called once per frame
    void Update()
    {
        ObjMove();
        RotateToMouse();

        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }
    }

    // 캐릭터가 마우스를 바라보게 하는 코드
    void RotateToMouse()
    {
        // 월드 좌표에서 마우스 위치 구하기
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 방향 벡터
        Vector2 dir = (mousePos - transform.position).normalized;

        // 각도 계산
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // 캐릭터 회전
        transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveV * Time.fixedDeltaTime);
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
        if (collision.CompareTag("Enemy") && !dead)
        {
            health--;
            if (health <= 0)
            {
                dead = true;
                Dead();
            }
        }
    }

    void Dead()
    {
        speed = 0.0f;
        Dash = 0.0f;
    }
}
