using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class BotCommander : MonoBehaviour
{
    [SerializeField] private List<Bot> _bots = new List<Bot>();
    [SerializeField] private BotDockStation _botDockStationPrefab;
    [SerializeField] private Transform _crystalStorage;

    private BotBaseSpawner _botBaseSpawner;

    private int _botsStationsMaxWidth = 3;
    private int _currentStationWidthIndex = 0;
    private int _currentStationLengthIndex = 0;

    private Vector3 _botStartPosition;
    private float _gapBetweenBots = 1.5f;

    private bool _isInitialized = false;

    public int BotsUnderCommand => _bots.Count;

    private Action _onCrystalDelivered;

    private void Start()
    {
        _botStartPosition = transform.position;

        CreateNewBot();
    }

    private void OnDisable()
    {
        if (_bots.Count == 0)
            return;

        foreach (Bot bot in _bots)
        {
            bot.CrystalDelivered -= _onCrystalDelivered;
        }
    }

    public void Initialize(Action OnCrystalDelivered, BotBaseSpawner botBaseSpawner)
    {
        _onCrystalDelivered = OnCrystalDelivered;
        _botBaseSpawner = botBaseSpawner;
        _isInitialized = true;
    }

    public void AquireTargets(IReadOnlyList<Crystal> _targets)
    {
        int targetIndex = 0;

        foreach (Bot bot in _bots)
        {
            if (bot.IsBusy)
                continue;

            if (targetIndex >= _targets.Count)
                return;

            bot.AquireTarget(_targets[targetIndex].transform.position);
            _targets[targetIndex].OnTargeted();
            targetIndex++;
        }
    }

    public void CreateNewBot()
    {
        if (_isInitialized == false)
            throw new UnityException("Trying to create bot while BotCommander(" + gameObject.name + ") isn't initialized!");

        Vector3 position = _botStartPosition + new Vector3(_currentStationWidthIndex, 0, -_currentStationLengthIndex) * _gapBetweenBots;
        BotDockStation newBotDockStation = Instantiate(_botDockStationPrefab, position, Quaternion.identity);
        newBotDockStation.transform.parent = transform;
        newBotDockStation.transform.forward = transform.forward;
        Bot newBot = newBotDockStation.GetComponentInChildren<Bot>(true);
        newBot.Initialize(_crystalStorage);
        newBot.CrystalDelivered += _onCrystalDelivered;
        _bots.Add(newBot);

        _currentStationWidthIndex++;

        if (_currentStationWidthIndex >= _botsStationsMaxWidth)
        {
            _currentStationWidthIndex = 0;
            _currentStationLengthIndex++;
        }
    }

    public void SendBotToBuildBase(Vector3 position)
    {
        Bot lastBot = _bots.Last();
        lastBot.BuildBase(position, _botBaseSpawner);
        BotDockStation botDockStation = lastBot.GetComponentInParent<BotDockStation>();
        lastBot.transform.parent = null;
        _bots.Remove(lastBot);
        Destroy(botDockStation.gameObject);

        _currentStationWidthIndex--;

        if (_currentStationWidthIndex < 0)
        {
            _currentStationWidthIndex = _botsStationsMaxWidth;
            _currentStationLengthIndex--;
        }
    }
}