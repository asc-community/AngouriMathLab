using static System.Console;

namespace AMTerminal
{
    internal sealed class NotebookStyleConsole
    {
        private int currCellId;

        public void WriteNextOutput<T>(T output)
        {
            currCellId++;
            WriteLine($"Out[{currCellId}] = {output}");
        }

        public void WriteError(string errorMessage)
        {
            WriteLine($"Err[{currCellId}] = {errorMessage}");
        }

        public string ReadNextInput()
        {
            Write($"In[{currCellId}] := ");
            return ReadLine();
        }
    }
}
