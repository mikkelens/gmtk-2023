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
	        NAME = (NAME.input.text, NAME.input.text.ToCharArray() == NAME.expected.text.ToCharArray()),
	        DOB = (DOB.input.text, DOB.input.text.ToCharArray() == DOB.expected.text.ToCharArray()),
	        Sex = (Sex.input.text, Sex.input.text.ToCharArray() == Sex.expected.text.ToCharArray()),
	        ISS = (ISS.input.text, ISS.input.text.ToCharArray() == ISS.expected.text.ToCharArray()),
	        EXP = (EXP.input.text, EXP.input.text.ToCharArray() == EXP.expected.text.ToCharArray()),
	        ID = (ID.input.text, ID.input.text.ToCharArray() == ID.expected.text.ToCharArray())
        };
        PersistentGameManager.Instance.NextSegment();
    }
}