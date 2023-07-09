using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
	        NAME = (NAME.input.text, NAME.input.text.SequenceEqual(NAME.expected.text)),
	        DOB = (DOB.input.text, DOB.input.text.SequenceEqual(DOB.expected.text)),
	        Sex = (Sex.input.text, Sex.input.text.SequenceEqual(Sex.expected.text)),
	        ISS = (ISS.input.text, ISS.input.text.SequenceEqual(ISS.expected.text)),
	        EXP = (EXP.input.text, EXP.input.text.SequenceEqual(EXP.expected.text)),
	        ID = (ID.input.text, ID.input.text.SequenceEqual(ID.expected.text))
        };
        PersistentGameManager.Instance.NextSegment();
    }
}