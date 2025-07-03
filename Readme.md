# 🎯 Mastermind Game (Console Version)

A simple console-based implementation of the classic **Mastermind** code-breaking game written in C#.

In this game, the computer generates (or accepts via command line) a **4-digit secret code** consisting of **distinct digits from 0 to 8**. The player has a limited number of attempts to guess the code. After each guess, the game provides feedback:

- ✅ **Well-Placed**: Correct digit in the correct position.
- 🔄 **Misplaced**: Correct digit in the wrong position.

---

## 🕹️ How to Play

### Rules
- The secret code consists of **4 distinct digits** from `0` to `8`.
- You must guess the code within a fixed number of attempts (default: **10**).
- After each guess, you'll see:
  - 🟩 Green blocks for well-placed digits.
  - 🟨 Yellow blocks for misplaced digits.

---

## 📦 Features

- Random or user-defined secret code
- Input validation and colored console feedback
- Command-line customization
- Fully modular architecture

---

## 🚀 Getting Started

### 🧰 Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/en-us/download) (version 6.0 or later)

### 🛠️ Build and Run

```bash
dotnet build
dotnet run
```

---

## 🧪 Command-Line Options

You can provide optional arguments:

| Argument | Description                                 | Example               |
|----------|---------------------------------------------|-----------------------|
| `-c`     | Set a custom secret code                    | `-c 1234`             |
| `-t`     | Set a custom number of max attempts         | `-t 15`               |


Example usage:

```bash
dotnet run -- -c 1234 -t 12
```

### Exit Command (End Of File):
Ctrl+C on Windows or Ctrl-D on Unix/Linux/Mac

---

## 📁 Project Structure

| File/Class           | Responsibility                                               |
|----------------------|--------------------------------------------------------------|
| `Code`               | Represents the secret or guessed code                        |
| `GuessResult`        | Encapsulates well-placed and misplaced feedback              |
| `CodeGenerator`      | Generates a valid 4-digit random code                        |
| `GameEngine`         | Handles evaluation, win conditions, and attempt tracking     |
| `GameInterface`      | Handles all user-facing console input/output with color      |
| `MastermindGame`     | Ties together engine and UI, manages game flow               |
| `ArgumentParser`     | Parses CLI arguments for code and attempt count              |
| `Program`            | Entry point and orchestrator                                 |

---

## 🎨 Sample Output

```text
Can you break the code?
Enter a valid guess.
Attempts remaining: 10
1234

Well-placed pieces: 1 🟩 
Misplaced   pieces: 2 🟨 🟨 

Attempts remaining: 9
...
```

---

## 📄 License

This project is open-source and available under the [MIT License](LICENSE).

---

## 🤝 Contributing

Contributions, issues and feature requests are welcome! Feel free to open an issue or submit a pull request.

---

## ✨ Author

Developed by Majed Altaemi