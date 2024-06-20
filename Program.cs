using System;
using System.Collections.Generic;
using static System.Console;

namespace Snake_refactor
{
    class Program
    {
        private static void Main(string[] args)
        {
            SetupConsole();
            
            // Initialize game variables
            var score = 5;
            var gameOver = false;
            var head = new Pixel(WindowWidth / 2, WindowHeight / 2, ConsoleColor.Red);
            var snake = new List<Pixel>();
            var random = new Random();
            var berry = GenerateBerry(random, WindowWidth, WindowHeight);
            var lastKeyPress = DateTime.Now;
            var movement = Direction.Right;
            
            Clear();
            DrawBorder();
            
            // Game loop
            while (true)
            {
                ClearConsole();

                // Check if head hits wall and if it is eating berry
                if (head.IsHitWall(WindowWidth, WindowHeight))
                {
                    gameOver = true;
                    break;
                }
                if (head.IsEatingBerry(berry))
                {
                    score++;
                    berry = GenerateBerry(random, WindowWidth, WindowHeight);
                    snake.Add(new Pixel(head.XPos, head.YPos, ConsoleColor.Green));
                }
                
                DrawSnakeAndBerry(head, snake, berry);
                
                HandleInput(ref movement, ref lastKeyPress);
                
                MoveSnake(head, movement, snake, score);
                
                DisplayScore(score);
                
                // Pause for a short duration
                System.Threading.Thread.Sleep(100);
                
            }
            // Game over message
            DisplayGameOver(score);
        }

        private static void SetupConsole()
        {
            WindowHeight = 16;
            WindowWidth = 32;
            CursorVisible = false;
        }

        static void DrawBorder()
        {

            // Draw top and bottom borders
            for (int i = 0; i < WindowWidth; i++)
            {
                SetCursorPosition(i, 0);
                Write("■");
                SetCursorPosition(i, WindowHeight - 1);
                Write("■");
            }

            // Draw left and right borders
            for (int i = 0; i < WindowHeight; i++)
            {
                SetCursorPosition(0, i);
                Write("■");
                SetCursorPosition(WindowWidth - 1, i);
                Write("■");
                WriteLine();
            }
        }

        private static void ClearConsole()
        {
            // Clear the console area within the borders
            for (int i = 1; i < WindowHeight - 1; i++)
            {
                SetCursorPosition(1, i);
                Write(new string(' ', WindowWidth - 2));
            }
        }

        static void DrawSnakeAndBerry(Pixel head, List<Pixel> snake, Pixel berry)
        {
            // Draw head of the snake
            SetCursorPosition(head.XPos, head.YPos);
            ForegroundColor = head.ScreenColor;
            Write("■");

            // Draw body of the snake
            foreach (var pixel in snake)
            {
                SetCursorPosition(pixel.XPos, pixel.YPos);
                ForegroundColor = pixel.ScreenColor;
                Write("■");
            }

            // Draw berry
            SetCursorPosition(berry.XPos, berry.YPos);
            ForegroundColor = berry.ScreenColor;
            Write("■");
        }

        private static void HandleInput(ref Direction movement, ref DateTime lastKeyPress)
        {
            if (KeyAvailable)
            {
                var key = ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        if (movement != Direction.Down)
                            movement = Direction.Up;
                        break;
                    case ConsoleKey.DownArrow:
                        if (movement != Direction.Up)
                            movement = Direction.Down;
                        break;
                    case ConsoleKey.LeftArrow:
                        if (movement != Direction.Right)
                            movement = Direction.Left;
                        break;
                    case ConsoleKey.RightArrow:
                        if (movement != Direction.Left)
                            movement = Direction.Right;
                        break;
                }

                lastKeyPress = DateTime.Now;
            }
        }

        private static void MoveSnake(Pixel head, Direction movement, List<Pixel> snake, int score)
        {
            // Add head to snake
            snake.Add(new Pixel(head.XPos, head.YPos, ConsoleColor.Green));

            // Move head
            head.Move(movement);
            
            // Remove tail if snake length exceeds score
            if (snake.Count > score)
            {
                snake.RemoveAt(0);
            }
        }

        private static Pixel GenerateBerry(Random random, int windowWidth, int windowHeight)
        {
            return new Pixel(random.Next(1, windowWidth - 2), random.Next(1, windowHeight - 2), ConsoleColor.Cyan);
        }

        private static void DisplayScore(int score)
        {
            SetCursorPosition(WindowWidth - 15, 0); // Adjust position if needed
            ForegroundColor = ConsoleColor.White;
            Write($"Score: {score}");
        }

        private static void DisplayGameOver(int score)
        {
            SetCursorPosition(Console.WindowWidth / 5, WindowHeight / 2);
            WriteLine($"Game over, Score: {score}");
            SetCursorPosition(WindowWidth / 5, WindowHeight / 2 + 1);
            WriteLine("Press any key...");
            SetCursorPosition(WindowWidth / 5, WindowHeight / 2 + 2);
            CursorVisible = true;
            ReadKey();
        }
    }

    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    class Pixel
    {
        public int XPos { get; set; }
        public int YPos { get; set; }
        public ConsoleColor ScreenColor { get; }

        public Pixel(int xPos, int yPos, ConsoleColor screenColor)
        {
            XPos = xPos;
            YPos = yPos;
            ScreenColor = screenColor;
        }

        public bool IsHitWall(int windowWidth, int windowHeight)
        {
            return XPos == windowWidth - 1 || XPos == 0 || YPos == windowHeight - 1 || YPos == 0;
        }
        public bool IsEatingBerry(Pixel berry)
        {
            return XPos == berry.XPos && YPos == berry.YPos;
        }
        public void Move(Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    YPos--;
                    break;
                case Direction.Down:
                    YPos++;
                    break;
                case Direction.Left:
                    XPos--;
                    break;
                case Direction.Right:
                    XPos++;
                    break;
            }
        }
    }
}
