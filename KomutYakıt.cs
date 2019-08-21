using System;
using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using UnityEngine;
using fr34kyn01535.Uconomy;

namespace DaeYakitTamir
{
    internal class KomutYakıt : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "yakit";
        public string Help => "Aracın yakıtını doldurur.";
        public string Syntax => "";
        public List<string> Aliases => new List<string>{ "benzin" };
        public List<string> Permissions => new List<string>{ "dae.yakittamir.yakit", "dae.yakittamir.benzin" };
        
        public void Execute(IRocketPlayer komutuÇalıştıran, string[] parametreler)
        {
            var oyuncu = (UnturnedPlayer)komutuÇalıştıran;

            var araç = oyuncu.CurrentVehicle;
            if (araç == null)
            {
                UnturnedChat.Say(oyuncu, YakıtTamir.Örnek.Translate("AraçtaDeğilsin"), Color.red);
                return;
            }

            if (!araç.usesFuel)
            {
                UnturnedChat.Say(oyuncu, YakıtTamir.Örnek.Translate("YakıtDoldurulamaz"), Color.red);
                return;
            }

            if (araç.fuel == araç.asset.fuel)
            {
                UnturnedChat.Say(oyuncu, YakıtTamir.Örnek.Translate("YakıtDoldurulmuş"));
                return;
            }

            var eksikYakıt = (ushort)(araç.asset.fuel - araç.fuel);
            var doldurulacakMiktar = eksikYakıt;

            if (parametreler.Length > 0)
            {
                if (!ushort.TryParse(parametreler[0], out doldurulacakMiktar))
                {
                    UnturnedChat.Say(oyuncu, YakıtTamir.Örnek.Translate("GeçersizGiriş"), Color.red);
                    return;
                }

                doldurulacakMiktar = Math.Min(doldurulacakMiktar, eksikYakıt);
            }

            var bakiye = YakıtTamir.Örnek.Configuration.Instance.XpKullanılsın ? oyuncu.Experience : Uconomy.Instance.Database.GetBalance(komutuÇalıştıran.Id);
            var bakiyedenDüşülecekMiktar = YakıtTamir.Örnek.Configuration.Instance.SabitFiyataDoldur ?
                YakıtTamir.Örnek.Configuration.Instance.SabitDolumÜcreti :
                doldurulacakMiktar * YakıtTamir.Örnek.Configuration.Instance.YakıtBaşınaÜcret;

            if (bakiyedenDüşülecekMiktar > bakiye)
            {
                UnturnedChat.Say(oyuncu, YakıtTamir.Örnek.Translate("EksikBakiye", bakiyedenDüşülecekMiktar - bakiye), Color.red);
                return;
            }

            if (YakıtTamir.Örnek.Configuration.Instance.XpKullanılsın)
            {
                oyuncu.Experience -= (uint)Math.Min(bakiyedenDüşülecekMiktar, uint.MaxValue);
            }
            else
            {
                Uconomy.Instance.Database.IncreaseBalance(komutuÇalıştıran.Id, -bakiyedenDüşülecekMiktar);
            }

            araç.askFillFuel(YakıtTamir.Örnek.Configuration.Instance.SabitFiyataDoldur ? araç.asset.fuel : doldurulacakMiktar);

			UnturnedChat.Say(oyuncu, YakıtTamir.Örnek.Translate("YakıtDolduruldu", bakiye - bakiyedenDüşülecekMiktar, bakiyedenDüşülecekMiktar));
        }
    }
}