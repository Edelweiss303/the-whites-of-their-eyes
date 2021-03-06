using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : Menu
{
	public SceneReference sceneToLoad;
	public SceneReference sceneToUnload;

	public void onLoadScene()
	{
		// Clear all previously cached traps
		CheckpointManager.obstacleDictionary.Clear();

		// Unbind player controls
		InputManager.Instance.UnbindControls();

		// Disable UI controls
		UICanvas.Instance.DisableAllControls();

		// Hide the pause menu
		UICanvas.Instance.pauseMenu.SetActive(false);

		InputManager.Instance.currentGameState = InputManager.GameStates.Resetting;
		SceneLoader.Instance.UnloadScene(sceneToUnload);
		SceneLoader.Instance.LoadScene(sceneToLoad, true);
		MenuManager.Instance.hideMenu(menuClassifier);
	}
}
