using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotGun : MonoBehaviour
{
    public GameObject[] BulletPrefab;       // Åº
    public Transform Point;                 // ÃÑ±¸ À§Ä¡
    public Text BulletCount;                // Åº °¹¼ö
    private int MaxBulletCount = 6;         // ÃÖ´ñÄ¡ Åº °¹¼ö
    public int NowBulletCount = 0;          // ÇöÀç Åº °¹¼ö
    public float BulletSpeed = 10.0f;       // Åº ¼Óµµ
    public float BulletAttack = 2.0f;       // Åº °ø°Ý

    // Start is called before the first frame update
    void Start()
    {
        BulletCount.text = NowBulletCount.ToString()+ " "+ "/" + " " + MaxBulletCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        Conmand();
        BulletCount.text = NowBulletCount.ToString() + " " + "/" + " " + MaxBulletCount.ToString();
    }

    void Conmand()
    {
        //Åº¾à ¹ß»ç
        if (Input.GetMouseButtonDown(0))
        {
            print("Left Click");
            if(NowBulletCount >= 1) 
            {
                int BulletNumber = Random.Range(3, 9);
                //print(BulletNumber);
                if (BulletPrefab != null)
                {
                    for (int i = 0; i < BulletNumber; i++)
                    {
                        float BulletSpread = Random.Range(-15f, 15f);
                        Quaternion bulletRot = Point.rotation * Quaternion.Euler(0, 0, BulletSpread);

                        GameObject Bullet = Instantiate(BulletPrefab[0], Point.position, bulletRot);

                        Rigidbody2D rb = Bullet.GetComponent<Rigidbody2D>();
                        rb.velocity = bulletRot * Vector3.right * BulletSpeed;
                        Destroy(Bullet, 2.0f);
                    }
                    NowBulletCount--;
                }
            }
            else if(NowBulletCount <= 0)
            {
                print("Bullet Empty");
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
            print("Reload");
        }
    }

    void Reload()
    {
        // ÃÑ Åº¾à ¼ö¿¡¼­ ÇöÀç Åº¾à ¼ö¸¦ »« ³ª¸ÓÁö Åº¾à ¼ö¸¦ ÀçÀåÀüÇÑ´Ù
        int reloadBullet = MaxBulletCount - NowBulletCount;
        NowBulletCount += reloadBullet;
        //print(Dice);
    }
}
