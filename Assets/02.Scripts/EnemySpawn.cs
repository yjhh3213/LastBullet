using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour
{
    [Header("���� ����")]
    public GameObject monsterPrefabA; // 1~8�� ����
    public GameObject monsterPrefabB; // 3~8�� 

    [Header("���� �����ð�")]
    public float spawnDelay = 0.2f;

    [Header("���̺� ����")]
    public int minWave = 1;
    public int maxWave = 8;

    [Header("�⺻���� ��Ģ")]
    public int A_startCount = 20;   // 1���̺� �⺻ ��
    public int A_addPerWave = 10;         // ���̺긶�� �߰�

    [Header("���� ����� ��Ģ")]
    public int B_startWave = 3;
    public int B_startCount = 5;
    public int B_addper2Wave = 2;

    [Header("�� ����")]
    public float minX = -50f;
    public float maxX = 50f;
    public float minZ = -50f;
    public float maxZ = 50f;
    public float minY = -50f;
    public float maxY = 50f;

    [Header("����")]
    public CountTimer countTimer;

    private bool spawning = false;

    void Update()
    {
        if (countTimer == null) return;

        int wave = countTimer.CurrentWave;

        if (wave < minWave || wave > maxWave) return;
        if (maxWave < countTimer.CurrentWave) return;
        // WaveEnded�� true�̸� ���ο� ���̺� ����
        if (countTimer.WaveEnded && !spawning)
        {
            StartCoroutine(SpawnWave());
            countTimer.ResteWaveFlag(); // �÷��� �ʱ�ȭ
        }
    }

    IEnumerator SpawnWave()
    {
        spawning = true;

        int wave = countTimer.CurrentWave;
        int aCount = A_startCount + (wave - 1) * A_addPerWave;

        int bCount = 0;
        if (wave >= B_startWave && wave <= maxWave)
        {
            bCount = B_startCount;

            int evenSteps = (wave / 2) - ((B_startWave - 1) / 2);
            if (evenSteps > 0)
            {
                bCount += evenSteps * B_addper2Wave;
            }
        }
        Debug.Log($"[Wave {wave}] Spawn A: {aCount}, B: {bCount}");

        for(int i = 0; i < aCount; i++)
        {
            SpawnMonster(monsterPrefabA);
            if(spawnDelay > 0f)
            {
                yield return new WaitForSeconds(spawnDelay);
            }
        }

        for(int i = 0;i < bCount;i++)
        {
            SpawnMonster(monsterPrefabB);
            if(spawnDelay > 0f)
            {
                yield return new WaitForSeconds(spawnDelay);
            }
        }

        spawning = false; ;
    }

    void SpawnMonster(GameObject prefab)
    {
        if (prefab == null) return;
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        float randomY = Random.Range(minY, maxY);
        Instantiate(prefab, new Vector3(randomX, randomY, randomZ), Quaternion.identity);

    }
}
