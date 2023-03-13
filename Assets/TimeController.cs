using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    [SerializeField] private float _timeScale = 1f;

    public float TimeElapsed { get; private set; } = 0;
    public int CurrentTick { get; private set; } = 0;
    public float TickRate { get; private set; } = 16;

    [SerializeField] private GameObject _demoProviderObject;
    private DemoProvider _demoProvider;

    // Start is called before the first frame update
    void Start()
    {
        _demoProvider = _demoProviderObject.GetComponent<DemoProvider>();
        CurrentTick = 0;
        TickRate = _demoProvider.TickRate;
    }

    // Update is called once per frame
    void Update()
    {
        TimeElapsed += Time.deltaTime * _timeScale;
        CurrentTick = Mathf.RoundToInt(TimeElapsed * TickRate);
        CurrentTick = Mathf.Min(CurrentTick, _demoProvider.FinalTick);
    }
}
