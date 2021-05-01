using System;
using PathCreation;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTower : MonoBehaviour
{
    [SerializeField] private Humanoid startHumanoidPrefab;
    [SerializeField] private Transform playerDistanceChecker;
    [SerializeField] private float maxDistanceFromFixPoint;
    [SerializeField] private BoxCollider playerTowerBoxCollider;
    [SerializeField] private PathCreator pathCreator;
    private List<Humanoid> humanoids;
    private Vector3 finishPoint;

    public event UnityAction<int> HumanoidAdded; 

    private void Start()
    {
        humanoids = new List<Humanoid>();
        var firstHumanoid = Instantiate(startHumanoidPrefab, transform.position, Quaternion.identity, transform);
        humanoids.Add(firstHumanoid);
        humanoids[0].Run();
        
        HumanoidAdded?.Invoke(humanoids.Count);
        finishPoint = pathCreator.path.GetPointAtDistance(pathCreator.path.length, EndOfPathInstruction.Stop);
    }

    private bool IsFinish() => (int)Vector3.Distance(finishPoint, transform.position) == 0;

    private void FixedUpdate()
    {
        if (IsFinish()) humanoids[0].Stop();
    }

    private void OnTriggerEnter(Collider other)
    {
        humanoids[0].Stop();
        if (other.gameObject.TryGetComponent(out Humanoid humanoid))
        {
            var humanoidTower = humanoid.GetComponentInParent<Tower>();
            var collectedHumanoids = humanoidTower?.CollectHumanoidInTower(playerDistanceChecker, maxDistanceFromFixPoint);
            if (collectedHumanoids != null)
            {
                for (var i = collectedHumanoids.Count - 1; i >= 0; i--)
                {
                    var insertingHumanoid = collectedHumanoids[i];
                    InsertHumanoidInPlayerTower(insertingHumanoid);
                }
                HumanoidAdded?.Invoke(humanoids.Count);
            }

            humanoidTower?.BreakTower();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        humanoids[0].Run();
    }

    private void InsertHumanoidInPlayerTower(Humanoid insertingHumanoid)
    {
        humanoids.Insert(0, insertingHumanoid);
        insertingHumanoid.transform.parent = transform;
        
        var fixPointY = insertingHumanoid.FixationPoint.localPosition.y;
        var localOffsetInPlayerTowerY = fixPointY * insertingHumanoid.transform.localScale.y;
        
        insertingHumanoid.transform.localPosition = new Vector3(0, -localOffsetInPlayerTowerY * (humanoids.Count - 1), 0);
        insertingHumanoid.transform.localRotation = Quaternion.identity;

        var tmpPos = playerDistanceChecker.position;
        playerDistanceChecker.position = new Vector3(tmpPos.x, tmpPos.y - localOffsetInPlayerTowerY, tmpPos.z);

        playerTowerBoxCollider.center = playerDistanceChecker.localPosition;
    }
}