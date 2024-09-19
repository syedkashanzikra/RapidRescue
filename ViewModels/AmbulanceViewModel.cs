using System;
using System.Collections.Generic;
using RapidRescue.Models;

namespace RapidRescue.ViewModels
{
    public class AmbulanceViewModel
    {
        // List to store breadcrumb data
        public List<Tuple<string, string>> Breadcrumbs { get; set; }

        // List of ambulances to be displayed in the view
        public List<Ambulance> Ambulances { get; set; }
    }
}
