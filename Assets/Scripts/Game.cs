﻿using System;
using Common.AppModes;
using Common.Extensions;
using Common.Interfaces;

public class Game : IDisposable
{
    private readonly DrawGameBoardMode _drawGameBoardMode;
    private readonly GameInitMode _gameInitMode;
    private readonly GamePlayMode _gamePlayMode;
    private readonly GameResetMode _gameResetMode;

    private IAppMode _activeMode;

    public Game(IAppContext appContext)
    {
        _drawGameBoardMode = new DrawGameBoardMode(appContext);
        _gameInitMode = new GameInitMode(appContext);
        _gamePlayMode = new GamePlayMode(appContext);
        _gameResetMode = new GameResetMode(appContext);
    }

    public void Start()
    {
        ActivateMode(_drawGameBoardMode);
    }

    public void Enable()
    {
        _drawGameBoardMode.Finished += OnDrawGameBoardModeFinished;
        _gameInitMode.Finished += OnGameInitModeFinished;
        _gamePlayMode.Finished += OnGamePlayModeFinished;
        _gameResetMode.Finished += OnGameResetModeFinished;
    }

    public void Disable()
    {
        _drawGameBoardMode.Finished -= OnDrawGameBoardModeFinished;
        _gameInitMode.Finished -= OnGameInitModeFinished;
        _gamePlayMode.Finished -= OnGamePlayModeFinished;
        _gameResetMode.Finished -= OnGameResetModeFinished;
    }

    public void Dispose()
    {
        _gameInitMode.Dispose();
    }

    private void OnDrawGameBoardModeFinished(object sender, EventArgs e)
    {
        ActivateMode(_gameInitMode);
    }

    private void OnGameInitModeFinished(object sender, EventArgs e)
    {
        ActivateMode(_gamePlayMode);
    }

    private void OnGamePlayModeFinished(object sender, EventArgs e)
    {
        ActivateMode(_gameResetMode);
    }

    private void OnGameResetModeFinished(object sender, EventArgs e)
    {
        ActivateMode(_drawGameBoardMode);
    }

    private void ActivateMode(IAppMode mode)
    {
        _activeMode?.Deactivate();
        _activeMode = mode;
        _activeMode.Activate();
    }
}