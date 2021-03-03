using AngouriMath;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AngouriGamma
{
    public sealed class GUISimplifier
    {
        private CancellationTokenSource lastTokenSource;
        public Task<Entity> Simplify(Entity expr)
        {
            if (lastTokenSource is not null)
                lastTokenSource.Cancel();
            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(2000);            
            try
            {
                return Task.Run(
                        () =>
                        {
                            MathS.Multithreading.SetLocalCancellationToken(tokenSource.Token);
                            return expr.Simplify();
                        }, tokenSource.Token
                        );
            }
            catch (OperationCanceledException)
            {
                return "Timeout";
            }
        }
    }
}
