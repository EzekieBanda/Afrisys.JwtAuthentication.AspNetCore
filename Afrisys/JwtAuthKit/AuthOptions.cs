using System;
using System.Collections.Generic;
using System.Text;

namespace Afrisys.JwtAuthKit;

public class AuthOptions
{
    public string Authority { get; set; } = default!;
    public string Audience { get; set; } = default!;
}
