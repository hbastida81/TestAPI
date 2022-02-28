using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Test.Entities
{
    public class Activity
    {
        [Required(ErrorMessage = "Activity_Id is required")]
        public int Activity_Id { get; set; }

        public string Activity_Title { get; set; }
        public string Activity_Product { get; set; }
        public string Activity_Status { get; set; }
        //public DateTime Activity_Created_at { get; set; }        
        //public DateTime Activity_Updated_at { get; set; }        

    }
}
