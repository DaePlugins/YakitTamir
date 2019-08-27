using Rocket.API.Collections;
using Rocket.Core.Plugins;

namespace DaeYakitTamir
{
    public class YakıtTamir : RocketPlugin<YakıtTamirYapılandırma>
    {
        public static YakıtTamir Örnek { get; private set; }

        protected override void Load()
        {
            Örnek = this;
        }

        protected override void Unload()
        {
            Örnek = null;
        }

        public override TranslationList DefaultTranslations => new TranslationList
        {
            { "AraçtaDeğilsin", "Bir araçta değilsin." },
            { "GeçersizGiriş", "Geçersiz giriş yaptın." },
            { "EksikBakiye", "Gereken ücreti karşılayamıyorsun. Eksik bakiye: {0}." },
            { "YakıtDoldurulamaz", "Bu araca yakıt dolduramazsın." },
            { "YakıtDoldurulmuş", "Yakıt zaten dolu." },
            { "YakıtDolduruldu", "Aracının yakıtı {1} karşılığı dolduruldu. Mevcut bakiyen: {0}" },
            { "TamirEdilemez", "Bu aracı tamir edemezsin." },
            { "TamirEdilmiş", "Aracında en ufak bir çizik bile yok." },
            { "TamirEdildi", "Aracın {1} karşılığı tamir edildi. Mevcut bakiyen: {0}" }
        };
    }
}