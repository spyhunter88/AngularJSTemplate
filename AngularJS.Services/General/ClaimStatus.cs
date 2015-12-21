using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AngularJS.Services.General
{
    public enum Status
    {
        CLAIM_DRAFT = 1,
        CLAIM_PREPARE_RUNNING = 10,
        CLAIM_RUNNING_PROGRAM = 11,
        CLAIM_ENDING_PROGRAM = 12,
        CLAIM_WAITING_APPROVAL = 13,
        CLAIM_PREPARE_CLAIM = 14,
        CLAIM_SUBMITTED_CLAIM = 15,
        CLAIM_DONE = 16,
        CLAIM_FAILED = 17,
        CLAIM_PAYMENT_PARTIAL = 25,
        CLAIM_ALLOCATION = 30
    }
}
