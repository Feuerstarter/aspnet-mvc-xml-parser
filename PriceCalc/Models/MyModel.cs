using System.ComponentModel.DataAnnotations;

namespace PriceCalc.Models {

    public class MyModel {

        [Required]
        [RegularExpression("^[0-9]*$")]
        public double Price { get; set; }

        public double Calc { get; set; }

        public double Dollar { get; set; }

        public double Euro { get; set; }

        public double Grivna { get; set; }

    }
}
