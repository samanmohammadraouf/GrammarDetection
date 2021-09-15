using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Grammar
{
    class alphabet
    {
        public string character;
    }
    class variable : alphabet
    {
        public List<string> equals = new List<string>();
    }
    class Terminal : alphabet
    {

    }

    class Production
    {
        public variable left;
        public List<alphabet> right = new List<alphabet>();
        public Production(variable left, List<alphabet> right)
        {
            this.left = left;
            this.right = right;
        }
        public bool Check(char c)
        {
            for (int i = 0; i < right.Count; i++)
            {
                if (c.ToString() == right[i].character)
                {
                    return true;
                }
            }
            return false;
        }

        public bool Check(Production X, Production Y)
        {
            if (right[0] == X.left && right[1] == Y.left)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    class Program
    {
        public static List<variable> variables = new List<variable>();
        public static List<Terminal> terminals = new List<Terminal>();
        public static List<Production> productions = new List<Production>();
        static void Main(string[] args)
        {
            int lines = int.Parse(Console.ReadLine());
            List<string> Lines = new List<string>();
            for (int i = 0; i < lines; i++)
            {
                string line = Console.ReadLine();
                line = line.Replace(" ", "");
                line = line.Replace(">", "");
                //Console.WriteLine(line);
                //string [] parts = line.Split('-');
                string part1 = line.Substring(0, 2);
                string part2 = line.Substring(3);
                part1 = part1.Replace("<", "");
                variable newVar = new variable();
                newVar.character = part1;
                //right hand of production
                string[] partitions = part2.Split('|');
                for (int j = 0; j < partitions.Length; j++)
                {
                    bool IsVariable = false;
                    for (int k = 0; k < partitions[j].Length; k++)
                    {
                        if (partitions[j][k] == '<')
                        {
                            IsVariable = true;
                            continue;
                        }
                        else if (IsVariable == true)
                        {
                            IsVariable = false;
                            continue;
                        }
                        else
                        {
                            bool IsAlreadyExists = false;
                            if (terminals.Count != 0)
                            {
                                for (int m = 0; m < terminals.Count; m++)
                                {
                                    if (partitions[j][k].ToString() == terminals[m].character)
                                    {
                                        IsAlreadyExists = true;
                                    }
                                }
                            }
                            if (IsAlreadyExists == false)
                            {
                                Terminal newTerminal = new Terminal();
                                newTerminal.character = partitions[j][k].ToString();
                                terminals.Add(newTerminal);
                            }
                        }
                    }
                    partitions[j] = partitions[j].Replace("<", "");
                    newVar.equals.Add(partitions[j]);
                }
                variables.Add(newVar);
            }

            string str = Console.ReadLine();

            CreateProductions();
            
            RemoveNullables();
            //for (int i = 0; i < productions.Count; i++)
            //{
            //    string x = "";
            //    for (int j = 0; j < productions[i].right.Count; j++)
            //    {
            //        x = x + productions[i].right[j].character;
            //    }
            //    Console.WriteLine(productions[i].left.character + " " + x);
            //}
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();
            RemoveUseless();
            //for (int i = 0; i < productions.Count; i++)
            //{
            //    string x = "";
            //    for (int j = 0; j < productions[i].right.Count; j++)
            //    {
            //        x = x + productions[i].right[j].character;
            //    }
            //    Console.WriteLine(productions[i].left.character + " " + x);
            //}
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();
            RemoveUnits();
            //for (int i = 0; i < productions.Count; i++)
            //{
            //    string x = "";
            //    for (int j = 0; j < productions[i].right.Count; j++)
            //    {
            //        x = x + productions[i].right[j].character;
            //    }
            //    Console.WriteLine(productions[i].left.character + " " + x);
            //}
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();
            RemoveRepeatedProductions();
            //for (int i = 0; i < productions.Count; i++)
            //{
            //    string x = "";
            //    for (int j = 0; j < productions[i].right.Count; j++)
            //    {
            //        x = x + productions[i].right[j].character;
            //    }
            //    Console.WriteLine(productions[i].left.character + " " + x);
            //}
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();
            chomskeyConverter();
            for(int i=0; i<productions.Count;i++)
            {
                if(productions[i].left==productions[i].right[0] && productions[i].right.Count==1)
                {
                    productions.RemoveAt(i);
                    i--;
                }
            }
            //for (int i = 0; i < productions.Count; i++)
            //{
            //    string x = "";
            //    for (int j = 0; j < productions[i].right.Count; j++)
            //    {
            //        x = x + productions[i].right[j].character;
            //    }
            //    Console.WriteLine(productions[i].left.character + " " + x);
            //}
            if (AcceptReject(str))
            {
                Console.WriteLine("Accepted");
            }
            else
            {
                Console.WriteLine("Rejected");
            }


            
            Console.ReadKey();
        }

        public static void RemoveUseless()
        {
            for(int i=0;i<productions.Count;i++)
            {
                if(productions[i].right.Count==1 && productions[i].left == productions[i].right[0])
                {
                    productions.RemoveAt(i);
                    i--;
                }
            }
        }
        public static void CreateProductions()
        {
            for (int i = 0; i < variables.Count; i++)
            {
                for (int j = 0; j < variables[i].equals.Count; j++)
                {
                    List<alphabet> characters = new List<alphabet>();
                    for (int k = 0; k < variables[i].equals[j].Length; k++)
                    {
                        for (int m = 0; m < variables.Count; m++)
                        {
                            if (variables[m].character == variables[i].equals[j][k].ToString())
                            {
                                characters.Add(variables[m]);
                            }
                        }

                        for (int m = 0; m < terminals.Count; m++)
                        {
                            if (terminals[m].character == variables[i].equals[j][k].ToString())
                            {
                                characters.Add(terminals[m]);
                            }
                        }
                    }
                    Production newProduction = new Production(variables[i], characters);
                    productions.Add(newProduction);
                }
            }
        }

        
        public static void RemoveNullables()
        {
            for (int i = 0; i < productions.Count; i++)
            {
                for (int j = 0; j < productions[i].right.Count; j++)
                {
                    if (productions[i].right.Count == 1 && productions[i].right[j].character == "#")
                    {
                        string variable = productions[i].left.character;
                        productions.RemoveAt(i);
                        i--;
                        
                        Replacement(variable, "");
                    }
                }
            }
        }

        //add production needed to be added because of lambda production or unit productions...
        public static void Replacement(string variable, string replace)
        {
            List<Production> newProductions = new List<Production>();
            for (int i = 0; i < productions.Count; i++)
            {
                if(productions[i].left.character==variable)
                {
                    bool IsiTJustItselt = true;
                    for(int j=0;j<productions[i].right.Count;j++)
                    {
                        if(productions[i].right[j].character!=variable)
                        {
                            IsiTJustItselt = false;
                        }
                    }
                    if(IsiTJustItselt==true)
                    {
                        continue;
                    }
                }
                for (int j = 0; j < productions[i].right.Count; j++)
                {
                    if (productions[i].right[j].character == variable)
                    {
                        List<alphabet> RightHand = new List<alphabet>();
                        for (int k = 0; k < productions[i].right.Count; k++)
                        {
                            if (k != j)
                            {
                                RightHand.Add(productions[i].right[k]);
                            }
                            else if (k == j && replace != "")
                            {
                                for (int x = 0; x < variables.Count; x++)
                                {
                                    if (replace == variables[x].character)
                                    {
                                        RightHand.Add(variables[x]);
                                    }
                                }
                            }
                        }
                        if (RightHand.Count == 0)
                        {
                            alphabet lambda = new alphabet();
                            lambda.character = "#";
                            RightHand.Add(lambda);
                        }
                        Production newProduction = new Production(productions[i].left, RightHand);
                        newProductions.Add(newProduction);
                    }
                }
            }
            productions = productions.Concat(newProductions).ToList<Production>();
        }



        public static void RemoveUnits()
        {
            for (int i = 0; i < productions.Count; i++)
            {
                if (productions[i].right.Count == 1)
                {
                    bool Isvariable = false;
                    for (int j = 0; j < variables.Count; j++)
                    {
                        if (variables[j].character == productions[i].right[0].character)
                        {
                            Isvariable = true;
                            string variable = productions[i].left.character;
                            string rightVariable = productions[i].right[0].character;
                            productions.RemoveAt(i);
                            i--;
                            Replacement(variable, rightVariable);
                            NewProductions(variable, rightVariable);
                        }
                    }
                }
            }
        }

        public static void NewProductions(string variable, string rightVariable)
        {
            int index = -1;
            for (int i = 0; i < variables.Count; i++)
            {
                if (variables[i].character == variable)
                {
                    index = i;
                }
            }
            for (int i = 0; i < productions.Count; i++)
            {
                if (productions[i].left.character == rightVariable)
                {
                    if(productions[i].left!=variables[index])
                    {
                        Production newPro = new Production(variables[index], productions[i].right);
                        productions.Add(newPro);
                    }
                }
            }
        }
        public static void RemoveRepeatedProductions()
        {
            for (int i = 0; i < productions.Count; i++)
            {
                if (i != productions.Count - 1)
                {
                    for (int j = i + 1; j < productions.Count; j++)
                    {
                        if (productions[i].left.character == productions[j].left.character)
                        {
                            bool IsEqual = true;
                            if (productions[i].right.Count != productions[j].right.Count)
                            {
                                IsEqual = false;
                            }
                            else
                            {
                                for (int k = 0; k < productions[i].right.Count; k++)
                                {
                                    if (productions[i].right[k].character != productions[j].right[k].character)
                                    {
                                        IsEqual = false;
                                        break;
                                    }
                                }
                            }
                            if (IsEqual == true)
                            {
                                productions.RemoveAt(j);
                                j--;
                            }
                        }
                    }
                }
            }
        }

        public static void chomskeyConverter()
        {
            //for (int i = 0; i < productions.Count; i++)
            //{
            //    string x = "";
            //    for (int j = 0; j < productions[i].right.Count; j++)
            //    {
            //        x = x + productions[i].right[j].character;
            //    }
            //    Console.WriteLine(productions[i].left.character + " " + x);
            //}
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();
            if (!IsChomskey())
            {
                TerminalsHandler();
            }

            //for (int i = 0; i < productions.Count; i++)
            //{
            //    string x = "";
            //    for (int j = 0; j < productions[i].right.Count; j++)
            //    {
            //        x = x + productions[i].right[j].character;
            //    }
            //    Console.WriteLine(productions[i].left.character + " " + x);
            //}
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();
            //Console.WriteLine();
            if (!IsChomskey())
            {
                variablesHandler();
            }
        }

        public static bool IsChomskey()
        {
            List<Production> problems = new List<Production>();
            bool IsChomskey = true;
            for (int i = 0; i < productions.Count; i++)
            {
                bool isOK = false;
                if (productions[i].right.Count == 2)
                {
                    for (int j = 0; j < variables.Count; j++)
                    {
                        if (productions[i].right[0].character == variables[j].character && productions[i].right[1].character == variables[j].character)
                        {
                            isOK = true;
                        }
                    }
                }
                else if (productions[i].right.Count == 1)
                {
                    for (int j = 0; j < terminals.Count; j++)
                    {
                        if (terminals[j].character == productions[i].right[0].character)
                        {
                            isOK = true;
                        }
                    }
                }
                if (isOK == false)
                {
                    IsChomskey = false;
                }
            }
            return IsChomskey;
        }

        public static void TerminalsHandler()
        {
            for (int i = 0; i < terminals.Count; i++)
            {
                if (terminals[i].character == "#")
                {
                    continue;
                }
                List<alphabet> RightHandSide = new List<alphabet>();
                RightHandSide.Add(terminals[i]);
                bool IsAlreadyExists = false;
                for (int j = 0; j < variables.Count; j++)
                {
                    if (variables[j].character == "T" + terminals[i].character)
                    {
                        IsAlreadyExists = true;
                        Production newPro = new Production(variables[j], RightHandSide);
                        productions.Add(newPro);
                    }
                }
                if (!IsAlreadyExists)
                {
                    variable newVar = new variable();
                    newVar.character = "T" + terminals[i].character;
                    variables.Add(newVar);
                    Production newPro = new Production(newVar, RightHandSide);
                    productions.Add(newPro);
                }
            }

            for (int i = 0; i < productions.Count; i++)
            {
                if (productions[i].left.character.Contains("T") && productions[i].left.character.Length > 1)
                {
                    continue;
                }
                bool isOk = false;
                if (productions[i].right.Count == 1)
                {
                    for (int j = 0; j < terminals.Count; j++)
                    {
                        if (terminals[j].character == productions[i].right[0].character)
                        {
                            isOk = true;
                        }
                    }
                }
                if (isOk == true)
                {
                    continue;
                }
                for (int j = 0; j < productions[i].right.Count; j++)
                {
                    for (int k = 0; k < terminals.Count; k++)
                    {
                        if (productions[i].right[j].character == terminals[k].character)
                        {
                            for (int m = 0; m < variables.Count; m++)
                            {
                                if (variables[m].character == "T" + terminals[k].character)
                                {
                                    productions[i].right[j] = variables[m];
                                }
                            }

                        }
                    }
                }
            }
        }


        public static void variablesHandler()
        {
            int j = 0;
            for (int i = 0; i < productions.Count; i++)
            {
                if (productions[i].right.Count > 2)
                {
                    List<alphabet> newProRightHand = new List<alphabet>();
                    newProRightHand.Add(productions[i].right[0]);
                    newProRightHand.Add(productions[i].right[1]);

                    bool IsVariableAlreadyExists = false;
                    for (int k = 0; k < productions.Count; k++)
                    {
                        if (productions[k].right.Count == 2)
                        {
                            if (productions[k].right[0] == productions[i].right[0] && productions[k].right[1] == productions[i].right[1])
                            {
                                IsVariableAlreadyExists = true;
                                productions[i].right.RemoveAt(0);
                                productions[i].right[0] = productions[k].left;
                            }
                        }
                    }

                    if (IsVariableAlreadyExists == false)
                    {
                        variable newVar = new variable();
                        newVar.character = "V" + j.ToString();
                        variables.Add(newVar);
                        j++;

                        Production newPro = new Production(newVar, newProRightHand);
                        productions.Add(newPro);

                        productions[i].right.RemoveAt(0);
                        productions[i].right[0] = newVar;
                    }
                    i = -1;
                }
            }
        }

        public static bool AcceptReject(string str)
        {
            string word = str;
            int size = word.Length;
            int r = productions.Count;
            bool[,,] T = new bool[size, size, r];

            int i, j, k, x, y, z;

            for (int m = 0; m < size; m++)
            {
                for (int n = 0; n < r; n++)
                {
                    if (productions[n].Check(word[m]))
                    {
                        T[m, 0, n] = true;
                    }
                }
            }

            for (i = 1; i < size; i++)
            {
                for (j = 0; j < size - i; j++)
                {
                    for (k = 0; k < i; k++)
                    {
                        for (x = 0; x < r; x++)
                        {
                            for (y = 0; y < r; y++)
                            {
                                if (T[j, k, x] && T[j + k + 1, i - k - 1, y])
                                {
                                    for (z = 0; z < r; z++)
                                    {
                                        if (productions[z].Check(productions[x], productions[y]))
                                        {
                                            T[j, i, z] = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            bool result = false;
            for (int m = 0; m < r; m++)
            {
                if (T[0, size - 1, m] && productions[m].left.character == variables[0].character)
                {
                    result = true;
                    break;
                }
            }

            return result;
            //    int size = str.Length;
            //    List<variable>[,] TwoDarray = new List<variable>[size, size];
            //    for (int i = 0; i < size; i++)
            //    {
            //        for (int j = 0; j < size; j++)
            //        {
            //            TwoDarray[i, j] = new List<variable>();
            //        }
            //    }

            //    for (int i = 0; i < size; i++)
            //    {
            //        for (int j = 0; j < productions.Count; j++)
            //        {
            //            if (productions[j].right.Count == 1)
            //            {
            //                if (productions[j].right[0].character == str[i].ToString())
            //                {
            //                    TwoDarray[i, i].Add(productions[j].left);
            //                }
            //            }
            //        }
            //    }


            //    for (int l = 2; l < size; l++)
            //    {
            //        for(int i=0;i<size-l+1;i++)
            //        {
            //            int j = i + l - 1;
            //            for(int k=i;k<=j-1;k++)
            //            {
            //                for(int m=0;m<productions.Count;m++)
            //                {
            //                    if (productions[m].right.Count==2)
            //                    {
            //                        if(TwoDarray[i,k].Contains(productions[m].right[0]) && TwoDarray[k+1, j].Contains(productions[m].right[1]))
            //                        {
            //                            TwoDarray[i, j].Add(productions[m].left);
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }



            //    //for (int i = 0; i < str.Length; i++)
            //    //{
            //    //    for (int j = 0; j < str.Length; j++)
            //    //    {
            //    //        for (int k = 0; k <TwoDarray[i,j].Count;k++)
            //    //        {
            //    //            Console.Write(TwoDarray[i, j][k].character);
            //    //        }
            //    //        Console.Write("                   ");
            //    //    }
            //    //    Console.WriteLine();
            //    //}

            //    int index = -1;
            //    for(int i=0;i<variables.Count;i++)
            //    {
            //        if(variables[i].character=="S")
            //        {
            //            index = i;
            //            break;
            //        }
            //    }

            //    for(int i=0;i<productions.Count;i++)
            //    {
            //        if(productions[i].right.Count==2)
            //        {
            //            if(TwoDarray[0,size-2].Contains(productions[i].right[0]) && TwoDarray[1, size - 1].Contains(productions[i].right[1]) && productions[i].left==variables[index])
            //            {
            //                return true;
            //            }
            //        }
            //    }

            //    if (TwoDarray[0, size - 1].Contains(variables[index]))
            //    {
            //        return true;
            //    }
            //    else if(TwoDarray[0,size-2].Contains(variables[index]) && TwoDarray[1,size-1].Contains(variables[index]))
            //    {
            //        return true;
            //    }
            //    else if(TwoDarray[0,size-2].Count==0 && TwoDarray[1,size-1].Contains(variables[index]))
            //    {
            //        return true;
            //    }
            //    else if(TwoDarray[1, size - 1].Count == 0 && TwoDarray[0, size - 2].Contains(variables[index]))
            //    {
            //        return true;
            //    }
            //    else
            //    {
            //        return false;
            //    }


        }

    }
}