using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revel8R.BusinessEntities
{
    public class Result
    {
        public Result()
        {
            this.WasSuccessful = false;
            this.Message = string.Empty;
        }

        public Result(bool initialValue)
        {
            this.WasSuccessful = initialValue;
            this.Message = string.Empty;
        }

        public bool WasSuccessful { get; set; }

        public string Message { get; set; }
    }
}
