using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watch : MonoBehaviour
{
<<<<<<< HEAD
    float zrot_Limit = 50.0f;       // z축 회전 제한 각도
    float zrot_spd = 10.0f;         // z축 회전 반응 속도

=======
>>>>>>> Player
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //bodyRenderer = transform.Find("body").GetComponent<SpriteRenderer>();
        RotateToMouse();
    }

    // 캐릭터가 마우스를 바라보게 하는 코드
    // MOUSE
    void RotateToMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
<<<<<<< HEAD
        Vector3 dir = mousePos - transform.position;

        // 좌우 방향 결정
        float yrot = (mousePos.x < transform.position.x) ? 180f : 0f;

        // 마우스 높이에 따라 z축 회전값 결정 (+-50사이)
        float normalizedY = Mathf.Clamp((mousePos.y - transform.position.y) * zrot_spd, -zrot_Limit, zrot_Limit);

        transform.rotation = Quaternion.Euler(0, yrot, normalizedY);
=======

        float yrot = 0.0f;
        float zrot = 0.0f;
        float zrotspd = 11.0f;
        /*print(mousePos.y);
        print(transform.position.y);*/
        if(mousePos.x < transform.position.x)
        {
            yrot = 180.0f;
        }
        else
        {
            yrot = 0.0f;
        }

        float pos = mousePos.y % 5;
        zrot += pos * zrotspd;

        transform.rotation = Quaternion.Euler(0, yrot, zrot);
>>>>>>> Player
    }
}
