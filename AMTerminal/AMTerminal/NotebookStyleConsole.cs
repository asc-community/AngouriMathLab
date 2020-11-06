using static System.Console;

namespace AMTerminal
{
    internal sealed class NotebookStyleConsole
    {
        private int currCellId = 1;

        public void WriteNextOutput<T>(T output)
        {
            ForegroundColor = System.ConsoleColor.White;
            WriteLine();
            WriteLine($"Out[{currCellId}] = {output}");
            currCellId++;
            ForegroundColor = System.ConsoleColor.White;
        }

        public void WriteError(string errorMessage)
        {
            ForegroundColor = System.ConsoleColor.Red;
            WriteLine();
            WriteLine($"Err[{currCellId}] = {errorMessage}");
            ForegroundColor = System.ConsoleColor.White;
        }

        public string ReadNextInput()
        {
            ForegroundColor = System.ConsoleColor.White;
            WriteLine();
            Write($"In[{currCellId}] := ");
            ForegroundColor = System.ConsoleColor.White;
            return ReadLine();
        }
    }
}
