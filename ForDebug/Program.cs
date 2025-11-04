using Mapster;
using XRayJournal.Core;
using XRayJournal.Core.OutputModels;
using XRayJournal.DAL;

namespace ForDebug
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DataContext data= new DataContext();

            //data.Database.EnsureCreated();

            PatientRepository repository = new PatientRepository(data);

            var a = repository.GetAll();

            var b = a.Adapt<List<PatientOutputModel>>();
        }
    }
}
