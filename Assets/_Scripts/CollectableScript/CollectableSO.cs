using UnityEngine;
using System.Collections.Generic;
using CustomInspector;

[CreateAssetMenu(fileName = "CollectableSO", menuName = "Scriptable Objects/CollectableSO")]
public class CollectableSO : ScriptableObject
{
    public List<CollectablePool> collectablePools;
    public List<LanePool> lanepatternPools;
    [SerializeField, AsRange(0, 100)] public Vector2 spawnInterval;
    [SerializeField, AsRange(1, 30)] public Vector2 spawnquota;
}
