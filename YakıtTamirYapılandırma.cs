using Rocket.API;

namespace DaeYakitTamir
{
    public class YakıtTamirYapılandırma : IRocketPluginConfiguration
    {
        public bool SabitFiyataDoldur { get; set; }
        public decimal SabitDolumÜcreti { get; set; }
        public decimal YakıtBaşınaÜcret { get; set; }

        public bool SabitFiyataTamirEt { get; set; }
        public decimal SabitTamirÜcreti { get; set; }
        public decimal SağlıkBaşınaÜcret { get; set; }

        public bool XpKullanılsın { get; set; }

        public void LoadDefaults()
        {
            SabitFiyataDoldur = true;
            SabitDolumÜcreti = 250.00m;
            YakıtBaşınaÜcret = 1.00m;

            SabitFiyataTamirEt = true;
            SabitTamirÜcreti = 250.00m;
            SağlıkBaşınaÜcret = 1.00m;

            XpKullanılsın = true;
        }
    }
}