using System;

namespace Code.UI.Presenters
{
    public abstract class Presenter<T> : IDisposable
    {
        protected T View { get; }

        protected Presenter(T view)
        {
            View = view;
        }

        public abstract void Dispose();
    }
}