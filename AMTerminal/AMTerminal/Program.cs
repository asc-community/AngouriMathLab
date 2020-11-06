using AngouriMath;
using AngouriMath.Core.Exceptions;
using AMTerminal;
using static AMTerminal.ArgumentParser;
using AngouriMath.Extensions;

var exit = false;
var console = new NotebookStyleConsole();
var parser = new ArgumentParser();

while (!exit)
{
    try
    {
        var inp = console.ReadNextInput();
        var (cmd, arguments) = parser.Parse(inp);
        console.WriteNextOutput(
            cmd switch
            {
                "simplify" when AssertNumberOfArguments(cmd, 1, arguments.Length)
                    => arguments[0].Simplify(),
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
        console.WriteError($"Unknown command: {unrec}");
    }
    catch (ParseException parseException)
    {
        console.WriteError($"ParseException {parseException.Message}");
    }
    catch (AngouriBugException amBug)
    {
        console.WriteError($"AngouriMath's bug: {amBug.Message}. It would be appreciated if you reported about it to us");
    }
}

