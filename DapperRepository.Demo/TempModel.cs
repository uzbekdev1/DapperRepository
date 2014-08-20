using System;

namespace DapperRepository.Demo
{
    public class TempModel
    {
        public TempModel()
        {
            LastModifiedDate = DateTime.Now;
            CreatedDate = DateTime.Now;
        }

        public long ID { get; set; } 

        public string Name { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime LastModifiedDate { get; set; }
    }
}