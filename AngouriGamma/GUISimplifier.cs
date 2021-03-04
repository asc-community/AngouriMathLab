﻿using AngouriMath;
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
        public async Task<Entity> Simplify(Entity expr)
        {
            if (lastTokenSource is not null)
                lastTokenSource.Cancel();
            var tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(3000);
            lastTokenSource = tokenSource;
            return await Task.Run(
                    () =>
                    {
                        MathS.Multithreading.SetLocalCancellationToken(tokenSource.Token);
                        return expr.Simplify();
                    }, tokenSource.Token
                    ).ContinueWith(t => t.IsCanceled ? "Timeout" : t.Result);
        }
    }
}
