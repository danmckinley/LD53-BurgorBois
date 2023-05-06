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

        [SerializeField] private Sprite rechargedSprite;
        public float flapCooldownTotalSecs = 2.75f;
        private float rechargeIntervalSecs;
        private int cooldownIterations;

        private Sprite defaultSprite;
        private Color rechargedColour;
        private Color onCooldownColour;
        private Color rechargeRGBDiffColour;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            defaultSprite = spriteRenderer.sprite;
            onCooldownColour = new Color(0.196f, 0.196f, 0.196f);
            rechargedColour = Color.white;
            rechargeIntervalSecs = .25f;
            flapCooldownTotalSecs = 2.75f;
        }

        public void UseFlap()
        {
            isUseable = false;
            cooldownIterations = (int) Math.Ceiling(flapCooldownTotalSecs / rechargeIntervalSecs);
            var rgbDiff = (float) ((1f - onCooldownColour.r) / cooldownIterations);
            rechargeRGBDiffColour = new Color(rgbDiff, rgbDiff, rgbDiff);
            spriteRenderer.color = onCooldownColour;
            StartCoroutine(FlapCooldownRoutine(0, onCooldownColour));
        }

        private IEnumerator FlapCooldownRoutine(int currI, Color currCooldownColour)
        {
            yield return new WaitForSeconds(rechargeIntervalSecs);

            if (currI < cooldownIterations) 
            {
                Color newColour = currCooldownColour + rechargeRGBDiffColour;
                spriteRenderer.color = newColour;
                StartCoroutine(FlapCooldownRoutine(currI + 1, newColour));
            } else {
                spriteRenderer.color = rechargedColour;
                spriteRenderer.sprite = rechargedSprite;
                yield return new WaitForSeconds(rechargeIntervalSecs);
                spriteRenderer.sprite = defaultSprite;
                isUseable = true;
            }
        }
    }
}