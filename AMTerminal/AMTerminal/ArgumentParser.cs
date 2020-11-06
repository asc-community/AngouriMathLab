using AngouriMath;
using System;
using System.Linq;

namespace AMTerminal
{
    internal abstract class InvalidInputException : Exception { }

    internal sealed class EmptyRequestException : InvalidInputException { }

    internal sealed class InvalidNumberOfArgumentsException : InvalidInputException
    {
        public string Command { get; }
        public int Expected { get; }
        public int Actual { get; }

        public InvalidNumberOfArgumentsException(string forCommand, int expected, int actual)
        {
            Command = forCommand;
            Expected = expected;
            Actual = actual;
        }
    }

    internal sealed class UnrecognizedCommand : InvalidInputException
    {
        public string Command { get; }
        public UnrecognizedCommand(string command)
            => Command = command;
    }

    internal sealed class ArgumentParser
    {
        internal ArgumentParser()
        {

        }

        internal (string cmd, string[] args) Parse(string input)
        {
            var items = input.Split("<<");
            if (items.Length == 0)
                throw new EmptyRequestException();
            (string cmd, string[] args) res;
            res.cmd = items[0].Trim();
            res.args = items.Skip(1).Select(c => c.Trim()).ToArray();
            return res;
        }

        internal static bool AssertNumberOfArguments(string command, int expected, int actual)
        {
            if (expected != actual)
                throw new InvalidNumberOfArgumentsException(command, expected, actual);
            return true;
        }
    }
}
