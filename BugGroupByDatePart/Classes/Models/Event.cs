using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BugGroupByDatePart.Classes.Models
{
    // the event that led to the money transfer 
    [Table("Event")]
    public abstract class Event
    {
        [Column("Id"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; private set; }
    }

    public class OrderThings : Event
    {
        // -- Associations -- //
        public virtual Order Order { get; internal set; }

        [Column("OrderId"), ForeignKey(nameof(Order)), Required]
        private long OrderId { get; set; }
    }




    // irrelevant types of events:

    public class TransferToUser : Event
    {
        // irrelevant
    }

    public class TransferToClient : Event
    {
        // irrelevant
    }

    // etc...
}
