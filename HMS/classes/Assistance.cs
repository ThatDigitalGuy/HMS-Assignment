using System;
using System.Speech.Synthesis;

namespace HMS.classes;

public class Assistance
{
    // If we need the Text-to-Speech in the console.
    public bool AssistiveTechnology { get; set; } = false;
    
    // Initialise the global speech module
    private readonly Utility _utils = new();
    private SpeechSynthesizer _speech = new SpeechSynthesizer
    {
        Rate = 0,
        Volume = 0
    };

    public void TestModule()
    {
        _utils.WriteToLogFile("[Assistive] Testing the 'Audio Synth' module. You may hear audio.");

        _speech.SpeakAsync("Test");
    }
}