using CoreUi;

namespace Toolbox.Activities
{
    public interface IActivity
    {
        object Execute(object input);
    }
    public interface IActivity<TInput, TOutput> : IActivity
    {
        TOutput Execute(TInput input);
    }

    public abstract class ActivityBase<TInput, TOutput> : IActivity<TInput, TOutput>
    {
        public abstract TOutput Execute(TInput input);

        public virtual object Execute(object input)
        {
            return Execute((TInput) input);
        }
    }
}