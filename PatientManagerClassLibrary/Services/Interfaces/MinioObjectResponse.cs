﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagerClassLibrary.Services.Interfaces
{
    public class MinioObjectResponse
    {
        public string ContentType { get; set; }
        public Stream Data { get; set; }
    }
}
