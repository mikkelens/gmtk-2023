using Tools.Types;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Core
{
	public class PersistentUIManager : PersistentSingleton<PersistentUIManager>
	{
		[SerializeField] private Image loadFadeImage;
		[SerializeField] private AnimationCurve loadFadeCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

		public void SetTransitionImageWithT(float t)
		{
			loadFadeImage.color = Color.black.WithAlpha(loadFadeCurve.Evaluate(t));
		}
	}
}