using System;
using System.Collections.Generic;
using System.Text;

namespace AppParcial.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Notes
    {
        [Key]
        public int NotesId { get; set; }
        public string Contents { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        public Notes()
        {
            this.CreatedDate = DateTime.Now;
            this.ModifiedDate = DateTime.Now;
        }

    }
}
