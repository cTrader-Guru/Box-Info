/*  CTRADER GURU --> Indicator Template 1.0.8

    Homepage    : https://ctrader.guru/
    Telegram    : https://t.me/ctraderguru
    Twitter     : https://twitter.com/cTraderGURU/
    Facebook    : https://www.facebook.com/ctrader.guru/
    YouTube     : https://www.youtube.com/channel/UCKkgbw09Fifj65W5t5lHeCQ
    GitHub      : https://github.com/ctrader-guru

*/

using System;
using cAlgo.API;
using cAlgo.API.Indicators;
using cAlgo.API.Internals;

namespace cAlgo.Indicators
{

    [Indicator(IsOverlay = true, AccessRights = AccessRights.None)]
    public class BoxInfo : Indicator
    {

        #region Enums

        /// <summary>
        /// Enumeratore per scegliere nelle opzioni la posizione del box info
        /// </summary>
        public enum MyPosition
        {

            TopRight,
            TopLeft,
            BottomRight,
            BottomLeft

        }

        public enum MyColors
        {

            AliceBlue,
            AntiqueWhite,
            Aqua,
            Aquamarine,
            Azure,
            Beige,
            Bisque,
            Black,
            BlanchedAlmond,
            Blue,
            BlueViolet,
            Brown,
            BurlyWood,
            CadetBlue,
            Chartreuse,
            Chocolate,
            Coral,
            CornflowerBlue,
            Cornsilk,
            Crimson,
            Cyan,
            DarkBlue,
            DarkCyan,
            DarkGoldenrod,
            DarkGray,
            DarkGreen,
            DarkKhaki,
            DarkMagenta,
            DarkOliveGreen,
            DarkOrange,
            DarkOrchid,
            DarkRed,
            DarkSalmon,
            DarkSeaGreen,
            DarkSlateBlue,
            DarkSlateGray,
            DarkTurquoise,
            DarkViolet,
            DeepPink,
            DeepSkyBlue,
            DimGray,
            DodgerBlue,
            Firebrick,
            FloralWhite,
            ForestGreen,
            Fuchsia,
            Gainsboro,
            GhostWhite,
            Gold,
            Goldenrod,
            Gray,
            Green,
            GreenYellow,
            Honeydew,
            HotPink,
            IndianRed,
            Indigo,
            Ivory,
            Khaki,
            Lavender,
            LavenderBlush,
            LawnGreen,
            LemonChiffon,
            LightBlue,
            LightCoral,
            LightCyan,
            LightGoldenrodYellow,
            LightGray,
            LightGreen,
            LightPink,
            LightSalmon,
            LightSeaGreen,
            LightSkyBlue,
            LightSlateGray,
            LightSteelBlue,
            LightYellow,
            Lime,
            LimeGreen,
            Linen,
            Magenta,
            Maroon,
            MediumAquamarine,
            MediumBlue,
            MediumOrchid,
            MediumPurple,
            MediumSeaGreen,
            MediumSlateBlue,
            MediumSpringGreen,
            MediumTurquoise,
            MediumVioletRed,
            MidnightBlue,
            MintCream,
            MistyRose,
            Moccasin,
            NavajoWhite,
            Navy,
            OldLace,
            Olive,
            OliveDrab,
            Orange,
            OrangeRed,
            Orchid,
            PaleGoldenrod,
            PaleGreen,
            PaleTurquoise,
            PaleVioletRed,
            PapayaWhip,
            PeachPuff,
            Peru,
            Pink,
            Plum,
            PowderBlue,
            Purple,
            Red,
            RosyBrown,
            RoyalBlue,
            SaddleBrown,
            Salmon,
            SandyBrown,
            SeaGreen,
            SeaShell,
            Sienna,
            Silver,
            SkyBlue,
            SlateBlue,
            SlateGray,
            Snow,
            SpringGreen,
            SteelBlue,
            Tan,
            Teal,
            Thistle,
            Tomato,
            Transparent,
            Turquoise,
            Violet,
            Wheat,
            White,
            WhiteSmoke,
            Yellow,
            YellowGreen

        }

        public enum MyBoxType
        {

            Box,
            Banner

        }

        #endregion

        #region Identity

        /// <summary>
        /// Nome del prodotto, identificativo, da modificare con il nome della propria creazione 
        /// </summary>
        public const string NAME = "Box Info";

        /// <summary>
        /// La versione del prodotto, progressivo, utilie per controllare gli aggiornamenti se viene reso disponibile sul sito ctrader.guru
        /// </summary>
        public const string VERSION = "1.0.9";

        #endregion

        #region Params

        /// <summary>
        /// L'identità del prodotto
        /// </summary>
        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://ctrader.guru/product/box-info/")]
        public string ProductInfo { get; set; }

        /// <summary>
        /// La percentuale di incremento dell'antimartingala
        /// </summary>
        [Parameter("Antimartingala K%", Group = "Params", DefaultValue = 2.15)]
        public double Corff { get; set; }

        /// <summary>
        /// Opzione per la visualizzazione del pannello informazioni
        /// </summary>
        [Parameter("Panel type ?", Group = "Options", DefaultValue = MyBoxType.Banner)]
        public MyBoxType PanelType { get; set; }

        /// <summary>
        /// Opzione per la visualizzazione del gross profit
        /// </summary>
        [Parameter("Show Gross Profit ?", Group = "Options", DefaultValue = true)]
        public bool ShowGross { get; set; }

        /// <summary>
        /// Opzione per la visualizzazione del net profit
        /// </summary>
        [Parameter("Show Net Profit ?", Group = "Options", DefaultValue = true)]
        public bool ShowNet { get; set; }

        /// <summary>
        /// Opzione per la visualizzazione della leva, potrebbe variare
        /// </summary>
        [Parameter("Show Leverage ?", Group = "Options", DefaultValue = true)]
        public bool ShowLeva { get; set; }

        /// <summary>
        /// Opzione per la visualizzazione della size esponenziale di antimartingala
        /// </summary>
        [Parameter("Show Antimartingala ?", Group = "Options", DefaultValue = true)]
        public bool ShowAntimarty { get; set; }

        /// <summary>
        /// Il colore del font
        /// </summary>
        [Parameter("Color", Group = "Styles", DefaultValue = MyColors.LightBlue)]
        public MyColors Boxcolor { get; set; }

        /// <summary>
        /// Opzione per la posizione del box info in verticale
        /// </summary>
        [Parameter("Vertical Position", Group = "Styles", DefaultValue = VerticalAlignment.Top)]
        public VerticalAlignment VAlign { get; set; }

        /// <summary>
        /// Opzione per la posizione del box info in orizontale
        /// </summary>
        [Parameter("Horizontal Position", Group = "Styles", DefaultValue = HorizontalAlignment.Left)]
        public HorizontalAlignment HAlign { get; set; }

        #endregion

        #region Property
        
        #endregion

        #region Indicator Events

        /// <summary>
        /// Eseguito all'avvio dell'indicatore
        /// </summary>
        protected override void Initialize()
        {

            // --> Stampo nei log la versione corrente
            Print("{0} : {1}", NAME, VERSION);
            
        }

        /// <summary>
        /// Eseguito ad ogni tick
        /// </summary>
        /// <param name="index">L'indice della candela corrente</param>
        public override void Calculate(int index)
        {

            // --> Eseguo la logica solo se è l'ultima candela
            if (!IsLastBar)
                return;

            switch (PanelType)
            {
                case MyBoxType.Box:

                    _drawBox(index);

                    break;

                case MyBoxType.Banner:

                    _drawBanner(index);

                    break;
                    
            }

            // <-- Non va si impalla :)
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Restituisce lo spread corrente
        /// </summary>
        private double _getSpreadInformation()
        {

            // --> Restituisco lo spread corrente
            return Math.Round(Symbol.Spread / Symbol.PipSize, 2);

        }

        /// <summary>
        /// Calcola la size per la prossima posizione in antimartingala
        /// </summary>
        /// <returns>Restiruisce la size da aprire</returns>
        private double[] _getAntimarty()
        {

            double tsbuy = 0.0;
            double tssell = 0.0;

            // --> Faccio la somma
            var MyPositions = Positions.FindAll("", Symbol.Name);

            foreach (var position in MyPositions)
            {

                if (position.TradeType == TradeType.Buy)
                    tsbuy += position.Quantity;

                if (position.TradeType == TradeType.Sell)
                    tssell += position.Quantity;

            }

            // --> Restituisco l'array con le informazioni
            double[] result = 
            {
                Math.Round(tsbuy / Corff, 2),
                Math.Round(tssell / Corff, 2)
            };

            return result;

        }

        private void _drawBox( int index ) {


            // --> Formatto il testo del box
            string tmpSpread = String.Format("{0:0.0}", _getSpreadInformation());
            string tmpGP = String.Format("{0:0.00}", Symbol.UnrealizedGrossProfit);
            string tmpNT = String.Format("{0:0.00}", Symbol.UnrealizedNetProfit);

            string info = String.Format("{0} SPREAD\r\n{1}", SymbolName, tmpSpread);

            if (ShowGross)
                info += String.Format("\r\n\r\nGROSS PROFIT\r\n{0}", tmpGP);

            if (ShowNet)
                info += String.Format("\r\n\r\nNET PROFIT\r\n{0}", tmpNT);

            if (ShowLeva)
                info += String.Format("\r\n\r\nLEVERAGE\r\n1:{0}", Account.PreciseLeverage);

            if (ShowAntimarty)
            {

                double[] antM = _getAntimarty();

                info += String.Format("\r\n\r\nANTIMARTINGALA\r\nBuy : {0} / Sell : {1}", antM[0], antM[1]);

            }

            Chart.DrawStaticText("BoxInfo", info, VAlign, HAlign, Color.FromName(Boxcolor.ToString("G")));

        }

        private void _drawBanner(int index)
        {
            
            // --> Formatto il testo del box
            string tmpSpread = String.Format("{0:0.0}", _getSpreadInformation());
            string tmpGP = String.Format("{0:0.00}", Symbol.UnrealizedGrossProfit);
            string tmpNT = String.Format("{0:0.00}", Symbol.UnrealizedNetProfit);

            string info = String.Format("{0} / {1}", SymbolName, tmpSpread);

            if (ShowGross)
                info += String.Format(" / {0}", tmpGP);

            if (ShowNet)
                info += String.Format(" / {0}", tmpNT);

            if (ShowLeva)
                info += String.Format(" / 1:{0}", Account.PreciseLeverage);

            if (ShowAntimarty)
            {

                double[] antM = _getAntimarty();

                info += String.Format(" / Buy : {0} / Sell : {1}", antM[0], antM[1]);

            }

            Chart.DrawStaticText("BoxInfo", info, VAlign, HAlign, Color.FromName(Boxcolor.ToString("G")));

        }

        #endregion

    }

}