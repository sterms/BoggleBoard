using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sterms
{
    /// <summary>
    /// Boggle Board - Uses a trie to find all the possible words in a boggle board.
    /// </summary>
    public class BoggleBoard
    {
        private char[,] m_Board;
        private static Trie s_Dictionary; //Making the dictionary static so we aren't loading it every time.

        #region Static Methods

        public static List<string> GetAllBoggleWords(int boardSize, string letters, string dictionaryFilePath = "dictionary.txt")
        {
            BoggleBoard current = new BoggleBoard(boardSize, letters, dictionaryFilePath);
            return current.FindWords();
        }

        public static List<string> GetAllBoggleWords(int boardSize, string[] letters, string dictionaryFilePath = "dictionary.txt")
        {
            BoggleBoard current = new BoggleBoard(boardSize, letters, dictionaryFilePath);
            return current.FindWords();
        }

        public static List<string> GetAllBoggleWords(int boardSize, char[] letters, string dictionaryFilePath = "dictionary.txt")
        {
            BoggleBoard current = new BoggleBoard(boardSize, letters, dictionaryFilePath);
            return current.FindWords();
        }

        public static List<string> GetAllBoggleWords(int boardSize, List<string> letters, string dictionaryFilePath = "dictionary.txt")
        {
            BoggleBoard current = new BoggleBoard(boardSize, letters, dictionaryFilePath);
            return current.FindWords();
        }

        public static List<string> GetAllBoggleWords(int boardSize, List<char> letters, string dictionaryFilePath = "dictionary.txt")
        {
            BoggleBoard current = new BoggleBoard(boardSize, letters, dictionaryFilePath);
            return current.FindWords();
        }

        #endregion

        #region Private Constructors

        private BoggleBoard(int boardSize, string letters, string dictionaryFilePath)
        {
            Initialize(boardSize, letters.Length, dictionaryFilePath);

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {                   
                    m_Board[i, j] = letters[(i * boardSize) + j];
                }
            }
        }

        private BoggleBoard(int boardSize, string[] letters, string dictionaryFilePath)
        {
            Initialize(boardSize, letters.Length, dictionaryFilePath);

            for (int i = 0; i < boardSize; i++)
            {
                for(int j = 0; j < boardSize; j++)
                {
                    if(letters[i * j].Length != 1) throw new ArgumentException("Incorrect sized element at index " + (i * j));
                    m_Board[i, j] = letters[(i * boardSize) + j][0];
                }
            }
        }

        private BoggleBoard(int boardSize, char[] letters, string dictionaryFilePath)
        {
            Initialize(boardSize, letters.Length, dictionaryFilePath);

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    m_Board[i, j] = letters[(i * boardSize) + j];
                }
            }
        }

        private BoggleBoard(int boardSize, List<char> letters, string dictionaryFilePath)
        {
            Initialize(boardSize, letters.Count, dictionaryFilePath);

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    m_Board[i, j] = letters[(i * boardSize) + j];
                }
            }
        }

        private BoggleBoard(int boardSize, List<string> letters, string dictionaryFilePath)
        {
            Initialize(boardSize, letters.Count, dictionaryFilePath);

            for (int i = 0; i < boardSize; i++)
            {
                for (int j = 0; j < boardSize; j++)
                {
                    if (letters[i * j].Length != 1) throw new ArgumentException("Incorrect sized element at index " + (i * j));
                    m_Board[i, j] = letters[(i * boardSize) + j][0];
                }
            }
        }

        private void Initialize(int boardSize, int letterCount, string dictionaryFilePath)
        {
            m_Board = new char[boardSize, boardSize];
            if (letterCount != boardSize * boardSize) throw new ArgumentException("Incorrect sized board source.");
            if (s_Dictionary == null || s_Dictionary.Size == 0) s_Dictionary = new Trie(dictionaryFilePath, 4);
        }

        #endregion

        #region Problem Solving Functions

        private List<string> FindWords()
        {
            Queue<Stack<string>> foundWords = new Queue<Stack<string>>();

            for(int row = 0; row <= m_Board.GetUpperBound(0); row++)
            {
                for(int col = 0; col <= m_Board.GetUpperBound(1); col++)
                {
                    foundWords.Enqueue(
                        FindWords(row, col, 
                        new ushort[(m_Board.GetUpperBound(0) + 1) * (m_Board.GetUpperBound(1) + 1)], 
                        string.Empty, 
                        new Stack<string>())
                        );
                }
            }

            //Using a set because boggle doesn't allow duplicates, even made with different spots.
            HashSet<string> wordSet = new HashSet<string>();
            while(foundWords.Count > 0)
            {
                Stack<string> currentStack = foundWords.Dequeue();
                while(currentStack.Count > 0)
                {
                    wordSet.Add(currentStack.Pop());
                }
            }

            return wordSet.ToList();
        }

        private Stack<string> FindWords(int row, int col, ushort[] currentPath, string currentWord, Stack<string> foundWords)
        {
            //Set Path, Add Current Char to String.
            currentPath[row * (m_Board.GetUpperBound(0) + 1) + col] = 1;
            currentWord += m_Board[row, col];

            if (s_Dictionary.ValidateWord(currentWord))
            {
                foundWords.Push(s_Dictionary.FindWord(currentWord));
            }

            ushort[] subPath = new ushort[currentPath.Length];
            //North
            if (CheckBounds(row - 1, col) && currentPath[(row - 1) * (m_Board.GetUpperBound(0) + 1) + (col)] == 0)
            {
                if (s_Dictionary.ValidatePrefix(currentWord + m_Board[row - 1, col]))
                {
                    Array.Copy(currentPath, subPath, currentPath.Length);
                    foundWords = FindWords(row - 1, col, subPath, currentWord, foundWords);
                }
            }
            //West
            if (CheckBounds(row, col - 1) && currentPath[(row) * (m_Board.GetUpperBound(0) + 1) + (col -1 )] == 0)
            {
                if (s_Dictionary.ValidatePrefix(currentWord + m_Board[row, col - 1]))
                {
                    Array.Copy(currentPath, subPath, currentPath.Length);
                    foundWords = FindWords(row, col - 1, subPath, currentWord, foundWords);
                }
            }
            //East
            if (CheckBounds(row, col + 1) && currentPath[(row) * (m_Board.GetUpperBound(0) + 1) + (col + 1)] == 0)
            {
                if (s_Dictionary.ValidatePrefix(currentWord + m_Board[row, col + 1]))
                {
                    Array.Copy(currentPath, subPath, currentPath.Length);
                    foundWords = FindWords(row, col + 1, subPath, currentWord, foundWords);
                }
            }
            //South
            if (CheckBounds(row + 1, col) && currentPath[(row + 1) * (m_Board.GetUpperBound(0) + 1) + (col)] == 0)
            {
                if (s_Dictionary.ValidatePrefix(currentWord + m_Board[row + 1, col]))
                {
                    Array.Copy(currentPath, subPath, currentPath.Length);
                    foundWords = FindWords(row + 1, col, subPath, currentWord, foundWords);
                }
            }
            //North-West
            if (CheckBounds(row - 1, col - 1) && currentPath[(row - 1) * (m_Board.GetUpperBound(0) + 1) + (col - 1)] == 0)
            {
                if (s_Dictionary.ValidatePrefix(currentWord + m_Board[row - 1, col - 1]))
                {
                    Array.Copy(currentPath, subPath, currentPath.Length);
                    foundWords = FindWords(row - 1, col - 1, subPath, currentWord, foundWords);
                }
            }
            //North-East
            if (CheckBounds(row - 1, col + 1) && currentPath[(row - 1) * (m_Board.GetUpperBound(0) + 1) + (col + 1)] == 0)
            {
                if (s_Dictionary.ValidatePrefix(currentWord + m_Board[row - 1, col + 1]))
                {
                    Array.Copy(currentPath, subPath, currentPath.Length);
                    foundWords = FindWords(row - 1, col + 1, subPath, currentWord, foundWords);
                }
            }
            //South-East
            if (CheckBounds(row + 1, col + 1) && currentPath[(row + 1) * (m_Board.GetUpperBound(0) + 1) + (col + 1)] == 0)
            {
                if (s_Dictionary.ValidatePrefix(currentWord + m_Board[row + 1, col + 1]))
                {
                    Array.Copy(currentPath, subPath, currentPath.Length);
                    foundWords = FindWords(row + 1, col + 1, subPath, currentWord, foundWords);
                }
            }
            //South-West
            if (CheckBounds(row + 1, col - 1) && currentPath[(row + 1) * (m_Board.GetUpperBound(0) + 1) + (col - 1)] == 0)
            {
                if (s_Dictionary.ValidatePrefix(currentWord + m_Board[row + 1, col - 1]))
                {
                    Array.Copy(currentPath, subPath, currentPath.Length);
                    foundWords = FindWords(row + 1, col - 1, subPath, currentWord, foundWords);
                }
            }
            //Return
            return foundWords;
        }

        //Returns false if out of bounds.
        private bool CheckBounds(int row, int col)
        {
            if (row < 0 || row > m_Board.GetUpperBound(0) || col < 0 || col > m_Board.GetUpperBound(1)) return false;
            return true;
        }

        #endregion


    }
}
