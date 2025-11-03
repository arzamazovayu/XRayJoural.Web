using XRayJournal.Core;

namespace ForDebug
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DataContext data= new DataContext();

            data.Database.EnsureCreated();
        }
    }
}
