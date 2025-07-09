using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscountCodeSystem.Server.Models;

public class DiscountCode
{
    public string Code { get; set; }
    public bool Used { get; set; } = false;
}
