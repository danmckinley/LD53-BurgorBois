using Assets.Scripts.NPCs;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class EnemyBirdGenerator : MonoBehaviour
    {

        [SerializeField] private EnemyBird objectToSpawn;

        public bool _flyRight = false;
        private SpriteRenderer _spriteRenderer;


        void Start()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            StartCoroutine(StartGenerationRoutine());
        }

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.tag.Equals("Enemy"))
            {
                 Destroy(collider.gameObject);
            }
        }

        private IEnumerator StartGenerationRoutine()
        {
            var randomSecs = Random.Range(5, 15);
            yield return new WaitForSeconds(randomSecs);
            var enemyBird = Instantiate(objectToSpawn);
            enemyBird._flyRight = _flyRight;
            enemyBird.transform.position = GetStartPosition();
            StartCoroutine(StartGenerationRoutine());
        }

        private Vector2 GetStartPosition()
        {
            var wallCentre = _spriteRenderer.transform.position;
            var wallTop = wallCentre.y + _spriteRenderer.bounds.extents.y;
            var wallBottom = wallCentre.y - _spriteRenderer.bounds.extents.y;

            var randomYPos = Random.Range(wallBottom, wallTop);

            if (_flyRight)
            {
                var xPos = wallCentre.x + _spriteRenderer.bounds.extents.x + 1;
                return new Vector2(xPos, randomYPos);
            } else
            {
                var xPos = wallCentre.x - _spriteRenderer.bounds.extents.x - 1;
                return new Vector2(xPos, randomYPos);
            }
        }


       
    }
}