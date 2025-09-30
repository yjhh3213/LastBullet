using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;

public class ShotGun : MonoBehaviour
{
    public GameObject ReloadImage;          // ÀçÀåÀü ÀÌ¹ÌÁö
    public GameObject[] BulletPrefab;       // Åº
    public GameObject[] EmptyPrefab;        // ÅºÇÇ
    public Transform FirePoint;             // ÃÑ±¸ À§Ä¡
    public Transform EmptyBullet;           // ÅºÇÇ ¹èÃâ
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

    public float EmptyBulletSpeed = 0.0f;
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
                        Quaternion bulletRot = FirePoint.rotation * Quaternion.Euler(0, 0, BulletSpread);

                        GameObject Bullet = Instantiate(BulletPrefab[0], FirePoint.position, bulletRot);

                        Rigidbody2D rb = Bullet.GetComponent<Rigidbody2D>();
                        rb.velocity = bulletRot * Vector3.right * BulletSpeed;
                        Destroy(Bullet, 2.0f);
                    }
                    EmptyBulletSpeed += Time.deltaTime;
                    Quaternion EBrot = EmptyBullet.rotation * Quaternion.Euler(0, 0, -EmptyBulletSpeed);
                    GameObject EB = Instantiate(EmptyPrefab[0], EmptyBullet.position, EBrot);
                    Destroy(EB, 2.0f);
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

    public float ReloadTime = 3.0f;     // QTE°¡ ÁøÇàµÇ´Â µ¿¾È ÀçÀåÀü ½Ã°£
    bool isReloading = false;           // ÀçÀåÀüÁßÀÎÁö
    /// </summary>
    public int qteMCount = 3;           // QTE ÃÖ´ë È½¼ö

    void Reload()
    {
        if (isReloading) return;

        // ÃÑ Åº¾à ¼ö¿¡¼­ ÇöÀç Åº¾à ¼ö¸¦ »« ³ª¸ÓÁö Åº¾à ¼ö¸¦ ÀçÀåÀüÇÑ´Ù
        int reloadBullet = MaxBulletCount - NowBulletCount;
        StartCoroutine(ReloadC(reloadBullet));
    }

    IEnumerator ReloadC(int reloadbullet)
    {
        isReloading = true;
        int qte = 0;            // QTE È½¼ö

        for(int i = 0; i < reloadbullet; i++)
        {
            // QTE ¹ß»ý ¿©ºÎ È®·ü°ú QTE È½¼ö°¡ ÃÖ´ëº¸´Ù ÀÛÀ» ¶§
            bool DoQte = (Random.value < 0.5f) && (qte < qteMCount);

            if (DoQte)
            {
                qte++;
                KeyCode getkey = (KeyCode)Random.Range(65, 91);     // A~Z ·£´ý
                print((char)getkey);

                ReloadImage.SetActive(true);

                float time = ReloadTime;
                bool IsSuccess = false;

                while(time > 0f)
                {
                    if (Input.GetKeyDown(getkey))
                    {
                        print("Reload!");
                        NowBulletCount++;
                        IsSuccess = true;
                        break;
                    }
                    time -= Time.unscaledDeltaTime;
                    yield return null;
                }

                if (!IsSuccess)
                {
                    print("Reload Fail!");
                }

                ReloadImage.SetActive(false);
            }
            else
            {
                NowBulletCount++;
                yield return new WaitForSeconds(0.1f);
            }
        }
        isReloading = false;

    }
}

