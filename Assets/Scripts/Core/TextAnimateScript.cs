using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace Core
{
    public class TextAnimateScript : MonoBehaviour
    {
        [SerializeField] private float animateSpeed = 20f; // letters per second
        [SerializeField] private bool animateFilledTextAtStartup = true;
        [DisableIf(nameof(animateFilledTextAtStartup))]
        [SerializeField] private bool removeTextAtStartup = false;
        [SerializeField, Required] private TextMeshProUGUI textComponent;

        // Start is called before the first frame update
        void Start()
        {
            if (animateFilledTextAtStartup)
            {
                StartCoroutine(AnimateText(textComponent.text));
            }
            else if (removeTextAtStartup)
            {
                textComponent.text = "";
            }
        }
        public bool Animating { get; private set; }
        public IEnumerator AnimateText(string text)
        {
            Animating = true;
            ClearText();
            float letterDelay = 1f / animateSpeed;
            foreach (char character in text)
            {
                yield return new WaitForSeconds(letterDelay);
                textComponent.text += character;
            }
            Animating = false;
        }
        public void ClearText()
        {
            textComponent.text = "";
        }
    }
}