using System;
using System.Collections.Generic;


namespace MastermindGame
{
    // Represents the result of a guess evaluation
    public class GuessResult
    {
        public int WellPlaced { get; set; }
        public int Misplaced { get; set; }

        public GuessResult(int wellPlaced, int misplaced)
        {
            WellPlaced = wellPlaced;
            Misplaced = misplaced;
        }
    }

    // Represents a code (either secret or guess)
    public class Code
    {
        private readonly List<char> _pieces;

        public Code(string codeString)
        {
            _pieces = codeString.ToList();
        }

        public Code(List<char> pieces)
        {
            _pieces = new List<char>(pieces);
        }

        public List<char> Pieces => new List<char>(_pieces);

        public bool IsValid()
        {
            // Check if all pieces are valid digits 0-8
            if (_pieces.Any(p => !char.IsDigit(p) || p < '0' || p > '8'))
                return false;

            // Check if all pieces are distinct
            if (_pieces.Distinct().Count() != _pieces.Count)
                return false;

            // Check if exactly 4 pieces
            return _pieces.Count == 4;
        }

        public override bool Equals(object obj)
        {
            if (obj is Code other)
            {
                return _pieces.SequenceEqual(other._pieces);
            }
            return false;
        }

    }

    // Handles code generation and validation logic
    public class CodeGenerator
    {
        private readonly Random _random;

        public CodeGenerator()
        {
            _random = new Random();
        }

        public Code GenerateRandomCode()
        {
            var availablePieces = new List<char> { '0', '1', '2', '3', '4', '5', '6', '7', '8' };
            var selectedPieces = new List<char>();

            for (int i = 0; i < 4; i++)
            {
                int index = _random.Next(availablePieces.Count);
                selectedPieces.Add(availablePieces[index]);
                availablePieces.RemoveAt(index);
            }

            return new Code(selectedPieces);
        }
    }

    // Handles game logic and evaluation
    public class GameEngine
    {
        private readonly Code _secretCode;
        private int _attempts;
        private readonly int _maxAttempts;

        public GameEngine(Code secretCode, int maxAttempts = 10)
        {
            _secretCode = secretCode;
            _maxAttempts = maxAttempts;
            _attempts = 0;
        }

        public bool IsGameOver => _attempts >= _maxAttempts;
        public int RemainingAttempts => _maxAttempts - _attempts;

        public GuessResult EvaluateGuess(Code guess)
        {
            _attempts++;

            var secretPieces = _secretCode.Pieces;
            var guessPieces = guess.Pieces;

            int wellPlaced = 0;
            int misplaced = 0;

            // Count well-placed pieces
            for (int i = 0; i < 4; i++)
            {
                if (secretPieces[i] == guessPieces[i])
                {
                    wellPlaced++;
                }
            }

            // Count total matches (well-placed and misplaced)
            int totalMatches = 0;
            for (int i = 0; i < 4; i++)
            {
                if (secretPieces.Contains(guessPieces[i]))
                {
                    totalMatches++;
                }
            }

            // Misplaced = total matches - well placed
            misplaced = totalMatches - wellPlaced;

            return new GuessResult(wellPlaced, misplaced);
        }

        public bool IsWinningGuess(Code guess)
        {
            return _secretCode.Equals(guess);
        }
    }

    // Handles user input and output
    public class GameInterface
    {
        public void DisplayWelcome()
        {
            Console.WriteLine("Can you break the code?");
            Console.WriteLine("Enter a valid guess.");
        }

        public void DisplayWin()
        {
            WriteColoredText("\nCongratz! You did it!\n", ConsoleColor.Green);
        }

        public void DisplayResult(GuessResult result)
        {
            Console.Write($"\nWell-placed pieces: {result.WellPlaced} ");
            for (int i = 0; i < result.WellPlaced; i++)
                WriteColoredText("█ ", ConsoleColor.Green);

            Console.Write($"\nMisplaced   pieces: {result.Misplaced} ");
            for (int i = 0; i < result.Misplaced; i++)
                WriteColoredText("█ ", ConsoleColor.Yellow);
            Console.Write("\n");
        }

        public void DisplayGameOver()
        {
            WriteColoredText("\nGame Over! You've run out of attempts.\n", ConsoleColor.Red);
        }

        public void DisplayInvalidInput()
        {
            WriteColoredText("\nInvalid input. Please enter 4 distinct digits from 0-8.\n", ConsoleColor.Red);
        }

        public string ReadInput()
        {
            return Console.ReadLine();
        }

        public void DisplayAttempts(int remaining)
        {
            Console.WriteLine($"Attempts remaining: {remaining}");
        }

        public void WriteColoredText(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }
    }

    // Main game controller
    public class MastermindGame
    {
        private readonly GameEngine _engine;
        private readonly GameInterface _interface;
        private bool _gameEnded;

        public MastermindGame(Code secretCode, int maxAttempts = 10)
        {
            _engine = new GameEngine(secretCode, maxAttempts);
            _interface = new GameInterface();
            _gameEnded = false;
        }

        public void Run()
        {
            _interface.DisplayWelcome();

            while (!_engine.IsGameOver && !_gameEnded)
            {
                _interface.DisplayAttempts(_engine.RemainingAttempts);

                string input = _interface.ReadInput();

                // Handle Ctrl+C on Windows / Ctrl-D on Unix/Linux/Mac (EOF)
                if (input == null)
                {
                    _interface.WriteColoredText("\nGoodbye! (EOF received)\n", ConsoleColor.Cyan);
                    _gameEnded = true;
                    break;
                }

                // Skip empty input
                if (string.IsNullOrWhiteSpace(input))
                    continue;

                var guess = new Code(input.Trim());

                if (!guess.IsValid())
                {
                    _interface.DisplayInvalidInput();
                    continue;
                }

                if (_engine.IsWinningGuess(guess))
                {
                    _interface.DisplayWin();
                    _gameEnded = true;
                    break;
                }

                var result = _engine.EvaluateGuess(guess);
                _interface.DisplayResult(result);
            }

            if (_engine.IsGameOver && !_gameEnded)
            {
                _interface.DisplayGameOver();
            }
        }
    }



    // Command line argument parser
    public class ArgumentParser
    {
        public string SecretCode { get; private set; }
        public int MaxAttempts { get; private set; } = 10;

        public bool Parse(string[] args)
        {
            try
            {
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i].ToLower())
                    {
                        case "-c":
                            if (i + 1 < args.Length)
                            {
                                SecretCode = args[i + 1];
                                i++; // Skip next argument as it's the code
                            }
                            break;

                        case "-t":
                            if (i + 1 < args.Length)
                            {
                                if (int.TryParse(args[i + 1], out int attempts) && attempts > 0)
                                {
                                    MaxAttempts = attempts;
                                    i++; // Skip next argument as it's the attempts
                                }
                            }
                            break;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    // Main program entry point
    class Program
    {
        static void Main(string[] args)
        {
            var parser = new ArgumentParser();
            if (!parser.Parse(args))
            {
                Console.WriteLine("Invalid arguments.");
                return;
            }

            Code secretCode;

            if (!string.IsNullOrEmpty(parser.SecretCode))
            {
                secretCode = new Code(parser.SecretCode);
                if (!secretCode.IsValid())
                {
                    Console.WriteLine("Invalid secret code. Code must be 4 distinct digits from 0-8.");
                    return;
                }
            }
            else
            {
                var generator = new CodeGenerator();
                secretCode = generator.GenerateRandomCode();
            }

            var game = new MastermindGame(secretCode, parser.MaxAttempts);
            game.Run();
        }
    }
}