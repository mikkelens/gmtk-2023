using System;
using System.Diagnostics.CodeAnalysis;
using Core;
using Core.Data;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public class IDMakerReturn : MonoBehaviour
{
	[Serializable]
	[InlineProperty]
	private struct TextMatch
	{
		[HorizontalGroup]
		public TextMeshProUGUI input;
		[HorizontalGroup]
		public TextMeshProUGUI expected;
	}

    [SerializeField, Required] private TextMatch NAME;
    [SerializeField, Required] private TextMatch DOB;
    [SerializeField, Required] private TextMatch Sex;
    [SerializeField, Required] private TextMatch ISS;
    [SerializeField, Required] private TextMatch EXP;
    [SerializeField, Required] private TextMatch ID;

    public void Return()
    {
        PersistentGameManager.Instance.Passport = new Passport
        {
	        NAME = (NAME.input.text, NAME.input.text == NAME.expected.text),
	        DOB = (DOB.input.text, DOB.input.text == DOB.expected.text),
	        Sex = (Sex.input.text, Sex.input.text == Sex.expected.text),
	        ISS = (ISS.input.text, ISS.input.text == ISS.expected.text),
	        EXP = (EXP.input.text, EXP.input.text == EXP.expected.text),
	        ID = (ID.input.text, ID.input.text == ID.expected.text)
        };
        PersistentGameManager.Instance.NextSegment();
    }
}