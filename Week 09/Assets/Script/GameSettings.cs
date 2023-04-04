using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettingsDB", menuName = "ScriptableObjects/GameSettings", order = 1)]
public class GameSettings : ScriptableObject
{
	public float gametimer = 0;

	public int selectedIndex;

	public void Reset()
	{
		gametimer = -99;
		selectedIndex = -1;
	}
}
