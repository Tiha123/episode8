using UnityEngine;
using CustomInspector;

[CreateAssetMenu(fileName ="PhaseSO", menuName = "Scriptable Objects/PhaseSO")]
public class PhaseSO : ScriptableObject
{
    [Space(10)]
        public string displayName;
        [Preview(Size.small)]public Sprite icon;
        public float distance;
        public float speed;
        public ObstacleSO obstacleData;
        public CollectableSO collectableSO;
}
