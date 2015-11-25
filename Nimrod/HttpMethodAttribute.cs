using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nimrod
{
    /// <summary>
    /// List of valid verbs for generating type script action controller
    /// Taken from https://msdn.microsoft.com/library/system.web.mvc.actionmethodselectorattribute
    /// </summary>
    public enum HttpMethodAttribute
    {
        Get, Post, Put, Delete, Head, Options, Patch
    }
}
