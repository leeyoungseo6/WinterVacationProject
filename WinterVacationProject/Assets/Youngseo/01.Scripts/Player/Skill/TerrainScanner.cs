using System.Collections;
using UnityEngine;

namespace YS
{
    public class TerrainScanner : PoolableMono
    {
        private void Start()
        {
            StartCoroutine(DOScale());
        }

        private IEnumerator DOScale()
        {
            float currentTime = 0, percent = 0, time = 3f;
            Vector3 startScale = Vector3.zero, endScale = new(80, 80, 80);
            
            while (percent < 1)
            {
                currentTime += Time.deltaTime;
                percent = currentTime / time;
                transform.localScale = Vector3.Lerp(startScale, endScale,
                    -(Mathf.Cos(Mathf.PI * percent) - 1) / 2);
                yield return null;
            }

            PoolManager.Instance.Push(this);
        }

        public override void Init()
        {
            StartCoroutine(DOScale());
        }
    }
}