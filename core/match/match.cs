using System;
using System.Linq;

namespace match
{
    class Match
    {
        List<Dictionary<string, List<int>>> myList = new List<Dictionary<string, List<int>>> {
            new Dictionary<string, List<int>> { { "apple", [12, 5, 8] },
            { "banana", [3, 17]  },
            { "cherry", [20] },
            { "date",  [9, 14, 1, 19] },
            { "elderberry", [4, 19, 16] },
        }};

        List<Dictionary<string, List<int>>> myList2 = new List<Dictionary<string, List<int>>> {
            new Dictionary<string, List<int>> {
            { "fig", [23]  },
            { "grape", [0, 25, 18] },
            { "honeydew", [23, 7, 10, 12, 199, 234] },
        }};

        List<string> myResult = [];
        List<Dictionary<string, List<int>>> aggregate;
        public static void Main(string[] args)
        {
            List<int> a = new List<int>{ 1, 2, 3 };
            List<int> b = new List<int> { 4, 5, 6 };

            a.AddRange(b);
            foreach (int num in a)
            {
                Console.WriteLine(num);
            }
        }

        public List<string> Rank(string word, List<Dictionary<string, List<int>>> myList)
        {
            aggregate.Clear();
            return myResult;
        }

        public void AddResult(List<Dictionary<string, List<int>>> myList)
        {
            aggregate.AddRange(myList);
        }
    }
}