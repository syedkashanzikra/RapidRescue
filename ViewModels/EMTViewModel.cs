using RapidRescue.Models;

namespace RapidRescue.ViewModels
{
    public class EMTViewModel
    {
        public List<Tuple<string, string>> Breadcrumbs { get; set; }

        public List<Users> EMT { get; set; }
    }


}
