using static System.Console;

namespace AMTerminal
{
    internal sealed class NotebookStyleConsole
    {
        private int currCellId = 1;

        public void WriteNextOutput<T>(T output)
        {
            WriteLine();
            WriteLine($"Out[{currCellId}] = {output}");
            currCellId++;
        }

        public void WriteError(string errorMessage)
        {
            WriteLine();
            WriteLine($"Err[{currCellId}] = {errorMessage}");
        }

        public string ReadNextInput()
        {
            WriteLine();
            Write($"In[{currCellId}] := ");
            return ReadLine();
        }
    }
}
