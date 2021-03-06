﻿using System;
using System.Collections.Generic;
using System.Text;

namespace atomapp.Models
{
    /// <summary>
    /// Коментарий задачи
    /// </summary>
    public class TskComment
    {
        public long Id { get; set; }
        public long TskId { get; set; }
        public DateTimeOffset AddedAt { get; set; }
        public long CreatorId { get; set; }
        public string CreatorName { get; set; }
        public string Text { get; set; }
        public string AudioGuid { get; set; }
    }
}
