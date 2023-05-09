using System.Collections;
using UnityEngine;

namespace Assets.Scripts.NPCs
{
    public class EnemyBird : MonoBehaviour
    {

        private SpriteRenderer _spriteRenderer;
        private Rigidbody2D _rb;
        public bool _flyRight;



        // Use this for initialization
        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rb = GetComponent<Rigidbody2D>();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {

            if (collision.gameObject.tag == "Enemy")
            {
                Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            }

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            MoveBird();
        }

        private void MoveBird()
        {
            if (_flyRight)
            {
                _rb.MovePosition(transform.position + (new Vector3(1, 0, 0) * Time.deltaTime * 5));
            } else
            {
                _rb.MovePosition(transform.position + (new Vector3(- 1, 0, 0) * Time.deltaTime * 5));
                _spriteRenderer.flipX = true;

            }
        }

        private void AdjustPlayerFacingDirection()
        {
/*            if (flyRight)
            {
                _spriteRenderer.flipY = true;
            }
            else
            {
                _spriteRenderer.flipY = false;
            }*/
        }
    }
}