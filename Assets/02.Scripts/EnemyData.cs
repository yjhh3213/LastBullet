using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData" , menuName = "Enemy/EnemyData")]
public class EnemyData : ScriptableObject
{
    public float hp;
    public float speed;
    public float wave;
}