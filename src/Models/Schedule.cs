using System;
using System.Collections.Generic;
using System.Linq;

namespace mmmsl.Models
{
    public class Schedule
    {
        public Division Division { get; set; } = new Division();
        public SortedDictionary<DateTime, IEnumerable<IGrouping<DateTimeOffset, Game>>> Entries { get; set; } = new SortedDictionary<DateTime, IEnumerable<IGrouping<DateTimeOffset, Game>>>();
    }
}