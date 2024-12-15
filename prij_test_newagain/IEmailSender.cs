using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prij_test_newagain
{
    
    public interface IEmailSender
    {
         Task SendEmailAsync(string email, string subject, string message);
    }
    
}
