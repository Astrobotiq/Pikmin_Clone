namespace Minions.Command
{
    public interface ICommand
    {
        void Execute();       
        void UpdateCommand(); 
        void Cancel();        
        bool IsFinished();
    }
}