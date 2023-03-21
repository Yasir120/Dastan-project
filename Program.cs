//Skeleton Program code for the AQA A Level Paper 1 Summer 2023 examination
//this code should be used in conjunction with the Preliminary Material
//written by the AQA Programmer Team
//developed in the Visual Studio Community Edition programming environment

using System;
using System.Collections.Generic;
using System.Security;
using System.Text.RegularExpressions;

namespace Dastan
{
    class Program
    {
        static void Main(string[] args)
        {
            //creating an object of Dastan 
            Dastan ThisGame = new Dastan(6, 6, 4);
            ThisGame.PlayGame();
            Console.WriteLine("Goodbye!");
            Console.ReadLine();
        }
    }
}
