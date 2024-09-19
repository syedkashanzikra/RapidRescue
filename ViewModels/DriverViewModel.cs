using RapidRescue.Models;

namespace RapidRescue.ViewModels
{
    public class DriverViewModel
    {
        public List<Tuple<string, string>> Breadcrumbs { get; set; }

        public List<Users> Drivers { get; set; }
    }
}
