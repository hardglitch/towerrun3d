using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private PlayerTower playerTower;
    [SerializeField] private Vector3 offsetPosition, offsetRotation;
    [SerializeField] private float cameraSpeed;
    private Vector3 targetOffsetPosition;

    private void Update()
    {
        UpdatePosition();
        offsetPosition = Vector3.MoveTowards(offsetPosition, targetOffsetPosition, cameraSpeed * Time.deltaTime);
    }

    private void OnEnable()
    {
        playerTower.HumanoidAdded += OnHumanoidAdded;
    }

    private void OnDisable()
    {
        playerTower.HumanoidAdded -= OnHumanoidAdded;
    }

    private void OnHumanoidAdded(int count)
    {
        targetOffsetPosition += offsetPosition + (Vector3.up + Vector3.back) * count / 10;
        UpdatePosition();
    }
    
    private void UpdatePosition()
    {
        transform.position = playerTower.transform.position;
        transform.localPosition += offsetPosition;

        var lookAtPoint = playerTower.transform.position + offsetRotation;
        transform.LookAt(lookAtPoint);
    }
}