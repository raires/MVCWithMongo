using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MVCWithMongo.Models
{
    public class DocumentPrimaryModel
    {
       
        public object _id { get; set; } // this is the identity field for MongoDb.

        public int ID { get; set; } 

        [Required]
        public string Name { get; set; }


    }
}