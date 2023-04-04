using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PersonDB", menuName = "ScriptableObjects/PersonalityDatabase", order = 1)]
public class PersonalityDatabase : ScriptableObject
{
	public Personality[] personalities;

	[System.Serializable]
	public struct Personality
	{
		public string name;
		[TextArea(20, 100)]
		public string initialPrompt;
	}
}
