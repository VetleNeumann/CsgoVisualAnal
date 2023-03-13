using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject _demoProviderObject;
    private DemoProvider _demoProvider;
    [SerializeField] private GameObject _timeControllerObject;
    private TimeController _timeController;

    // Score
    private TMP_Text _scoreTextField;
    private (int CTScore, int TScore) _lastDisplayedScore;

    // Start is called before the first frame update
    void Start()
    {
        _demoProvider = _demoProviderObject.GetComponent<DemoProvider>();
        _timeController = _timeControllerObject.GetComponent<TimeController>();

        _scoreTextField = gameObject.GetComponentInChildren<TMP_Text>();
        _lastDisplayedScore = (0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        int currentTick = _timeController.CurrentTick;
        (int CTScore, int TScore) currentScore = _demoProvider.Round.GetScore(currentTick);
        if (currentScore != _lastDisplayedScore)
        {
            _scoreTextField.text = $"{currentScore.CTScore} - {currentScore.TScore}";
            _lastDisplayedScore = currentScore;
        }
    }
}
