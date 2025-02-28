using System.Collections.Generic;
using CustomInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ObstacleSO", menuName = "Scriptable Objects/ObstacleSO")]
public class ObstacleSO : ScriptableObject
{
    [SerializeField, Foldout] public List<ObstaclePool> obstaclePools;
    [AsRange(0,100)] public Vector2 obstacleInterval;
}
