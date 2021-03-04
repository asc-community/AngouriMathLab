using AngouriMath;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AngouriGamma
{
    public sealed class GUILimit
    {
        public int Timeout { get; set; }
        private CancellationTokenSource lastTokenSource;
        public async Task<Entity> FindLimit(Entity expr, Entity.Variable x, Entity dest, AngouriMath.Core.ApproachFrom from)
        {
            if (lastTokenSource is not null)
                lastTokenSource.Cancel();
            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(Timeout);
            lastTokenSource = tokenSource;
            return await Task.Run(
                    () =>
                    {
                        MathS.Multithreading.SetLocalCancellationToken(tokenSource.Token);
                        return expr.Limit(x, dest, from);
                    }, tokenSource.Token
                    ).ContinueWith(t => t.IsCanceled ? "Timeout" : t.Result);
        }
    }
}
