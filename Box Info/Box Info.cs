
// --> UPDATES : RIFERIMENTI

using Updates;

// <-- UPDATES : RIFERIMENTI

using System;
using cAlgo.API;


namespace cAlgo.Indicators
{

    [Indicator(IsOverlay = true, AccessRights = AccessRights.None)]
    public class BoxInfo : Indicator
    {

        // --> Inizializzo le valriabili per gestire gli input dei colori

        Colors BoxColor = Colors.Red;
        StaticPosition Position = StaticPosition.TopRight;

        [Parameter("Show Gross Profit ?", DefaultValue = true)]
        public bool showGross { get; set; }

        [Parameter("Show Net Profit ?", DefaultValue = true)]
        public bool showNet { get; set; }

        [Parameter("Show Leverage ?", DefaultValue = true)]
        public bool showLeva { get; set; }

        [Parameter("Show Antimartingala ?", DefaultValue = true)]
        public bool showAntimarty { get; set; }

        [Parameter("Color", DefaultValue = "Red")]
        public string boxcolor { get; set; }

        [Parameter("Position", DefaultValue = "TopRight")]
        public string position { get; set; }

// --> UPDATES : PARAMETRI

        [Parameter("Check Updates ?", DefaultValue = true)]
        public bool CheckUpdates { get; set; }

// <-- UPDATES : PARAMETRI

// --> UPDATES : VARIABILI E COSTANTI

        private const string VERSION = "1.0.4";
        private const string CANONICAL = "Box Info";
        private const string UPDATESENDPOINT = "https://ctrader.guru/downloads/box-info/";

// <-- UPDATES : VARIABILI E COSTANTI        

        private const double coeff = 2.15;

        protected override void Initialize()
        {

// --> UPDATES : MANIFESTO E CONTROLLO

            Print("{0} : {1}", CANONICAL, VERSION);

            if (CheckUpdates)
            {

                var mycheck = new Check(VERSION, UPDATESENDPOINT);

                int updated = mycheck.HaveNewVersion();

                string updatemex = string.Format("{0} : Unmanaged situation !", CANONICAL);

                switch (updated)
                {

                    case 0:

                        updatemex = string.Format("{0} : Your version is updated !", CANONICAL);

                        break;

                    case 1:

                        updatemex = string.Format("{0} : Updates available {1} !", CANONICAL, mycheck.ServerVersion);
                        ChartObjects.DrawText("Updates", updatemex, StaticPosition.TopLeft, Colors.Red);

                        break;

                    case -1:

                        updatemex = string.Format("{0} : Error on check updates ...", CANONICAL);

                        break;

                }

                Print(updatemex);

            }

// <-- UPDATES : MANIFESTO E CONTROLLO

            // --> Effettuo una conversione per rendere l'input utente leggibile dal codice

            Enum.TryParse(boxcolor, out BoxColor);
            Enum.TryParse(position, out Position);

        }

        public override void Calculate(int index)
        {

            if (IsLastBar)
            {

                double[] profits = GetProfitInformation();

                string tmpSpread = String.Format("{0:0.0}", GetSpreadInformation());
                string tmpGP = String.Format("{0:0.00}", profits[0]);
                string tmpNT = String.Format("{0:0.00}", profits[1]);

                string info = String.Format("{0} SPREAD\r\n{1}", Symbol.Code, tmpSpread);

                if (showGross)
                    info += String.Format("\r\n\r\nGROSS PROFIT\r\n{0}", tmpGP);

                if (showNet)
                    info += String.Format("\r\n\r\nNET PROFIT\r\n{0}", tmpNT);

                if (showLeva)
                    info += String.Format("\r\n\r\nLEVERAGE\r\n1:{0}", Account.PreciseLeverage);

                if (showAntimarty)
                {

                    double[] antM = GetAntimarty();

                    info += String.Format("\r\n\r\nANTIMARTINGALA\r\nBuy : {0} / Sell : {1}", antM[0], antM[1]);

                }

                ChartObjects.DrawText("Box", info, Position, BoxColor);

            }

        }

        private double[] GetAntimarty()
        {

            double tsbuy = 0.0;
            double tssell = 0.0;

            // --> Faccio la somma

            foreach (var position in Positions)
            {

                if (position.SymbolCode != Symbol.Code)
                    continue;

                if (position.TradeType == TradeType.Buy)
                    tsbuy += position.Quantity;
                if (position.TradeType == TradeType.Sell)
                    tssell += position.Quantity;

            }

            // --> Restituisco l'array con le informazioni

            double[] result = 
            {
                Math.Round(tsbuy / coeff, 2),
                Math.Round(tssell / coeff, 2)
            };

            return result;

        }

        private double GetSpreadInformation()
        {

            // --> Restituisco lo spread corrente

            return Math.Round(Symbol.Spread / Symbol.PipSize, 2);

        }

        private double[] GetProfitInformation()
        {

            // --> Raccolgo tutte le operazioni su questo simbolo

            double ttgp = 0.0;
            double ttnp = 0.0;

            // --> Faccio la somma

            foreach (var position in Positions)
            {

                if (position.SymbolCode != Symbol.Code)
                    continue;

                ttgp += position.GrossProfit;
                ttnp += position.NetProfit;

            }

            // --> Restituisco l'array con le informazioni

            double[] result = 
            {
                Math.Round(ttgp, 2),
                Math.Round(ttnp, 2)
            };

            return result;

        }

    }

}













