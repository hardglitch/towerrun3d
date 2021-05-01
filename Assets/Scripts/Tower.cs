using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private Vector2Int humanoidInTowerRange = new Vector2Int(1, 5);
    [SerializeField] private Humanoid[] humanoidPrefabs;
    private List<Humanoid> humanoidInTower;

    private void Start()
    {
        humanoidInTower = new List<Humanoid>();
        var humanoidPrefabsInTowerCount = Random.Range(humanoidInTowerRange.x, humanoidInTowerRange.y);
        SpawnHumanoidTowers(humanoidPrefabsInTowerCount);
    }

    private void SpawnHumanoidTowers(int humanoidCount)
    {
        if (humanoidPrefabs is null) return;
        var spawnPoint = transform.position;
        for (var i = 0; i < humanoidCount; i++)
        {
            var spawnedHumanoid = humanoidPrefabs[Random.Range(0, humanoidPrefabs.Length)];
            var newHumanoid = Instantiate(spawnedHumanoid, spawnPoint, Quaternion.identity, transform);
            humanoidInTower.Add(newHumanoid);
            humanoidInTower[i].transform.localPosition = new Vector3(0, humanoidInTower[i].transform.localPosition.y,0);
            spawnPoint = humanoidInTower[i].FixationPoint.position;

            // var rb = humanoidInTower[i].gameObject.AddComponent<Rigidbody>();
            // rb.useGravity = true;
            // rb.isKinematic = true;
            // rb.mass = 80;
        }
    }

    [CanBeNull]
    public List<Humanoid> CollectHumanoidInTower(Transform playerDistanceChecker, float maxDistanceFromFixPoint)
    {
        var towerSize = humanoidInTower.Count;
        for (var i = 0; i < towerSize; i++)
        {
            var distanceBetweenPoints = GetDistanceY(playerDistanceChecker, humanoidInTower[i].FixationPoint.transform);
            if (distanceBetweenPoints is null || distanceBetweenPoints > maxDistanceFromFixPoint) continue;
            var collectedHumanoids = humanoidInTower.GetRange(0, i+1);
            humanoidInTower.RemoveRange(0, i+1);
            return collectedHumanoids;
        }

        return null;
    }

    [CanBeNull]
    private float? GetDistanceY(Transform playerDistanceChecker, Transform humanoidFixPoint)
    {
        if (playerDistanceChecker is null || humanoidFixPoint is null) return null;
        var playerDistanceCheckerY = new Vector3(0, playerDistanceChecker.position.y,0);
        var humanoidFixPointY = new Vector3(0, humanoidFixPoint.position.y,0);

        return Vector3.Distance(playerDistanceCheckerY, humanoidFixPointY);
    }

    public void BreakTower()
    {
        foreach (var humanoid in humanoidInTower)
        {
            // humanoid.transform.parent = null;
            var rb = humanoid.gameObject.AddComponent<Rigidbody>();
            // rb.useGravity = true;
            // rb.isKinematic = false;
            // rb.mass = 100;
            // rb.AddExplosionForce(1200, transform.localPosition, 10);
            rb.AddForce(Vector3.forward, ForceMode.Impulse);
        }
        // //Destroy(gameObject);
    }
}