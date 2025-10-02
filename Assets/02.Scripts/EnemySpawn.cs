using UnityEngine;
using System.Collections;

public class EnemySpawn : MonoBehaviour
{
    [Header("몬스터 설정")]
    public GameObject monsterPrefab;
    public float spawnDelay = 0.2f;

    [Header("웨이브 설정")]
    public int baseMonsterCount = 20;   // 1웨이브 기본 수
    public int addPerWave = 10;         // 웨이브마다 추가

    [Header("맵 범위")]
    public float minX = -50f;
    public float maxX = 50f;
    public float minZ = -50f;
    public float maxZ = 50f;
    public float minY = -50f;
    public float maxY = 50f;

    [Header("참조")]
    public CountTimer countTimer;

    private bool spawning = false;

    void Update()
    {
        if (countTimer == null) return;

        // WaveEnded가 true이면 새로운 웨이브 시작
        if (countTimer.WaveEnded && !spawning)
        {
            StartCoroutine(SpawnWave());
            countTimer.ResteWaveFlag(); // 플래그 초기화
        }
    }

    IEnumerator SpawnWave()
    {
        spawning = true;

        int wave = countTimer.CurrentWave;
        int monsterCount = baseMonsterCount + (wave - 1) * addPerWave;

        Debug.Log($"웨이브 {wave} 시작! 몬스터 {monsterCount}마리 소환");

        for (int i = 0; i < monsterCount; i++)
        {
            SpawnMonster();
            yield return new WaitForSeconds(spawnDelay);
        }

        Debug.Log($"웨이브 {wave} 완료");
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
