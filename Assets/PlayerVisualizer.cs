using DemoInfo;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject _demoProviderObject;
    private DemoProvider _demoProvider;
    [SerializeField] private GameObject _timeControllerObject;
    private TimeController _timeController;

    [SerializeField] private GameObject _playerModelPrefab;
    [SerializeField] private Material _counterTerroristMaterial;
    [SerializeField] private Material _terroristMaterial;

    private List<long> _players;
    private Dictionary<long, GameObject> _playerModels;
    private Dictionary<long, Team> _previousTeam;

    private long poppin;

    // Start is called before the first frame update
    void Start()
    {
        _demoProvider = _demoProviderObject.GetComponent<DemoProvider>();
        _timeController = _timeControllerObject.GetComponent<TimeController>();

        _players = _demoProvider.Players.GetPlayers();
        _playerModels = new Dictionary<long, GameObject>();
        _previousTeam = new Dictionary<long, Team>();

        foreach (long steamId in _players)
        {
            if (_demoProvider.Players.GetPlayerName(steamId) == "Poppin")
            {
                poppin = steamId;
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        int currentTick = _timeController.CurrentTick;
        foreach (long steamId in _players)
        {
            UpdatePlayerModel(steamId, currentTick);
        }
    }

    private void UpdatePlayerModel(long steamId, int tick)
    {
        // Remove/Instantiate Player Model
        bool playerModelExistsForPlayer = _playerModels.ContainsKey(steamId);
        Vector? positionVector = _demoProvider.Players.GetPlayerPosition(steamId, tick);
        Vector? viewDirectionVector = _demoProvider.Players.GetPlayerViewDirection(steamId, tick);
        if (positionVector == null)
        {
            if (playerModelExistsForPlayer)
            {
                Debug.Log("Destroying...");
                Destroy(_playerModels[steamId]);
                _playerModels.Remove(steamId);
                _previousTeam.Remove(steamId);
            }
            return;
        }
        Vector3 position = Util.DemoInfoVecToVec3(positionVector);
        Vector2 viewDirection = Util.DemoInfoVecToVec2(viewDirectionVector);
        Team team = _demoProvider.Players.GetPlayerTeam(steamId, tick) ?? Team.Spectate;
        
        if (!playerModelExistsForPlayer)
        {
            Debug.Log($"Instantiating PlayerModel at {position}");
            GameObject newPlayerModel = Instantiate(_playerModelPrefab, Vector3.zero, Quaternion.identity);
            newPlayerModel.transform.parent = transform;
            _playerModels.Add(steamId, newPlayerModel);
            _previousTeam.Add(steamId, team);
        }

        // Update Material depending on team
        GameObject playerModel = _playerModels[steamId];
        if (!playerModelExistsForPlayer || team != _previousTeam[steamId])
        {
            Debug.Log("Updating material..");
            Material newMaterial = (team == Team.CounterTerrorist ? _counterTerroristMaterial : _terroristMaterial);
            playerModel.GetComponentInChildren<Renderer>().material = newMaterial;
            _previousTeam[steamId] = team;
        }
        
        // Update Transform
        playerModel.transform.position = position;
        playerModel.transform.rotation = Quaternion.Euler(0, -viewDirection.x, 0);

        if (steamId == poppin)
        {
            //Debug.Log(viewDirection);
        }
    }
}
