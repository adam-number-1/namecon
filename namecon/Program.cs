// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.IO;
using conlib;

namespace Program;

public class DirQueue{
    private DirQueueNode? first = null;
    private DirQueueNode? last = null;

    public void Enqueue(string dirPath){
        DirQueueNode newNode = new DirQueueNode(dirPath);
        if (last != null) last.next = newNode;
        last = newNode;

        if (first == null) first = newNode;
    }

    public string? Dequeue(){
        if (first != null){
            string result = first.data;
            first = first.next;
            return result;
        }
        return null;
    }
}

public class DirQueueNode{
    public DirQueueNode? next = null;
    public string data;

    public DirQueueNode(string data){
        this.data = data;
    }
}

public class MainClass{
    public static void Main(string[] args){

        string definitionsPath = args[0];
        string? dirPath = args[1];

        string paramText = File.ReadAllText(definitionsPath);
        Dictionary<string, string> paramDict = ParamLoader.GetParamDict(
            paramText
        );

        DirQueue dq = new DirQueue();

        Console.WriteLine($"starting replacements in {dirPath}");
        int totalFiles = 0;
        int replacedFiles = 0;

        while (dirPath != null){
            string[] filePaths = Directory.GetFiles(dirPath);
            foreach(string path in filePaths){
                totalFiles++;

                if (!path.EndsWith(".py")) continue;

                string sourceCode = File.ReadAllText(path);
                sourceCode = ParamReplacer.ReplaceParams(
                    sourceCode,
                    paramDict
                );
                File.WriteAllText(path, sourceCode);
                replacedFiles++;
                Console.WriteLine($"replaced params in {path}");
            }

            string[] subdirPaths = Directory.GetDirectories(dirPath);
            foreach(string subdirPath in subdirPaths){
                dq.Enqueue(subdirPath);
            }

            dirPath = dq.Dequeue();
        }

        Console.WriteLine($"files visited: {totalFiles}");
        Console.WriteLine($"files .py files visited: {replacedFiles}");
    }
}
