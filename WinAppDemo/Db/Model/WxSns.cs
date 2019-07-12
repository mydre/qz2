using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAppDemo.Db.Model
{
    [Table("WXSns")]
    public class WxSns
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("wxID")]
        public string WxId { get; set; }

        [Column("type")]
        public int Type { get; set; }

        [Column("createTime")]
        public string CreateTime { get; set; }

        [Column("content")]
        public string Content { get; set; }

        [Column("comment")]
        public string Comment { get; set; }
       
        [Column("supportNum")]
        public int SupportNum { get; set; }

        [Column("commentNum")]
        public int CommentNum { get; set; }
    }
}
