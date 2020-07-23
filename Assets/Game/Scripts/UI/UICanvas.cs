﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;

public class UICanvas : Singleton<UICanvas>
{
    public GameObject pauseMenu;
    public GameObject controlsMenu;
    public GameObject settingsMenu;
    public GameObject gameOverMenu;
    public GameObject gameFinishedMenu;

    [Space]
    public GameObject healthBar;


    private Canvas _canvas;
    private Slider _healthSlider;

    [SerializeField] private InputAction _pauseAction;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        
        if(_canvas == null)
        {
            Debug.LogError("Canvas not found.");
        }

        _canvas.worldCamera = Camera.main;
        _canvas.planeDistance = 0.05f;

        _healthSlider = healthBar.GetComponent<Slider>();

        
    }

    private void Start()
    {
        InputManager.Instance.OnGameStateChange += OnGameStateChanged;
    }

    private void OnGameStateChanged(InputManager.GameStates state)
    {
        switch (state)
        {
            case InputManager.GameStates.Playing:
                pauseMenu.SetActive(false);
                controlsMenu.SetActive(false);
                settingsMenu.SetActive(false);
                gameOverMenu.SetActive(false);
                gameFinishedMenu.SetActive(false);
                healthBar.SetActive(true);
                Time.timeScale = 1;
                break;

            case InputManager.GameStates.Paused:
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                healthBar.SetActive(false);
                break;

            case InputManager.GameStates.Reloading:
                PlayerData data = SaveLoadSystem.LoadPlayerData();

                if (data != null)
                {
                    CheckpointManager.Instance.LoadCheckpoint(data);
                }

                InputManager.Instance.currentGameState = InputManager.GameStates.Playing;
                break;

            case InputManager.GameStates.Resetting:
                UICanvas.instance.UnbindControls();
                Time.timeScale = 1;
                InputManager.Instance.currentGameState = InputManager.GameStates.Playing;
                break;

            case InputManager.GameStates.GameOver:
                gameOverMenu.SetActive(true);
                healthBar.SetActive(false);
                break;

            case InputManager.GameStates.GameFinished:
                gameFinishedMenu.SetActive(true);
                healthBar.SetActive(false);
                Time.timeScale = 0;
                break;
        }
    }

    public void LoadLastCheckpoint()
    {
        InputManager.Instance.currentGameState = InputManager.GameStates.Reloading;
    }

    public void PauseGame()
    {
        InputManager.Instance.currentGameState = InputManager.GameStates.Paused;
    }

    public void ResumeGame()
    {
        InputManager.Instance.currentGameState = InputManager.GameStates.Playing;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ChangeHealthBar(float newValue)
    {
        _healthSlider.value = newValue;
    }
    private void OnPauseButtonDown(InputAction.CallbackContext context)
    {
        if (InputManager.Instance.currentGameState == InputManager.GameStates.Playing)
        {
            PauseGame();
        }
        else if(InputManager.Instance.currentGameState == InputManager.GameStates.Paused)
        {
            ResumeGame();
        }
    }

    public void BindControls()
    {
        _pauseAction.started += OnPauseButtonDown;
    }

    public void UnbindControls()
    {
        _pauseAction.started -= OnPauseButtonDown;
    }
    private void OnEnable()
    {
        _pauseAction.Enable();
    }

    private void OnDisable()
    {
        _pauseAction.Disable();
    }
}
