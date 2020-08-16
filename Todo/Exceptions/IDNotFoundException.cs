﻿using System;
using System.Runtime.Serialization;

namespace Todo.Web.Services
{
    public class IDNotFoundException : Exception
    {
        public IDNotFoundException(int ID) : base($"Item with ID={ID} is not found.") { }
    }
}