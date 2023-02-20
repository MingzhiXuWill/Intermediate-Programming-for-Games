using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
	[SerializeField]
	AudioClip bgm;

	private void Start()
	{
		MusicManager.instance.SwitchMusic(bgm);
	}

	public void GameStart()
	{
		SceneManager.LoadScene("GameScene");
	}
}
