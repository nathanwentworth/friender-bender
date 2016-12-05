using UnityEngine;
using System.Collections;

namespace Kino
{

    public class GradientEval : MonoBehaviour {

        private Contour ct;
        public Gradient grad;

        void Start() {
            ct = GetComponent<Contour>();
            StartCoroutine(ColorGrad());
        }

        IEnumerator ColorGrad()
        {
            float timer = Random.value;
            while (true)
            {
                timer += .001f;
                if (timer >= 1) timer = 0;
                ct.lineColor = grad.Evaluate(timer);
                yield return null;
            }
        }
    }
}
