using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Introduction.Common
{
    public class AddFilter
    {
        public string SearchQuery {  get; set; } //
        public Guid? CarTypeId { get; set; } //
        public string? Make {  get; set; } //
        public string? Model {  get; set; } //
        public int? YearFrom { get; set; } //
        public int? YearTo { get; set; } //
        public int? MileageFrom { get; set; } //
        public int? MileageTo { get; set; } //
    }
}
