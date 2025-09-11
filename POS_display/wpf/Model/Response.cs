using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransactionService.Models.TransModel;

namespace POS_display.wpf.Model
{
    public class Response<T>
    {
        public BaseResponse baseResponse { get; set; }
        public T result { get; set; }
    }
}
