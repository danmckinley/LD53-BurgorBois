using UnityEngine;
using UnityEngine.Events;

public class LandingPadController : MonoBehaviour
{
    [SerializeField] public UnityEvent babyDelivered;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Baby"))
        {
            gameObject.SetActive(false);
            Destroy(other.gameObject);
            babyDelivered?.Invoke();
        }
    }
}