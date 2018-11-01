using System;
using System.Collections.Generic;
using System.Text;

namespace IRunes.Models
{
    public abstract class BaseModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
    }
}
