using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BugGroupByDatePart.Classes.Models
{
    [Table("Order")]
    public class Order
    {
        [Column("Id"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; private set; }

        // irrelevant data
    }
}
