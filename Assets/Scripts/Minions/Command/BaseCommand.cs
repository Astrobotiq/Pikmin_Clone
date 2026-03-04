
namespace Minions.Command
{
    /// <summary>
    /// Tüm komutların türeyeceği, hedef bağımsız soyut temel sınıf.
    /// </summary>
    public abstract class BaseCommand : ICommand
    {
        protected bool _isFinished;

        public virtual void Execute() 
        {
            _isFinished = false;
        }

        public abstract void UpdateCommand();

        public virtual void Cancel()
        {
            _isFinished = true;
        }

        public bool IsFinished() => _isFinished;
    }
}