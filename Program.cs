using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zmeyka
{
    class Program
    {

        static  int gridW = 90;
        static  int gridH = 25;
        static Cell[,] grid = new Cell[gridH, gridW];
        static Cell currentCell;
        static Cell food;
        static int FoodCount;
        static int direction;
        static int speed = 1;
        static bool Populated = false;
        static bool Lost = false;
        static int snakeLength;

        static void Main(string[] args)
        {
            if (!Populated)
            {
                FoodCount = 0;
                snakeLength = 5;
                SplashScreen();
                Console.Clear();
                populateGrid();
                currentCell = grid[(int)Math.Ceiling((double)gridH / 2), (int)Math.Ceiling((double)gridW / 2)];
                updatePos();
                addFood();
                Populated = true;
            }

            while (!Lost)
            {
                Restart();
            }
        }
        static void SplashScreen()
        {

            string[] ss = new string[10];
            ss[0] = "  sss   ssssss sssss s s   s ssssss";
            ss[1] = " s   s  s    s s     s s   s s    s";
            ss[2] = "  s     s    s s     s s   s s    s";
            ss[3] = "   s    s    s sssss s   s   ssssss";
            ss[4] = "    s   s    s s     s   s   s    s";
            ss[5] = "     s  s    s s     s   s   s    s";
            ss[6] = "s    s  s    s s     s   s   s    s";
            ss[7] = " ssss   ssssss s     s   s   s    s";
            ss[8] = "                                   ";
            ss[9] = "The best Magistr C#, Sorry I was too lazy to write your middle name";

            Console.ForegroundColor = ConsoleColor.Magenta;
            for (int i = 0; i < ss.Length; i++)
                for (int j = 0; j < ss[i].Length; j++)
                {
                    Console.SetCursorPosition(j + 25, i + 10);
                    Console.Write(ss[i][j]);
                    System.Console.Beep(200, 1);
                    System.Threading.Thread.Sleep(50);
                }
            Console.SetCursorPosition(30, 25);
            Console.Write("Press any key to start");
            Console.ResetColor();
            Console.ReadKey();
        }
        static void Restart()
        {
            Console.SetCursorPosition(0, 0);
            printGrid();
            Console.WriteLine("Length: {0}", snakeLength);
            getInput();
        }

        static void updateScreen()
        {
            Console.SetCursorPosition(0, 0);
            printGrid();
            Console.WriteLine("Length: {0}", snakeLength);
        }

        static void getInput()
        {

            ConsoleKeyInfo input;
            while (!Console.KeyAvailable)
            {
                Move();
                updateScreen();
            }
            input = Console.ReadKey();
            doInput(input.KeyChar);
        }

        static void checkCell(Cell cell)
        {
            if (cell.val == "$")
            {
                eatFood();
            }
            if (cell.visited)
            {
                Lose();
            }
        }

        static void Lose()
        {
            Console.Clear();
            Console.SetCursorPosition(50, 10);
            Console.WriteLine("Поражение(");
            Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);
            Environment.Exit(-1);
        }

        static void doInput(char inp)
        {
            switch (inp)
            {
                case 'w':
                    goUp();
                    break;
                case 's':
                    goDown();
                    break;
                case 'a':
                    goRight();
                    break;
                case 'd':
                    goLeft();
                    break;
            }
        }

        static void addFood()
        {
            Random r = new Random();
            Cell cell;
            while (true)
            {
                cell = grid[r.Next(grid.GetLength(0)), r.Next(grid.GetLength(1))];
                if (cell.val == " ")
                    cell.val = "$";
                break;
            }
        }

        static void eatFood()
        {
            snakeLength += 1;
            addFood();
        }

        static void goUp()
        {
            if (direction == 2)
                return;
            direction = 0;
        }

        static void goRight()
        {
            if (direction == 3)
                return;
            direction = 1;
        }

        static void goDown()
        {
            if (direction == 0)
                return;
            direction = 2;
        }

        static void goLeft()
        {
            if (direction == 1)
                return;
            direction = 3;
        }

        static void Move()
        {
            if (direction == 0)
            {

                if (grid[currentCell.y - 1, currentCell.x].val == "#")
                {
                    Lose();
                    return;
                }
                visitCell(grid[currentCell.y - 1, currentCell.x]);
            }
            else if (direction == 1)
            {
                if (grid[currentCell.y, currentCell.x - 1].val == "#")
                {
                    Lose();
                    return;
                }
                visitCell(grid[currentCell.y, currentCell.x - 1]);
            }
            else if (direction == 2)
            {

                if (grid[currentCell.y + 1, currentCell.x].val == "#")
                {
                    Lose();
                    return;
                }
                visitCell(grid[currentCell.y + 1, currentCell.x]);
            }
            else if (direction == 3)
            {
                if (grid[currentCell.y, currentCell.x + 1].val == "#")
                {
                    Lose();
                    return;
                }
                visitCell(grid[currentCell.y, currentCell.x + 1]);
            }
            Thread.Sleep(speed * 80);
        }

        static void visitCell(Cell cell)
        {
            currentCell.val = "@";
            currentCell.visited = true;
            currentCell.decay = snakeLength;
            checkCell(cell);
            currentCell = cell;
            updatePos();
        }

        static void updatePos()
        {

            currentCell.Set("@");
            if (direction == 0)
            {
                currentCell.val = "^";
            }
            else if (direction == 1)
            {
                currentCell.val = "<";
            }
            else if (direction == 2)
            {
                currentCell.val = "v";
            }
            else if (direction == 3)
            {
                currentCell.val = ">";
            }

            currentCell.visited = false;
            return;
        }

        static void populateGrid()
        {
            for (int col = 0; col < gridH; col++)
            {
                for (int row = 0; row < gridW; row++)
                {
                    Cell cell = new Cell();
                    cell.x = row;
                    cell.y = col;
                    cell.visited = false;

                    if (cell.x == 0 || cell.x > gridW - 2 || cell.y == 0 || cell.y > gridH - 2)
                    {
                        cell.Set("#");
                    }
                    else
                    {
                        cell.Clear();
                    }

                    grid[col, row] = cell;
                }
            }
        }

        static void printGrid()
        {
            string toPrint = "";
            for (int col = 0; col < gridH; col++)
            {
                for (int row = 0; row < gridW; row++)
                {
                    grid[col, row].decaySnake();
                    toPrint += grid[col, row].val;

                }
                toPrint += "\n";
            }
            Console.WriteLine(toPrint);
        }

    }
}