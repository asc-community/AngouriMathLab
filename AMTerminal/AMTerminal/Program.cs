using AngouriMath.Core.Exceptions;
using AMTerminal;
using static AMTerminal.ArgumentParser;
using AngouriMath.Extensions;
using System;
using AngouriMath;

Console.WriteLine(@$"
Terminal for AngouriMath 2019-2020. AngouriMath's assembly version: {typeof(Entity).Assembly.GetName().Version}. MIT License.
");

var console = new NotebookStyleConsole();
var parser = new ArgumentParser();
var exit = false;
Console.Title = "AngouriMath terminal";

while (!exit)
{
#if DEBUG
#else
    try
    {
#endif
        var inp = console.ReadNextInput();
        var (cmd, arguments) = parser.Parse(inp);

        console.WriteNextOutput(
            cmd switch
            {
                "exit" when AssertNumberOfArguments(cmd, 0, arguments.Length)
                    => throw new UserExitException(),
                "simplify" when AssertNumberOfArguments(cmd, 1, arguments.Length)
                    => arguments[0].ToEntity().Simplify(),
                "eval" or "evaluate" when AssertNumberOfArguments(cmd, 1, arguments.Length)
                    => arguments[0].ToEntity().Evaled,
                "solve" when AssertNumberOfArguments(cmd, 2, arguments.Length)
                    => arguments[1].Solve(arguments[0]),
                "diff" or "differentiate" when AssertNumberOfArguments(cmd, 2, arguments.Length)
                    => arguments[1].Differentiate(arguments[0]),
                "int" or "integrate" when AssertNumberOfArguments(cmd, 2, arguments.Length)
                    => arguments[1].Integrate(arguments[0]),
                "lim" or "limit" when AssertNumberOfArguments(cmd, 3, arguments.Length)
                    => arguments[2].Limit(arguments[0], arguments[1]),
                "limleft" when AssertNumberOfArguments(cmd, 3, arguments.Length)
                    => arguments[2].Limit(arguments[0], arguments[1], AngouriMath.Core.ApproachFrom.Left),
                "limright" when AssertNumberOfArguments(cmd, 3, arguments.Length)
                    => arguments[2].Limit(arguments[0], arguments[1], AngouriMath.Core.ApproachFrom.Right),
                "latex" when AssertNumberOfArguments(cmd, 1, arguments.Length)
                    => arguments[0].Latexise(),
                var unrecognized => throw new UnrecognizedCommand(unrecognized)
            }
            );
#if DEBUG
#else
    }
    catch (UserExitException)
    {
        exit = true;
    }
    catch (EmptyRequestException)
    {
        console.WriteError("Empty request");
    }
    catch (InvalidNumberOfArgumentsException exp)
    {
        console.WriteError($"Unexpected number of arguments for {exp.Command}. Expected: {exp.Expected}, Actual: {exp.Actual}");
    }
    catch (UnrecognizedCommand unrec)
    {
        console.WriteError($"Unknown command: {unrec.Command}");
    }
    catch (ParseException parseException)
    {
        console.WriteError($"ParseException {parseException.Message}");
    }
    catch (AngouriBugException amBug)
    {
        console.WriteError($"AngouriMath's bug: {amBug.Message}. It would be appreciated if you reported about it to us");
    }
#endif
}


internal sealed class UserExitException : Exception { }