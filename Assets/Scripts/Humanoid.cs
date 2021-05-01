using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Humanoid : MonoBehaviour
{
    [SerializeField] private Transform fixationPoint;
    public Transform FixationPoint => fixationPoint;
    private Animator animator;
    private readonly int isRunning = Animator.StringToHash("isRunning");

    private void Awake()
    {
        if (TryGetComponent(out Animator _animator))
        {
            animator = _animator;
        }
    }

    public void Run() => animator.SetBool(isRunning, true);
    public void Stop() => animator.SetBool(isRunning, false);
}
