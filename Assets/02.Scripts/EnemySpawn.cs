using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour
{
    [Header("���� ����")]
    public GameObject monsterPrefab;
    public float spawnDelay = 0.2f;

    [Header("���̺� ����")]
    public int baseMonsterCount = 20;   // 1���̺� �⺻ ��
    public int addPerWave = 10;         // ���̺긶�� �߰�

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
        int monsterCount = baseMonsterCount + (wave - 1) * addPerWave;

        Debug.Log($"���̺� {wave} ����! ���� {monsterCount}���� ��ȯ");

        for (int i = 0; i < monsterCount; i++)
        {
            SpawnMonster();
            yield return new WaitForSeconds(spawnDelay);
        }

        Debug.Log($"���̺� {wave} �Ϸ�");
        spawning = false;
    }

    void SpawnMonster()
    {
        if (monsterPrefab == null) return;

        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        float randomY = Random.Range(minY, maxY);
        Vector3 spawnPos = new Vector3(randomX, randomY, randomZ);

        Instantiate(monsterPrefab, spawnPos , Quaternion.identity);
    }
}
