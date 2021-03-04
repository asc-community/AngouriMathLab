using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using WpfMath;

namespace AngouriGamma
{
    public sealed class Renderer
    {
        // \begin{cases}
        // \implies
        // \mathbb{RR}

        public Task<BitmapSource> Render(string latex)
            => Task.Run(() =>
            {
                TexFormulaParser parser = new();
                try
                {
                    var formula = parser.Parse(latex.Replace(@"\implies", @"\Rightarrow"));
                    var src = formula.GetRenderer(TexStyle.Display, 50, "Arial").RenderToBitmap(0, 0, 100);
                    src.Freeze();
                    return src;
                }
                catch (NullReferenceException) // because who knows why it happens
                {
                    return null; 
                }
            });
    }
}
