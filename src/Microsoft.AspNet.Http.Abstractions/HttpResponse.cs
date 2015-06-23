// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading.Tasks;

namespace Microsoft.AspNet.Http
{
    public abstract class HttpResponse
    {
        private static readonly Func<object, Task> _callbackDelegate = callback => ((Func<Task>)callback)();
        private static readonly Func<object, Task> _disposeDelegate = disposable =>
        {
            ((IDisposable)disposable).Dispose();
            return Task.FromResult(0);
        };

        public abstract HttpContext HttpContext { get; }

        public abstract int StatusCode { get; set; }

        public abstract IHeaderDictionary Headers { get; }

        public abstract Stream Body { get; set; }

        public abstract long? ContentLength { get; set; }

        public abstract string ContentType { get; set; }

        public abstract IResponseCookies Cookies { get; }

        public abstract bool HasStarted { get; }

        public abstract void OnResponseStarting(Func<object, Task> callback, object state);

        public virtual void OnResponseStarting(Func<Task> callback) => OnResponseStarting(_callbackDelegate, callback);

        public abstract void OnResponseCompleted(Func<object, Task> callback, object state);

        public virtual void DisposeOnCompleted(IDisposable disposable) => OnResponseStarting(_disposeDelegate, disposable);

        public virtual void OnResponseCompleted(Func<Task> callback) => OnResponseCompleted(_callbackDelegate, callback);

        public virtual void Redirect(string location) => Redirect(location, permanent: false);

        public abstract void Redirect(string location, bool permanent);
    }
}
