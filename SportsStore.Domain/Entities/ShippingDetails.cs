using System.ComponentModel.DataAnnotations;

namespace OnlineStore.Domain.Entities {

    public class ShippingDetails {
        [Required(ErrorMessage = "Proszę podać nazwisko.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Proszę podać pierwszy wiersz adresu.")]
        [Display(Name = "Wiersz 1")]
        public string Line1 { get; set; }
        [Display(Name = "Wiersz 2")]
        public string Line2 { get; set; }
        [Display(Name = "Wiersz 3")]
        public string Line3 { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę miasta.")]
        public string City { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę województwa")]
        public string State { get; set; }

        public string Zip { get; set; }

        [Required(ErrorMessage = "Proszę podać nazwę kraju.")]
        public string Country { get; set; }

        public bool GiftWrap { get; set; }
    }
}
