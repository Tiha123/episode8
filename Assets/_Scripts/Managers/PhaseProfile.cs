using UnityEngine;
using CustomInspector;
using System.Collections.Generic;

[System.Serializable]
    public struct PhaseProfile
    {
        [Space(10)]
        public string profileName;
        public float Distance;
        public float speed;

        [Space(10)]
        [AsRange(0,100)] public Vector2 obstacleInterval;
        [AsRange(0, 100)] public Vector2 collectableInterval;
        [AsRange(1, 30)] public Vector2 collectableQuota;

        [Space(10)]
        public List<int> obstacleWeightList;
        public List<int> collectableWeightList;
        public List<int> patternWeightList;
    }
