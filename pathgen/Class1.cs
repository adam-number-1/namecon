using System;
using System.Collections;
using System.IO;

namespace pathgen{

    internal class PathQueue{
        private PathQueueNode? first = null;
        private PathQueueNode? last = null;

        public void Enqueue(string dirPath){
            PathQueueNode newNode = new PathQueueNode(dirPath);
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

    internal class PathQueueNode{
        public PathQueueNode? next = null;
        public string data;

        public PathQueueNode(string data){
            this.data = data;
        }
    }

    internal class DirPathGen
    {
        private PathQueue dirQueue;
        public DirPathGen(string dirPath){
            this.dirQueue = new PathQueue();
            this.dirQueue.Enqueue(dirPath);
        }

        public IEnumerator<string> GetEnumerator(){
            string? dirPath = dirQueue.Dequeue();
            while (dirPath != null){

                yield return dirPath;

                string[] subdirPaths = Directory.GetDirectories(dirPath);
                foreach(string subdirPath in subdirPaths){
                    dirQueue.Enqueue(subdirPath);
                }

                dirPath = dirQueue.Dequeue();
            }
        }
    }

    public class FilePathGen{
        private DirPathGen dirGen;

        public FilePathGen(string dirPath){
            this.dirGen = new DirPathGen(dirPath);
        }

        public IEnumerator<string> GetEnumerator(){
            foreach (string dirPath in dirGen){
                string[] filePaths = Directory.GetFiles(dirPath);
                foreach (string filePath in filePaths){
                    yield return filePath;
                }
            }
        }

    }

}