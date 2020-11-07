using AMTerminal;
using static AMTerminal.ArgumentParser;
using AngouriMath.Extensions;
using System;
using AngouriMath;
using System.Linq;
using AngouriMath.Core.Exceptions;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using static AngouriMath.Entity.Number;

Console.ForegroundColor = ConsoleColor.Yellow;
Console.WriteLine(@$"Terminal for AngouriMath
AngouriMath's assembly version: {typeof(Entity).Assembly.GetName().Version}.
(c) Angouri 2019-2020 MIT license
");
Console.ForegroundColor = ConsoleColor.White;

var console = new NotebookStyleConsole();
var parser = new ArgumentParser();
var exit = false;
Console.Title = "AngouriMath terminal";

var outputStack = new Stack<Entity>();
var popVar = MathS.Var("pop");

while (!exit)
{
#if DEBUG
#else
    try
    {
#endif
        var inp = console.ReadNextInput();
        var (cmd, arguments) = parser.Parse(inp);

        var entityArguments = arguments.Select(PreProcessor).ToArray();

        Entity PreProcessor(string str)
        {
            var res = str.ToEntity().Replace(ent => ent == popVar ? outputStack.Pop() : ent);
            for (int i = 1; i < 10; i++)
            {
                var var = MathS.Var($"pop_{i}");
                if (!res.Nodes.Contains(var))
                    break;
                res = res.Substitute(var, outputStack.Pop());
            }
            return res;
        }

        static string PostProcessor(Entity entity)
            => (entity.Complexity > 1000 ? entity.InnerSimplified : entity.Simplify()).ToString();

        static string Evaluate(Entity str)
            => str.EvalNumerical() is Real re ? re.EDecimal.ToString() : str.EvalNumerical().ToString();

        var output = (
            cmd switch
            {
                "exit" when AssertNumberOfArguments(cmd, 0, entityArguments.Length)
                    => throw new UserExitException(),

                "simplify" when AssertNumberOfArguments(cmd, 1, entityArguments.Length)
                    => PostProcessor(entityArguments[0].Simplify()),

                "solve" when AssertNumberOfArguments(cmd, 2, entityArguments.Length)
                    => PostProcessor(entityArguments[1].Solve(AsVar(entityArguments[0]))),

                "diff" or "differentiate" when AssertNumberOfArguments(cmd, 2, entityArguments.Length)
                    => PostProcessor(entityArguments[1].Differentiate(AsVar(entityArguments[0]))),

                "int" or "integrate" when AssertNumberOfArguments(cmd, 2, entityArguments.Length)
                    => PostProcessor(entityArguments[1].Integrate(AsVar(entityArguments[0]))),

                "lim" or "limit" when AssertNumberOfArguments(cmd, 3, entityArguments.Length)
                    => PostProcessor(entityArguments[2].Limit(AsVar(entityArguments[0]), entityArguments[1])),

                "limleft" when AssertNumberOfArguments(cmd, 3, entityArguments.Length)
                    => PostProcessor(entityArguments[2].Limit(AsVar(entityArguments[0]), entityArguments[1], AngouriMath.Core.ApproachFrom.Left)),

                "limright" when AssertNumberOfArguments(cmd, 3, entityArguments.Length)
                    => PostProcessor(entityArguments[2].Limit(AsVar(entityArguments[0]), entityArguments[1], AngouriMath.Core.ApproachFrom.Right)),

                "latex" when AssertNumberOfArguments(cmd, 1, entityArguments.Length)
                    => entityArguments[0].Latexise(),

                "eval" or "evaluate" when AssertNumberOfArguments(cmd, 1, entityArguments.Length)
                    => Evaluate(entityArguments[0]),

                "" => throw new EmptyRequestException(),

                var expression => expression.Last() == '\u0005' ? Evaluate(string.Join("", expression.SkipLast(1))) : PostProcessor(PreProcessor(expression).Simplify())
            }
            );

        outputStack.Push(output);
        console.WriteNextOutput(output);

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
        console.WriteError($"Parse error: {parseException.Message}");
    }
    catch (AngouriBugException amBug)
    {
        console.WriteError($"AngouriMath's bug: {amBug.Message}. It would be appreciated if you reported about it to us");
    }
#endif
}

static Entity.Variable AsVar(Entity entity)
{
    if (entity is Entity.Variable v)
        return v;
    throw new ExpectedVariableException();
}

internal sealed class UserExitException : Exception { }