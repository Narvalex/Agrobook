namespace Agrobook.Server
{
    public class EventStoreLoader
    {
        public enum StartConflictOption
        {
            Kill, Connect, Error
        }

        internal static void Load(StartConflictOption opt = StartConflictOption.Connect)
        {
            
        }
    }
}
