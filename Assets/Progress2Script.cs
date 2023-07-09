using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Core;

public class Progress2Script : MonoBehaviour
{
    [Header("Necessary")]
    [SerializeField] private WireButtonLeft button1;
    [SerializeField] private WireButtonLeft button2;
    [SerializeField] private WireButtonLeft button3;
    [SerializeField] private WireButtonLeft button4;

    [Header("Saving data")]
    [SerializeField] private Slider mixingSlider1;
    [SerializeField] private Slider mixingSlider2;
    [SerializeField] private WireButtonRight button1Right;
    [SerializeField] private WireButtonRight button2Right;
    [SerializeField] private WireButtonRight button3Right;
    [SerializeField] private WireButtonRight button4Right;
    private List<WireButtonRight> AllRightButtons => new List<WireButtonRight>
        {button1Right, button2Right, button3Right, button4Right};

    // Update is called once per frame
    private void Update()
    {
        if (button1.placed && button2.placed && button3.placed && button4.placed && gameObject.GetComponent<Button>().interactable == false)
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
    }

    public void Pressed()
    {
        SaveBombData();
        PersistentGameManager.Instance.ReturnToHub();
    }

    public void SaveBombData()
    {
        int mixingOffness = 0;
        mixingOffness += Mathf.RoundToInt(Mathf.Abs(mixingSlider1.value * 100 / 2 - 50));
        mixingOffness += Mathf.RoundToInt(Mathf.Abs(mixingSlider2.value * 100 / 2 - 50));
        PersistentGameManager.Instance.BombData.mixingOffness += mixingOffness;

        int incorrectWireNumber = AllRightButtons.Count(rightButton => !rightButton.ConnectedCorrectly);
        PersistentGameManager.Instance.BombData.incorrectWireNumber = incorrectWireNumber;
    }
}