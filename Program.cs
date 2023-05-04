using System;
using System.Collections.Generic;
using System.IO;
using Slicer;

var parseResult = new RunnerOptionsParser().Parse(Environment.GetCommandLineArgs(), out RunnerOptions options);

if (!parseResult)
{
    return;
}

var legendReader = new StreamReader(options.InitialLegend);
var legendWriter = new StreamWriter(options.ResultLegend);

var indexes = new LinkedList<int>();
int idColumn = 0;
int positionColumn = 1;
int a0Column = 2;
int a1Column = 3;
string currentLine = "";
legendReader.ReadLine(); // skip header
legendWriter.WriteLine("id position a0 a1");
var currentRow = 1;
while ((currentLine = legendReader.ReadLine()) != null)
{
    var splitted = currentLine.Split(" ");
    var position = int.Parse(splitted[positionColumn]);
    if (position < options.StartPosition)
    {
        currentRow++;
    }
    else if (position > options.EndPosition)
    {
        break;
    }
    else
    {
        indexes.AddLast(currentRow++);
        legendWriter.WriteLine($"{splitted[idColumn]} {splitted[positionColumn]} {splitted[a0Column]} {splitted[a1Column]}");
    }
}

legendReader.Close();
legendWriter.Close();

var hapReader = new StreamReader(options.InitialHaps);
var hapWriter = new StreamWriter(options.ResultHaps);
var hapRowIndex = 0;
var indexEnumerator = indexes.GetEnumerator();
indexEnumerator.MoveNext();
var readChar = (char)hapReader.Peek();
while (readChar > -1)
{
    if (hapRowIndex < indexEnumerator.Current - 1)
    {
        SkipToEOL(hapReader);
    }
    else if (hapRowIndex == indexEnumerator.Current - 1)
    {
        WriteSlice(hapReader, hapWriter, readChar);
        SkipToEOL(hapReader);
        hapWriter.Write(Environment.NewLine);
        if (!indexEnumerator.MoveNext())
        {
            break;
        }
    }

    hapRowIndex++;
    readChar = (char)hapReader.Read();
}

hapReader.Close();
hapWriter.Close();

void SkipToEOL(StreamReader reader)
{
    char readChar = (char)reader.Read();
    while (readChar != '\n')
    {
        readChar = (char)reader.Read();
    }
}

void WriteSlice(StreamReader reader, StreamWriter writer, char currentChar)
{
    var columnIndex = 0;
    while (columnIndex < options.SampleCount)
    {
        while (currentChar != ' ')
        {
            if (currentChar == ' ')
            {
                break;
            }

            writer.Write(currentChar);
            currentChar = (char)reader.Read();
        }
        
        if (columnIndex != options.SampleCount - 1)
        {
            writer.Write(currentChar);
        }

        columnIndex++;
        currentChar = (char)reader.Read();
    }
}