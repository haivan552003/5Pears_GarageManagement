﻿using System;

namespace BE_API.Models
{
    public class car_seat
    {
        public int id { get; set; }
        public string name { get; set; }
        public int car_id { get; set; }
        public int row { get; set;}
        public int col { get; set; }
        public string status { get; set; }
    }
}
