// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.IO;
using conlib;
using pathgen;

namespace Program;



public class MainClass
{
    public static void Main(string[] args)
    {

        string definitionsPath = args[0];
        string? dirPath = args[1];

        string paramText = File.ReadAllText(definitionsPath);
        Dictionary<string, string> paramDict = ParamLoader.GetParamDict(
            paramText
        );


        Console.WriteLine($"starting replacements in {dirPath}");
        int totalFiles = 0;
        int replacedFiles = 0;

        FilePathGen fileGen = new FilePathGen(dirPath);

        foreach (string filePath in fileGen)
        {
            string fileText = File.ReadAllText(filePath);
            fileText = ParamReplacer.ReplaceParams(fileText, paramDict);
            replacedFiles++;
            File.WriteAllText(filePath, fileText);
        }

        Console.WriteLine($"files visited: {totalFiles}");
        Console.WriteLine($"files .py files visited: {replacedFiles}");
    }
}
