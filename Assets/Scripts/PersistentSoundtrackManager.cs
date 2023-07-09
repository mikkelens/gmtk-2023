using Tools.Types;
using UnityEngine;

public class PersistentSoundtrackManager : PersistentSingleton<PersistentSoundtrackManager>
{
	[SerializeField] private AudioClip backgroundMusic;
	[SerializeField] private AudioSource mySource;

	private void Start()
	{
		mySource.clip = backgroundMusic;
		mySource.Play();
	}
}