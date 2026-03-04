using Enemy.Controls;

namespace Minions.Command
{
    /// <summary>
    /// Minion'un olduğu yerde durmasını sağlayan pasif komut.
    /// </summary>
    public class IdleCommand : BaseCommand
    {
        private readonly Mover _mover;

        public IdleCommand(Mover mover)
        {
            _mover = mover;
        }

        public override void Execute()
        {
            base.Execute();
            _mover.Stop();
            // İleride buraya rastgele animasyon oynatma mantığı eklenebilir.
        }

        public override void UpdateCommand()
        {
            // Idle durumunda yapılacak ekstra bir update yoksa boş kalabilir.
            // Ancak OCP'ye uygun olarak burası gelecekteki davranışlara açıktır.
        }
    }
}