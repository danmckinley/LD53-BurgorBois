using UnityEngine;

public class LandingPadController : MonoBehaviour
{
    private CapsuleCollider2D capsuleCollider;

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Baby"))
        {
            Destroy(other.gameObject);
        }
    }
}
