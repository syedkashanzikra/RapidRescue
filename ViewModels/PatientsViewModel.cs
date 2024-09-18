using RapidRescue.Models;

namespace RapidRescue.ViewModels
{
    public class PatientsViewModel
    {
        public List<Tuple<string, string>> Breadcrumbs { get; set; }

        public List<Users> Patients { get; set; }
    }

}
