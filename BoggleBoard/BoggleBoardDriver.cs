
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sterms
{
    public class BoggleBoardDriver
    {
        static void Main(string[] args)
        {
            //This is the BoggleBoardDriver. 
            //The list of words can be in a few various forms, string[], char[], list<string>, list<char>.
            //Parameters are the size of board, the letters on the board, and there is an optional parameter for file path.
            //It is set to use "dictionary.txt" in the run-time directory by default.
            //Mis-matched boards and input sources throw Argument Exceptions.
            //The first call to the BoggleBoard class is expensive, as it includes the dictionary load. All calls after do not need to intialize it.

            List<string> words = BoggleBoard.GetAllBoggleWords(4, new char[] { 'p', 's', 'u', 'n', 'z', 'a', 's', 'q', 'f', 't', 'r', 't', 'i', 'j', 'm', 'a' });

            List<string> words2 = BoggleBoard.GetAllBoggleWords(4, "acdefrteiostnesn");

            List<char> boggleChars = new List<char>();

            boggleChars.Add('d'); boggleChars.Add('j'); boggleChars.Add('n'); boggleChars.Add('p');
            boggleChars.Add('r'); boggleChars.Add('s'); boggleChars.Add('t'); boggleChars.Add('z');
            boggleChars.Add('e'); boggleChars.Add('t'); boggleChars.Add('r'); boggleChars.Add('n');
            boggleChars.Add('i'); boggleChars.Add('u'); boggleChars.Add('s'); boggleChars.Add('r');

            List<string> words3 = BoggleBoard.GetAllBoggleWords(4, boggleChars);


            ////Uncomment to use live game.
            //while (true)
            //{
            //    Console.Out.WriteLine("Enter Boggle String:");
            //    string input = Console.ReadLine();
            //    Console.Clear();
            //    if (input == "quit") break;
            //    else
            //    {
            //        List<string> words = BoggleBoard.GetAllBoggleWords(4, input);

            //        foreach(string word in words.OrderByDescending(s => s.Length))
            //        { 
            //            Console.Out.WriteLine(word);
            //        }
            //    }
            //}

        }
    }
}
