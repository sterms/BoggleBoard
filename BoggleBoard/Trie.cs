using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sterms
{
    /// <summary>
    /// Trie class, written by Steve Stermer. steve@stermer.org
    /// </summary>
    public class Trie
    {
        private TrieNode m_Root;
        public int Size { get; private set; } = 0;

        public Trie(string filePath, int minimumWordLength = 3)
        {
            m_Root = new TrieNode("");

            if (string.IsNullOrEmpty(filePath)) return;
            if (!File.Exists(filePath)) throw new FileNotFoundException("Trie: Dictionary not found at: " + filePath);
            LoadTrie(filePath, minimumWordLength);
        }

        public Trie()
        {
            m_Root = new TrieNode("");
        }

        private void LoadTrie(string filePath, int minimumWordLength)
        {
            string DictionaryIO = File.ReadAllText(filePath);
            string[] DictionaryWords = DictionaryIO.Split('\r', ' ', '\n');
            for(int i = 0; i < DictionaryWords.Length; i++)
            {
                if (DictionaryWords[i].Length >= minimumWordLength) Insert(DictionaryWords[i]);
            }
        }

        public void Insert(string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            value = value.ToLowerInvariant();

            Size++;

            Insert(value, 0, m_Root);          
        }

        private void Insert(string value, int index, TrieNode node)
        {
            //If the node doesn't have that trie branch, add it.
            if (!node.m_ChildNodes.ContainsKey(value[index]))
            {
                node.m_ChildNodes.Add(value[index], new TrieNode(node.m_Value + value[index]));
            }

            //If we still have more letters, keep recursing.
            if (index < value.Length - 1) Insert(value, index + 1, node.m_ChildNodes[value[index]]);

            //This was the last, mark it as valid.
            if (index == value.Length - 1) node.m_ChildNodes[value[index]].m_ValidEnd = true; 
        }

        public bool ValidateWord(string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            value = value.ToLowerInvariant();

            return ValidateWord(value, 0, m_Root);
        }

        private bool ValidateWord(string value, int index, TrieNode node)
        {
            //If we don't have any more nodes, its not in the trie... false
            if (!node.m_ChildNodes.ContainsKey(value[index]))
            {
                return false;
            }

            //If we still have room to recurse, do it.
            if (index < value.Length - 1) return ValidateWord(value, index + 1, node.m_ChildNodes[value[index]]);

            //We must be on the last index, if it's child node had a valid end, return true.
            if (node.m_ChildNodes[value[index]].m_ValidEnd) return true;

            return false;
        }

        //Method to figure out if we can kill a path.
        public bool ValidatePrefix(string value)
        {
            if (string.IsNullOrEmpty(value)) return false;
            value = value.ToLowerInvariant();

            return ValidatePrefix(value, 0, m_Root);
        }

        private bool ValidatePrefix(string value, int index, TrieNode node)
        {
            //If we don't have any more nodes, its not in the trie... false
            if (!node.m_ChildNodes.ContainsKey(value[index]))
            {
                return false;
            }

            //If we still have room to recurse, do it.
            if (index < value.Length - 1) return ValidatePrefix(value, index + 1, node.m_ChildNodes[value[index]]);

            //We must be on the last index, and the node had the value in its child nodes, so its valid.
            return true;
        }

        /// <summary>
        /// Finds a single word. Returns empty if string wasn't valid.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string FindWord(string value)
        {
            if (string.IsNullOrEmpty(value)) return string.Empty;
            value = value.ToLowerInvariant();

            return FindWord(value, 0, m_Root);
        }

        private string FindWord(string value, int index, TrieNode node)
        {
            //If we don't have any more nodes, its not in the trie.
            if (!node.m_ChildNodes.ContainsKey(value[index]))
            {
                return string.Empty;
            }

            //If we still have room to recurse, do it.
            if (index < value.Length - 1) return FindWord(value, index + 1, node.m_ChildNodes[value[index]]);

            //If we were on the last node, if the end is valid, add it.
            if (index == value.Length - 1 && node.m_ChildNodes[value[index]].m_ValidEnd == true) return node.m_ChildNodes[value[index]].m_Value;

            return string.Empty;
        }
        

        /// <summary>
        /// Function finds all words in a string. Drops every index of the string into the root to find all sub strings.
        /// Using 'Check Neighbor' approach, so building all permutations of grid strings, and this function, is depreciated.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public List<string> FindAllWords(string value)
        {
            if (string.IsNullOrEmpty(value)) return new List<string>();
            value = value.ToLowerInvariant();

            Queue<Stack<string>> wordQueue = new Queue<Stack<string>>();
            for(int i = 0; i < value.Length; i++)
            {
                wordQueue.Enqueue(FindAllWords(value, i, m_Root, new Stack<string>()));
            }

            //Unwrap the Queue(Of Stacks<string>) for return. Hash set for no duplicates.
            HashSet<string> returnList = new HashSet<string>();
            while(wordQueue.Count > 0)
            {
                Stack<string> currentStack = wordQueue.Dequeue();
                while(currentStack.Count > 0)
                {
                    returnList.Add(currentStack.Pop());
                }
            }
            return returnList.ToList();
        }  
        
        private Stack<string> FindAllWords(string value, int index, TrieNode node, Stack<string> currentStack)
        {
            //If we uncover a word while working toward another, add it.
            if (node.m_ValidEnd == true)
            {
                currentStack.Push(node.m_Value);
            }

            //If we don't have any more nodes, its not in the trie.
            if (!node.m_ChildNodes.ContainsKey(value[index]))
            {
                return currentStack;
            }

            //If we still have room to recurse, do it.
            if (index < value.Length - 1) return FindAllWords(value, index + 1, node.m_ChildNodes[value[index]], currentStack);

            //If we were on the last node, if the end is valid, add it.
            if (index == value.Length - 1 && node.m_ChildNodes[value[index]].m_ValidEnd == true) currentStack.Push(node.m_ChildNodes[value[index]].m_Value);

            return currentStack;
        }      

        private class TrieNode
        {
            public string m_Value; //holds the current string value at this depth.
            public Dictionary<char, TrieNode> m_ChildNodes; //using a Hash Table instead of a char[26] to support UTF-16
            public bool m_ValidEnd = false;

            public TrieNode(string value)
            {
                m_Value = value;
                m_ChildNodes = new Dictionary<char, TrieNode>();
            }

        }
    }
}
