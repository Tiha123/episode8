using UnityEngine;
using System.Collections.Generic;

public class ObstacleDCompose : ObstacleDouble
{
    public List<Obstacle> compositePrefabs;

    void Start()
    {
        SpawnComposited();
    }
    private void SpawnComposited()
    {

        spawnedPos.ForEach(p =>
        {
            int rnd = Random.Range(0, compositePrefabs.Count);
            Obstacle prefab = compositePrefabs[rnd];

            var o = Instantiate(prefab, p, Quaternion.identity, transform);
            Vector3 localPos = o.transform.localPosition;
            o.transform.localPosition = new Vector3(localPos.x, 0f, 0f);
        });

    }
}
