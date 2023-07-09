using UnityEngine;

namespace Core.Dialogue
{
	public class DialogueButtonScript : MonoBehaviour
	{
		public int MyIndex { get; set; }

		public void SendInteraction()
		{
			DialogueScript.Instance.ChooseOption(MyIndex);
		}
	}
}