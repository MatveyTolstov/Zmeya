using System;
using System.Threading;

namespace Zmeyka
{
    internal class Snake
    {
        private int maxX, maxY;
        private bool running = true;
        private int snakeX, snakeY;

        public Snake(int maxX, int maxY)
        {
            this.maxX = maxX;
            this.maxY = maxY;
            snakeX = maxX / 2;
            snakeY = maxY / 2;

            Thread drawingThread = new Thread(DrawSnake);
            drawingThread.Start();
        }

        public void HandleInput(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow:
                    break;
                case ConsoleKey.DownArrow:

                    break;
                case ConsoleKey.LeftArrow:
                    break;
                case ConsoleKey.RightArrow:

                    break;
            }
        }

        private void DrawSnake()
        {
            while (running)
            {
                Console.Clear();
                Console.SetCursorPosition(snakeX, snakeY);
                Console.Write("@");
                Thread.Sleep(100); 
            }
        }
    }
}
