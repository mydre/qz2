using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAppDemo.Db.Model
{
    [Table("WXMessage")]
    public class WxMessage
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("wxID")]
        public string WxId { get; set; }

        [Column("createTime")]
        public string CreateTime { get; set; }

        [Column("isSend")]
        public int IsSend { get; set; }

        [Column("type")]
        public int Type { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("path")]
        public string Path { get; set; }

        [Column("status")]
        public int Status { get; set; }
    }
}
