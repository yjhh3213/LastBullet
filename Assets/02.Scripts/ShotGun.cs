using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShotGun : MonoBehaviour
{
    public GameObject ReloadImage;          // ÀçÀåÀü ÀÌ¹ÌÁö
    public GameObject[] BulletPrefab;       // Åº
    public Transform Point;                 // ÃÑ±¸ À§Ä¡
    public Text BulletCount;                // Åº °¹¼ö
    private int MaxBulletCount = 6;         // ÃÖ´ñÄ¡ Åº °¹¼ö
    public int NowBulletCount = 0;          // ÇöÀç Åº °¹¼ö
    public float BulletSpeed = 10.0f;       // Åº ¼Óµµ

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

    public float ReloadTime = 3.0f;

    void Reload()
    {
        // ÃÑ Åº¾à ¼ö¿¡¼­ ÇöÀç Åº¾à ¼ö¸¦ »« ³ª¸ÓÁö Åº¾à ¼ö¸¦ ÀçÀåÀüÇÑ´Ù
        int reloadBullet = MaxBulletCount - NowBulletCount;
        //NowBulletCount += reloadBullet;

        for(int i = 1; i < reloadBullet + 1; i++)
        {
            int Dice = Random.Range(0, 2);
            if(Dice == 1)
            {
                Time.timeScale = 0f;        // °ÔÀÓ ÀÏ½ÃÁ¤Áö
                ReloadImage.SetActive(true);
                ReloadTime -= Time.deltaTime;
                print(ReloadTime);
                if (ReloadTime <= 0f)
                {
                    ReloadImage.SetActive(false);
                    Time.timeScale = 1f;    // °ÔÀÓ Àç°³
                    ReloadTime = 3.0f;      // ÀçÀåÀü ½Ã°£ ÃÊ±âÈ­
                }
            }
            else
            {
                NowBulletCount++;
            }
            print(Dice);
            //print(i);
        }
        return;
        //print(Dice);
    }
}
