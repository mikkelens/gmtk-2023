using UnityEngine;

public class ButtonHoldScript : MonoBehaviour
{
    public bool pressed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ButtonClicked()
    {
        pressed = !pressed;
    }
}
