using UnityEngine;

public class StretchWireScript : MonoBehaviour
{
	// call each frame
	public void StretchBetween(Vector2 source, Vector2 destination)
	{
		Transform myTransform = transform;

		Vector2 destinationVector = destination - source;

		Quaternion rotation = Quaternion.FromToRotation(Vector3.right, destinationVector.normalized);
		myTransform.localRotation = rotation;

		RectTransform myRectTransform = myTransform.GetComponent<RectTransform>();
		myRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, destinationVector.magnitude / transform.localScale.x);
	}
}