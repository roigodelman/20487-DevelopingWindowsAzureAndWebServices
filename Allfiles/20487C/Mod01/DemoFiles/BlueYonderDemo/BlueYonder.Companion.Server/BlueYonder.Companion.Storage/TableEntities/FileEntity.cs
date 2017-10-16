using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueYonder.Companion.Storage.TableEntities
{
    // TODO: Exercise 1: Task 3a: Derive from TableServiceEntity
    public class FileEntity : TableServiceEntity
    {
        public Uri Uri { get; set; }

        public string FileName { get; set; }

        public int TripId { get; set; }

        public string Description { get; set; }

        public int Type { get; set; }

        public bool IsPrivate { get; set; }

        public string UserId { get; set; }     
    }
}