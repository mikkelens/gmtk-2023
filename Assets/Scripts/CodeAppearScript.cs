using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class CodeAppearScript : MonoBehaviour
{
    [SerializeField] private string text = "REMEMBER ";
    [SerializeField, Required] private TextMeshProUGUI textComponent;

    private void Start()
    {
        textComponent.text = text + string.Join("", PersistentGameManager.Instance.BombData.PIN);
    }
}