using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace Bird
{
    public class FlapCharge : MonoBehaviour
    {
        public bool isUseable = true;

        private SpriteRenderer spriteRenderer;

        private Color onCooldownColour;
        private Color notOnCooldownColour;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            notOnCooldownColour = Color.white;
            onCooldownColour = new Color(0.196f,0.196f,0.196f);
        }

        private void Update()
        {
            //Debug.Log(spriteRenderer.color);
        }

        public void UseFlap()
        {
            isUseable = false;
            spriteRenderer.color = onCooldownColour;
            StartCoroutine(StartFlapCooldownRoutine());
            StartCoroutine(ChargeUpAnimationRoutine());
        }

        private IEnumerator StartFlapCooldownRoutine()
        {
            yield return new WaitForSeconds(3f);
            isUseable = true;
        }

        private IEnumerator ChargeUpAnimationRoutine()
        {
            Color c = new Color(0,0,0);
            
            for (float i = 0.196f; i < 1; i+= 0.067f)
            {
                c.r = i;
                c.b = i;
                c.g = i;

                spriteRenderer.color = c;

                yield return new WaitForSeconds(.25f);
            }
        }
    }
}