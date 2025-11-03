using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.DTOs
{
    public class XRayExamDTO
    {
        public int Id { get; set; }

        public string XRayName { get; set; }

        public float XRayDose { get; set; }

        public byte XRayShots { get; set; }

        public DateOnly XRayDate { get; set; }

        public string Category {  get; set; }

        public string? XRayDiagnose { get; set; }

        public bool? XRayPatology { get; set; }

        public int? XRayCost { get; set; }

        public string? Doctor { get; set; }

        public string? Laborant { get; set; }

        public string? XRayModality { get; set; }

        public bool? InOperation { get; set; }

        public bool? Contrast { get; set; }

        public PatientDTO Patient { get; set; }
    }
}
