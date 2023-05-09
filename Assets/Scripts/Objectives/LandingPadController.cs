using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class LandingPadController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Baby"))
        {
            Destroy(other.gameObject);
        }
    }
}
