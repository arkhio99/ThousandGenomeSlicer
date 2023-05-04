using System;
using System.IO;
using NDesk.Options;
using Newtonsoft.Json;

namespace Slicer;

public class RunnerOptionsParser
{
    private RunnerOptions _options;
    private bool _showHelp = false;
    private readonly OptionSet _optionsSet;

    public RunnerOptionsParser()
    {
        _options = new RunnerOptions();
        _optionsSet = new OptionSet
        {
            { "ih|initial-hap=", "Path to initial .hap file", v => _options.InitialHaps = v },
            { "rh|result-hap=", "Path to result .hap file", v => _options.ResultHaps = v },
            { "il|initial-legend=", "Path to initial .legend file", v => _options.InitialLegend = v },
            { "rl|result-legend=", "Path to result .legend file", v => _options.ResultLegend = v },
            { "sp|start-position=", "Start snp position", v => _options.StartPosition = TryParseInt(v, "sp|start-position") },
            { "ep|end-position=", "End snp position", v => _options.StartPosition = TryParseInt(v, "ep|end-position") },
            { "sc|sample-count=", "Number of samples, should be lower than sample count in .hap file", v => _options.SampleCount = TryParseInt(v, "sc|sample-count") },
            { "config=", "Path to config", v => _options = JsonConvert.DeserializeObject<RunnerOptions>(File.ReadAllText(v)) },
            { "help", "Show help", v => _showHelp = true },
        };
    }

    public bool Parse(string[] args, out RunnerOptions options)
    {
        _optionsSet.Parse(args);

        if (_showHelp)
        {
            options = default(RunnerOptions);
            _optionsSet.WriteOptionDescriptions(Console.Out);
            return false;
        }

        options = _options;
        return true;
    }

    private int TryParseInt(string value, string argumentName)
    {
        if (int.TryParse(value, out int parsedValue))
        {
            return parsedValue;                                    
        }
        else
        {
            throw new Exception($"{argumentName} is not integer number");
        };
    }
}