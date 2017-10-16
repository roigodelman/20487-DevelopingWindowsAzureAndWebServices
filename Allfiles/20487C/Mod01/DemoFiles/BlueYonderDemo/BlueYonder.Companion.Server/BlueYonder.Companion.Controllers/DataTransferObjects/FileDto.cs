using BlueYonder.Companion.Storage.TableEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueYonder.Companion.Controllers.DataTransferObjects
{
    public class FileDto
    {
        public string FileName { get; set; }

        public Uri Uri { get; set; }

        public int TripId { get; set; }

        public int LocationId { get; set; }

        public string Description { get; set; }

        public bool IsPrivate { get; set; }

        public FileType Type { get; set; }

        public string UserId { get; set; }
    }
}
