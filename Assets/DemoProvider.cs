#nullable enable
using CSGO;
using DemoInfo;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class DemoProvider : MonoBehaviour
{
    [SerializeField] private string DemoPath = "Demos/demo1.dem";
    public float TickRate { get; private set; } = 16;
    public int FinalTick { get; private set; }

    // Demo Trackers
    public PlayerTracker Players;
    public RoundTracker Round;
    
    private DemoParser _parser;
    int count = 0;

    // Start is called before the first frame update
    void Awake()
    {
        ParseDemo(DemoPath);
        TickRate = _parser.TickRate;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ParseDemo(string demoPath)
    {
        // Set-up
        _parser = new(File.OpenRead(demoPath));
        Players = new PlayerTracker(_parser);
        Round = new RoundTracker(_parser);

        // Parse
        Debug.Log($"Parsing {demoPath}..");
        _parser.ParseHeader();
        //Debug.Log(_parser.Map);
        //Debug.Log(_parser.Header.PlaybackTicks);

        _parser.TickDone += Parser_TickDone;

        _parser.ParseToEnd();
        FinalTick = _parser.CurrentTick - 2;
        Debug.Log($"Parsed successfully! Total number of ticks: {FinalTick + 1}");
    }

    private void Parser_TickDone(object? sender, TickDoneEventArgs e)
    {
        count++;   
        Players.Process_CurrentTick();
        Round.Process_CurrentTick();
    }
}
