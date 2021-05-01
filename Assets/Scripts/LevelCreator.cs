using PathCreation;
using UnityEngine;

public class LevelCreator : MonoBehaviour
{
    [SerializeField] private PathCreator pathCreator;
    [SerializeField] private Tower towerPrefab;
    [SerializeField] private int humanoidTowerCount;

    private void Start()
    {
        CreateLevel();
    }

    private void CreateLevel()
    {
        var pathLength = pathCreator.path.length;
        var distanceBetweenTowers = pathLength / humanoidTowerCount;

        for (var i = 1; i <= humanoidTowerCount; i++)
        {
            var towerSpawnPoint = pathCreator.path.GetPointAtDistance(distanceBetweenTowers * (i - 0.5f), EndOfPathInstruction.Stop);
            Instantiate(towerPrefab, towerSpawnPoint, Quaternion.identity);
        }
    }
}
