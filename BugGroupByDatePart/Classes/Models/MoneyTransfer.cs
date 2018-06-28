using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BugGroupByDatePart.Classes.Models
{
    [Table("MoneyTransfer")]
    public class MoneyTransfer
    {
        [Column("Id"), Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; private set; }

        [Column("Date"), Required]
        public DateTime Date { get; set; }

        [Column("Amount", TypeName = "decimal(18,2)"), Required]
        public decimal Amount { get; set; }



        // -- Associations -- //

        public virtual CustomUser Author { get; internal set; }

        [Column("AuthorId"), ForeignKey(nameof(Author)), Required]
        private string AuthorId { get; set; }

        // the event that led to the money transfer
        public virtual Event Event { get; internal set; }

        [Column("EventId"), ForeignKey(nameof(Event)), Required]
        private long EventId { get; set; }


    }

    // transfer associated to the client entity
    public class MoneyTransferClient : MoneyTransfer
    {
        // -- Associations -- //
        public virtual Client Client { get; internal set; }

        [Column("ClientId"), ForeignKey(nameof(Client)), Required]
        private long ClientId { get; set; }
    }

    // transfer associated to the user entity
    public class MoneyTransferUser : MoneyTransfer
    {
        // -- Associations -- //
        public virtual CustomUser User { get; internal set; }

        [Column("UserId"), ForeignKey(nameof(User)), Required]
        private string UserId { get; set; }
    }
}
