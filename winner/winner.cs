using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace winner {
    class winner {

        static string inputFile = string.Empty;
        static string outputFile = string.Empty;
        static readonly string Error = "Error";
        static readonly string Input = "abc.txt";
        static readonly string Output = "xyz.txt";
        static readonly char Hash = '#';

        private enum CardsEnum {
            One = 1, Two = 2, Three = 3, Four = 4, Five = 5,
            Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10,
            Eleven = 11, Twelve = 12, Thirteen = 13
        };

        static void Main(string[] args) {

            Dictionary<string, int> dict = new Dictionary<string, int> { };
            string path = System.IO.Directory.GetCurrentDirectory();
            inputFile = Path.Combine(path, Input);

            int cardNum = 0;
            int suitNum = 0;
            int suitFinal = 0;
            bool errorBreak = false;

            using (var sr = new StreamReader(inputFile)) {

                string rFileCheck = sr.ReadToEnd();

                string[] readCheck = rFileCheck.Split(new[] { "\r\n" }, StringSplitOptions.None);

                foreach (var itemCheck in readCheck) {
                    string[] splittedCheck = itemCheck.Split(':', ',');
                    foreach (var items in splittedCheck.Skip(1)) {
                        if (items.Length > 3 || splittedCheck[0].Length == 0) {
                            WriteInFile(Error, 0);
                            errorBreak = true;
                            break;
                        }
                        //break;
                    }
                }
            }
            if (!errorBreak) {

                using (var validSR = new StreamReader(inputFile)) {
                    List<string> scoreResults = new List<string>();
                    List<string> nameResults = new List<string>();
                    string read;

                    while ((read = validSR.ReadLine()) != null) {

                        if (!String.IsNullOrWhiteSpace(read)) {
                            int charIndex = read.IndexOf(':', StringComparison.Ordinal);
                            FindCards(charIndex, read);
                        }
                    }
                }
            }


            void FindCards(int charIndex, string read) {
                try {
                    string playerName;
                    if (charIndex > 0) {
                        playerName = read.Substring(0, charIndex);
                        int intFirstCard = charIndex - 3;
                        string card1 = read.Substring(charIndex + (int)CardsEnum.One, (int)CardsEnum.Two);
                        List<char> card1List = new List<char>();
                        card1List.AddRange(card1);
                        string card2 = read.Substring(charIndex + (int)CardsEnum.Four, (int)CardsEnum.Two);
                        List<char> card2List = new List<char>();
                        card2List.AddRange(card2);
                        string card3 = read.Substring(charIndex + (int)CardsEnum.Seven, (int)CardsEnum.Two);
                        List<char> card3List = new List<char>();
                        card3List.AddRange(card3);
                        string card4 = read.Substring(charIndex + (int)CardsEnum.Ten, (int)CardsEnum.Two);
                        List<char> card4List = new List<char>();
                        card4List.AddRange(card4);
                        string card5 = read.Substring(charIndex + (int)CardsEnum.Thirteen, (int)CardsEnum.Two);
                        List<char> card5List = new List<char>();
                        card5List.AddRange(card5);
                        int final = calculate(card1List[0].ToString())
                            + calculate(card2List[0].ToString())
                            + calculate(card3List[0].ToString())
                            + calculate(card4List[0].ToString())
                            + calculate(card5List[0].ToString());
                        suitFinal = SuitCalc(card1List[1].ToString())
                            + SuitCalc(card2List[1].ToString())
                            + SuitCalc(card3List[1].ToString())
                            + SuitCalc(card4List[1].ToString())
                            + SuitCalc(card5List[1].ToString());
                        dict.Add(playerName + Hash + suitFinal, final);
                    }
                    else {
                        WriteInFile(Error, 0);
                        Console.WriteLine(Error);
                    }

                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    WriteInFile(Error, 0);
                }
            }

            GetWinnerString(dict);

            void GetWinnerString(Dictionary<string, int> dict) {
                try {
                    StringBuilder getWinner = new StringBuilder();
                    int x = 0;
                    var dictSuit = new Dictionary<string, int> { };

                    foreach (KeyValuePair<string, int> keys in dict) {
                        if (dict.Values.Max() == keys.Value) {
                            dictSuit.Add(keys.Key.Split(Hash)[0], Int32.Parse(keys.Key.Split(Hash)[1]));
                        }
                    }
                    foreach (KeyValuePair<string, int> keys in dictSuit) {
                        if (dictSuit.Values.Max() == keys.Value) {
                            x++;
                            if (x == 1) {
                                getWinner.Append(keys.Key.Split(Hash)[0]);
                            }
                            else {
                                getWinner.Append("," + keys.Key.Split(Hash)[0]);
                            }

                            int winnersInt = getWinner.ToString().LastIndexOf(getWinner.ToString());
                            string winnerString = getWinner.ToString().Substring(0, getWinner.ToString().Length - winnersInt);
                            WriteInFile(winnerString, dict.Values.Max());
                        }
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                    WriteInFile(Error, 0);
                }
            }


            void WriteInFile(string playerName, int score) {
                try {
                    outputFile = Path.Combine(path, Output);
                    File.Delete(outputFile);
                    StreamWriter sWriter = new StreamWriter(outputFile, true, Encoding.ASCII);
                    if (Error.Equals(playerName)) {
                        sWriter.Write(playerName);
                        Console.WriteLine(playerName);
                    }
                    else {
                        string displayWinner = playerName + " : " + score;
                        sWriter.Write(displayWinner);
                        Console.WriteLine(displayWinner);
                    }

                    sWriter.Close();
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }

            const string CardA = "A";
            const string CardJ = "J";
            const string CardQ = "Q";
            const string CardK = "K";
            const string SuitH = "H";
            const string SuitS = "S";
            const string SuitD = "D";
            const string SuitC = "C";

            int calculate(string num) {
                if (!(num.ToString().Equals(CardA))
                    && !(num.ToString().Equals(CardJ))
                    && !(num.ToString().Equals(CardK))
                    && !(num.ToString().Equals(CardQ))) {
                    cardNum = int.Parse(num.ToString());
                }
                else {
                    switch (num) {
                        case CardA:
                            cardNum = (int)CardsEnum.One;
                            break;
                        case CardJ:
                            cardNum = (int)CardsEnum.Eleven;
                            break;
                        case CardQ:
                            cardNum = (int)CardsEnum.Twelve;
                            break;
                        case CardK:
                            cardNum = (int)CardsEnum.Thirteen;
                            break;
                    }
                }
                return cardNum;
            }

            int SuitCalc(string SuitGroup) {
                if (!(SuitGroup.ToString().Equals(SuitD))
                    && !(SuitGroup.ToString().Equals(SuitC))
                    && !(SuitGroup.ToString().Equals(SuitS))
                    && !(SuitGroup.ToString().Equals(SuitH))) {
                    suitNum = 0;
                }
                else {
                    switch (SuitGroup) {
                        case SuitH:
                            suitNum = (int)CardsEnum.Three;
                            break;
                        case SuitS:
                            suitNum = (int)CardsEnum.Four;
                            break;
                        case SuitD:
                            suitNum = (int)CardsEnum.Two;
                            break;
                        case SuitC:
                            suitNum = (int)CardsEnum.One;
                            break;
                    }
                }
                return suitNum;
            }
            Console.ReadLine();
        }
    }
}

