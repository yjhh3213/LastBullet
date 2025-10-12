using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watch : MonoBehaviour
{
    float zrot_Limit = 50.0f;
    float zrot_spd = 10.0f;

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
        Vector3 dir = mousePos - transform.position;

        float yrot = (mousePos.x < transform.position.x) ? 180f : 0f;

        float normalizedY = Mathf.Clamp((mousePos.y - transform.position.y) * zrot_spd, -zrot_Limit, zrot_Limit);

        transform.rotation = Quaternion.Euler(0, yrot, normalizedY);
    }
}
