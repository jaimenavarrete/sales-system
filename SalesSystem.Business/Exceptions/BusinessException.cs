﻿using System;

namespace SalesSystem.Business.Exceptions
{
    public class BusinessException : Exception
    {
        public BusinessException(string message) : base (message)
        { }
    }
}
