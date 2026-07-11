using System;
using System.Collections.Generic;
using System.Text;

namespace FixFlow.Domain.Enums;

public enum RequestStatus
{
    Created = 1,
    Assigned = 2,
    Accepted = 3,
    InProgress = 4,
    Completed = 5,
    CustomerConfirmed = 6,
    Cancelled = 7,
    Rejected = 8,
    OnHold = 9
}
