using UnityEngine;
using PathCreation;

[RequireComponent(typeof(Rigidbody))]
public class PathFollower : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private PathCreator pathCreator;
    private new Rigidbody rigidbody;
    private float distanceTravelled;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        distanceTravelled += Time.fixedDeltaTime * speed;
        if (pathCreator is null) return;
        var nextPoint = pathCreator.path.GetPointAtDistance(distanceTravelled, EndOfPathInstruction.Stop);
        nextPoint.y = transform.position.y;
        transform.LookAt(nextPoint);
        rigidbody.MovePosition(nextPoint);
    }
}
