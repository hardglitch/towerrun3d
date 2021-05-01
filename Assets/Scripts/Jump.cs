using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Jump : MonoBehaviour
{
    [SerializeField] private float jumpForce = 300f;
    private new Rigidbody rigidbody;
    private bool isJump;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnMouseDown()
    {
        if (isJump) return;
        rigidbody.AddForce(Vector3.up * jumpForce);
        isJump = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out Ground _))
        {
            isJump = false;
        }
    }
}
