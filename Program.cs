namespace app
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(OldPhonePad("33#")); // => output: E
            Console.WriteLine(OldPhonePad("227*#")); // => output: B
            Console.WriteLine(OldPhonePad("4433555 555666#")); // => output: HELLO 
            Console.WriteLine(OldPhonePad("8 88777444666*664#")); // => output: TURING
        }

        static Dictionary<string, string[]> Mapper = new Dictionary<string, string[]>
    {
        //I personally don't like naming keys with number first
        //This makes it more readable but having a little bit of a trade off
        //By adding PAD before accessing the map
        { "PAD_1", new string[] { "&", ",", "(" } },
        { "PAD_2", new string[] { "A", "B", "C" } },
        { "PAD_3", new string[] { "D", "E", "F" } },
        { "PAD_4", new string[] { "G", "H", "I" } },
        { "PAD_5", new string[] { "J", "K", "L" } },
        { "PAD_6", new string[] { "M", "N", "O" } },
        { "PAD_7", new string[] { "P", "Q", "R", "S" } },
        { "PAD_8", new string[] { "T", "U", "V" } },
        { "PAD_9", new string[] { "W", "X", "Y", "Z" } }
    };

        public static string OldPhonePad(string input)
        {
            KeyValuePair<char, int>[] tokenizedInput = TokenizePad(input);
            return Translate(tokenizedInput);
        }
        public static KeyValuePair<char, int>[] TokenizePad(string input)
        {
            char[] inputArray = input.ToCharArray();
            List<KeyValuePair<char, int>> resultArray = new List<KeyValuePair<char, int>>();
            char tempValue = inputArray[0];
            int tempCount = 1;
            for (int i = 1; i < inputArray.Length; i++)
            {
                if (inputArray[i] != tempValue)
                {
                    resultArray.Add(new KeyValuePair<char, int>(tempValue, tempCount));
                    tempValue = inputArray[i];
                    tempCount = 1;
                }
                else
                {
                    tempCount++;
                }
            }
            resultArray.Add(new KeyValuePair<char, int>(tempValue, tempCount));
            return resultArray.ToArray();
        }
        public static string Translate(KeyValuePair<char, int>[] padPairs)
        {
            //For small optimization we could make a string builder instead
            //But in this small application it is easier to read this way
            string resultString = "";
            foreach (KeyValuePair<char, int> padPair in padPairs)
            {
                if (padPair.Key == '#') break; //Early exit making sure that if it reaches # it will stop
                if (padPair.Key == '*')        //Could make it peek next element before add instead of add then remove but it is easier to read this way
                {                              //If performance sensitive use the peek instead
                    resultString = resultString.Substring(0, resultString.Length - 1);
                }
                resultString += GetPadValue(padPair.Key, padPair.Value);
            }
            return resultString;
        }

        //There is a case where user presing pads multiple times to loop back to the values
        //Let say 2 contains chars ["A", "B", "C"]
        //While pressing 2 4 consecutive time like 2222 it will loop back and display "A"
        //pressing 5 times will be "B"
        //It would be too dirty if we access the pad value through other functions
        //This way is more verbose/readable
        public static string GetPadValue(char padNumber, int occurance)
        {
            if (!Mapper.ContainsKey("PAD_" + padNumber)) return "";
            int padLength = Mapper["PAD_" + padNumber].Length;
            return Mapper["PAD_" + padNumber][(occurance - 1) % padLength];
        }
    }
}