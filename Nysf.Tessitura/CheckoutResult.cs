using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    public enum CheckoutResult : byte
    {
        Unprocessed, Succeeded, CreditCardInvalid, CreditCardTimeMismatch, ServerTimeout,
        CreditCardNotAuthorized, CreditCardDeclined
    }
}