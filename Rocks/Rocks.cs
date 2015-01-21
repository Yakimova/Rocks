using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
/*
    Problem 12.** Falling Rocks

    Implement the "Falling Rocks" game in the text console.
        A small dwarf stays at the bottom of the screen and can move left and right (by the arrows keys).
        A number of rocks of different sizes and forms constantly fall down and you need to avoid a crash.
        Rocks are the symbols ^, @, *, &, +, %, $, #, !, ., ;, - distributed with appropriate density. The dwarf is (O).
    Ensure a constant game speed by Thread.Sleep(150).
    Implement collision detection and scoring system.

 */

struct Rock
{
    public int x;
    public int y;
    public string c;
    public ConsoleColor color;
}

struct Dwarf
{
    public int x;
    public int y;
    public ConsoleColor color;
    public string c;
}
class Rocks
{
    static void PrintOnPossition(int x, int y, string c, ConsoleColor color = ConsoleColor.Gray)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(c);
    }
    static void PrintStringOnPossition(int x, int y, string c, ConsoleColor color = ConsoleColor.Gray)
    {
        Console.SetCursorPosition(x, y);
        Console.ForegroundColor = color;
        Console.Write(c);
    }
    static void Main()
    {
        int speed = 100;
        int score = 0;
        int playfieldWidth = 20;
        int livesCount = 3;
        Console.BufferHeight = Console.WindowHeight = 15;
        Console.BufferWidth = Console.WindowWidth = 40;
        Dwarf userDwarf = new Dwarf();
        userDwarf.x = 2;
        userDwarf.y = Console.WindowHeight - 1;
        userDwarf.color = ConsoleColor.White;
        userDwarf.c = "(0)";
        Random randomGenerator = new Random();
        List<Rock> rocks = new List<Rock>();

        char[] rockdesign = "^@*&+$#!.;".ToCharArray();
        String[] colorNames = ConsoleColor.GetNames(typeof(ConsoleColor));
        int numColors = colorNames.Length;

        while (true)
        {
            speed++;
            if (speed > 500)
                speed = 500;

            if (speed<=200)
                score += 2;
            else if (speed <= 400)
                score += 4;
            else if(speed > 400)
                score += 8;
            bool isHit = false;

            // create rock
            {
                Rock newRock = new Rock();

                string colorName = colorNames[randomGenerator.Next(numColors)];
                ConsoleColor color = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorName);
                newRock.color = color;

                newRock.x = randomGenerator.Next(0, playfieldWidth);
                newRock.y = 0;

                int design = randomGenerator.Next(0, 9);
                newRock.c = Convert.ToString(rockdesign[design]);

                rocks.Add(newRock);
            }

            // move dwarf
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo pressedKey = Console.ReadKey(true);
                if (pressedKey.Key == ConsoleKey.LeftArrow)
                {
                    if ((userDwarf.x - 1) >= 0)
                    {
                        userDwarf.x -= 1;
                    }
                }
                else if (pressedKey.Key == ConsoleKey.RightArrow)
                {
                    if (userDwarf.x - 1 < (playfieldWidth-4))
                    {
                        userDwarf.x += 1;
                    }
                }
            }

            List<Rock> newList = new List<Rock>();
            //move rocks
            for (int i = 0; i < rocks.Count; i++)
            {
                Rock oldRock = rocks[i];
                Rock newRock = new Rock();
                newRock.x = oldRock.x;
                newRock.y = oldRock.y + 1;
                newRock.c = oldRock.c;
                newRock.color = oldRock.color;
                if (newRock.y == userDwarf.y && (newRock.x == userDwarf.x || newRock.x == (userDwarf.x + 1) || newRock.x == (userDwarf.x+2)))
                {
                    livesCount--;
                    score -= 20;
                    isHit = true;
                    speed += 10;
                    if (livesCount <= 0)
                    {
                        PrintStringOnPossition(8, 7, "GAME OVER!!!", ConsoleColor.Red);
                        PrintStringOnPossition(8, 10, "Press [enter] to exit", ConsoleColor.Red);
                        Console.ReadLine();
                        Environment.Exit(0);
                    }
                }
                if (newRock.y<Console.WindowHeight)
                    newList.Add(newRock);
            }
            rocks = newList;

            // clear the console
            Console.Clear();

            //draw rocks
            foreach (Rock rock in rocks)
            {
                PrintOnPossition(rock.x, rock.y, rock.c, rock.color);
            }
            if (isHit)
            {
                rocks.Clear();
                PrintOnPossition(userDwarf.x, userDwarf.y, "(0)", ConsoleColor.Red);
            }
            else
                PrintOnPossition(userDwarf.x, userDwarf.y, userDwarf.c, userDwarf.color);

            // information
            PrintStringOnPossition(24, 2, "Lives: "+ livesCount, ConsoleColor.White);
            PrintStringOnPossition(24, 4, "Speed: " + speed, ConsoleColor.Green);
            PrintStringOnPossition(24, 6, "Score: " + score, ConsoleColor.Red);

            //slow down program
            System.Threading.Thread.Sleep(600-speed);
        }
    }
}
