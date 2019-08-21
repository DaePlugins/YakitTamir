using System;
using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using UnityEngine;
using fr34kyn01535.Uconomy;

namespace DaeYakitTamir
{
    internal class KomutTamir : IRocketCommand
    {
        public AllowedCaller AllowedCaller => AllowedCaller.Player;
        public string Name => "tamir";
        public string Help => "Aracı tamir eder.";
        public string Syntax => "";
        public List<string> Aliases => new List<string>();
        public List<string> Permissions => new List<string> { "dae.yakittamir.tamir" };

        public void Execute(IRocketPlayer komutuÇalıştıran, string[] parametreler)
        {
            var oyuncu = (UnturnedPlayer)komutuÇalıştıran;

            var araç = oyuncu.CurrentVehicle;
            if (araç == null)
            {
                UnturnedChat.Say(oyuncu, YakıtTamir.Örnek.Translate("AraçtaDeğilsin"), Color.red);
                return;
            }

            if (!araç.usesHealth)
            {
                UnturnedChat.Say(oyuncu, YakıtTamir.Örnek.Translate("TamirEdilemez"), Color.red);
                return;
            }

            if (araç.health == araç.asset.health)
            {
                UnturnedChat.Say(oyuncu, YakıtTamir.Örnek.Translate("TamirEdilmiş"));
                return;
            }

            var eksikSağlık = (ushort)(araç.asset.health - araç.health);
            var tamirEdilecekMiktar = eksikSağlık;

            if (parametreler.Length > 0)
            {
                if (!ushort.TryParse(parametreler[0], out tamirEdilecekMiktar))
                {
                    UnturnedChat.Say(oyuncu, YakıtTamir.Örnek.Translate("GeçersizGiriş"), Color.red);
                    return;
                }

                tamirEdilecekMiktar = Math.Min(tamirEdilecekMiktar, eksikSağlık);
            }

            var bakiye = YakıtTamir.Örnek.Configuration.Instance.XpKullanılsın ? oyuncu.Experience : Uconomy.Instance.Database.GetBalance(komutuÇalıştıran.Id);

            var bakiyedenDüşülecekMiktar = YakıtTamir.Örnek.Configuration.Instance.SabitFiyataTamirEt ?
                YakıtTamir.Örnek.Configuration.Instance.SabitTamirÜcreti :
                tamirEdilecekMiktar * YakıtTamir.Örnek.Configuration.Instance.SağlıkBaşınaÜcret;

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
			
            araç.askRepair(YakıtTamir.Örnek.Configuration.Instance.SabitFiyataTamirEt ? araç.asset.health : tamirEdilecekMiktar);

			UnturnedChat.Say(oyuncu, YakıtTamir.Örnek.Translate("TamirEdildi", bakiye - bakiyedenDüşülecekMiktar, bakiyedenDüşülecekMiktar));
        }
    }
}