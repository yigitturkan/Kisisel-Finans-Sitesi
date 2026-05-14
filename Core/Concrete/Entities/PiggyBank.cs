using Core.Abstract.Bases; // Mevcut BaseEntity yapını kullanıyoruz
using System;

namespace Core.Concrete.Entities
{
    public class PiggyBank : BaseEntity
    {
        public string Name { get; set; }           // Kumbara ismi: "Araba İçin", "Tatil" vb.
        public decimal TargetAmount { get; set; }  // Hedeflenen miktar
        public decimal CurrentAmount { get; set; } // Biriken miktar

        // Eğer projedeki ana paran farklı döviz cinslerindeyse (Dolar, Altın vb.)
        // Kumbaranın hangi cinsten olduğunu belirtmek için:
        public string CurrencyCode { get; set; }   // "USD", "TRY", "GOLD" vb.

        public string Icon { get; set; }           // Web arayüzünde seçilecek ikon (opsiyonel)
        public string UserId { get; set; }
    }
}