// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Namecon{
    public class Namecon{
        public static void Main(string []args){
            Stack<string> dirPaths= new Stack<string>();
            dirPaths.Push(args[0]);
            string namesPath = args[1];

            Dictionary<string , string> namesDict = GetNamesDict(namesPath);

            while (dirPaths.Count > 0){
                string currDir = dirPaths.Pop();
                string[] filePaths = Directory.GetFiles(currDir);
                ProcessFiles(filePaths);
                // replace all the :params a: with params a: bbbb
                // compose the new text
                // write out
                // subverse into directories
            }
        }

        public static Dictionary<string , string> GetNamesDict(string path){
            Dictionary<string ,string> result = new Dictionary<string ,string>();
            IEnumerable<string> lines = File.ReadLines(path, Encoding.UTF8);

            int linesVisited = 0;
            foreach(string line in lines){
                if (!line.StartsWith(":param ")){
                    throw new ArgumentException(
                        $"Wrong value at line {linesVisited}: {line}"
                    );
                }
                string[] chunks = line.Split(": ", StringSplitOptions.None);
                result.Add(chunks[0], chunks[1]);
                linesVisited++;
            }

            return result;
        }

        public static void ProcessFiles(
            string[] filePaths, 
            Dictionary<string,string> namesDict
        ){
            foreach(string path in filePaths){
                string newText = "";
                IEnumerable<string> lines = File.ReadLines(path, Encoding.UTF8);
                foreach(string line in lines){
                    try{
                        string replacement = namesDict[line];
                        newText += replacement;
                    }
                    catch (KeyNotFoundException){
                        newText += line;
                    }
     
                }
                File.WriteAllText(path, newText);
            }
        }

    }
}