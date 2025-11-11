using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADC.Domain.Models;

public class AuthenticationModel
{
    public bool IsAuthenticated { get; set; }
    public Guid? UserId { get; set; }
}