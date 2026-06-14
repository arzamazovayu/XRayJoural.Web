using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core.DTOs
{
    public class RecordDTO
    {
        public int Id { get; set; }

        public int PatientId { get; set; }

        public int NumberId { get; set; }

        public int? ExamId { get; set; }

        public int UserId { get; set; }

        public DateOnly Date { get; set; }

        public virtual PatientDTO? Patient { get; set; }

        public virtual NumberDTO? Number { get; set; }

        public virtual XRayExamDTO? Exam { get; set; }

        public virtual UserDTO? User { get; set; }
    }
}
