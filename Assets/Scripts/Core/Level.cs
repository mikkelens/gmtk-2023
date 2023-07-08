using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Tools.Types;
using UnityEngine;

namespace Core
{
	[Serializable]
	public class Level
	{
		[field: SerializeField, UsedImplicitly] private string Name { get; set; } = "Unnamed Level";
		[field: SerializeField] public List<Segment> PreparationSegments { get; private set; }
		[field: SerializeField] public List<ActionSegment> ActionSegments { get; private set; }
		[field: SerializeField] public List<SceneReference> Ending { get; private set; }

		[NonSerialized] public List<Segment> remainingSegments;

		[Serializable]
		public class Segment
		{
			[field: SerializeField] public SceneReference SegmentScene { get; private set; }
		}
		[Serializable]
		public class ActionSegment : Segment
		{
			[field: SerializeField] public SceneReference FailScene { get; private set; }
		}
	}
}