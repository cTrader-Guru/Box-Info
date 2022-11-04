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
using cAlgo.API.Internals;

using cTrader.Guru.Helper;

namespace cAlgo.Indicators
{

    [Indicator(IsOverlay = true, AccessRights = AccessRights.None)]
    public class BoxInfo : Indicator
    {

        #region Enums

        public enum MyBoxType
        {

            Box,
            Banner

        }

        #endregion

        #region Identity

        public const string NAME = "Box Info";

        public const string VERSION = "1.1.5";

        #endregion

        #region Params

        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://www.google.com/search?q=ctrader+guru+box+info")]
        public string ProductInfo { get; set; }

        [Parameter("Label (empty = all)", Group = "Params", DefaultValue = "")]
        public string LabelObserve { get; set; }

        [Parameter("Antimartingala K%", Group = "Params", DefaultValue = 2.15)]
        public double Corff { get; set; }

        [Parameter("Panel type ?", Group = "Options", DefaultValue = MyBoxType.Banner)]
        public MyBoxType PanelType { get; set; }

        [Parameter("Show Gross Profit ?", Group = "Options", DefaultValue = true)]
        public bool ShowGross { get; set; }

        [Parameter("Show Net Profit ?", Group = "Options", DefaultValue = true)]
        public bool ShowNet { get; set; }

        [Parameter("Show Leverage ?", Group = "Options", DefaultValue = true)]
        public bool ShowLeva { get; set; }

        [Parameter("Show Antimartingala ?", Group = "Options", DefaultValue = true)]
        public bool ShowAntimarty { get; set; }

        [Parameter("Show Equity %", Group = "Options", DefaultValue = true)]
        public bool ShowEquityPercentage { get; set; }

        [Parameter("Color", Group = "Styles", DefaultValue = ColorFromEnum.ColorNameEnum.Coral)]
        public ColorFromEnum.ColorNameEnum Boxcolor { get; set; }

        [Parameter("Vertical Position", Group = "Styles", DefaultValue = VerticalAlignment.Top)]
        public VerticalAlignment VAlign { get; set; }

        [Parameter("Horizontal Position", Group = "Styles", DefaultValue = HorizontalAlignment.Left)]
        public HorizontalAlignment HAlign { get; set; }

        #endregion

        #region Indicator Events

        protected override void Initialize()
        {

            Print("{0} : {1}", NAME, VERSION);

            LabelObserve = LabelObserve.Trim();

        }

        public override void Calculate(int index)
        {

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

        }

        #endregion

        #region Private Methods

        private double _getSpreadInformation()
        {

            return Math.Round(Symbol.Spread / Symbol.PipSize, 2);

        }

        private double[] _getBoxInfo()
        {

            double tsbuy = 0.0;
            double tssell = 0.0;

            double tsGross = 0.0;
            double tsNet = 0.0;

            foreach (var position in Positions)
            {

                if (position.SymbolName != SymbolName || (LabelObserve.Length > 0 && position.Label != LabelObserve))
                    continue;

                if (position.TradeType == TradeType.Buy)
                    tsbuy += position.Quantity;

                if (position.TradeType == TradeType.Sell)
                    tssell += position.Quantity;

                tsGross += position.GrossProfit;
                tsNet += position.NetProfit;

            }

            double[] result = 
            {
                Math.Round(tsbuy / Corff, 2),
                Math.Round(tssell / Corff, 2),
                tsGross,
                tsNet
            };

            return result;

        }

        private void _drawBox(int index)
        {

            double[] boxInfo = _getBoxInfo();
            string whatLabel = LabelObserve.Length > 0 ? LabelObserve : "All";

            string tmpSpread = String.Format("{0:0.0}", _getSpreadInformation());

            string info = String.Format("{0} ( {1}; {2} ) SPREAD\r\n{3}", SymbolName, TimeFrame, whatLabel, tmpSpread);

            if (ShowGross)
                info += String.Format("\r\n\r\nGROSS PROFIT\r\n{0:0.00}", boxInfo[2]);

            if (ShowNet)
                info += String.Format("\r\n\r\nNET PROFIT\r\n{0:0.00}", boxInfo[3]);

            if (ShowLeva)
                info += String.Format("\r\n\r\nLEVERAGE\r\n1:{0}", Account.PreciseLeverage);

            if (ShowAntimarty)
                info += String.Format("\r\n\r\nANTIMARTINGALA\r\nBuy : {0:0.00} / Sell : {1:0.00}", boxInfo[0], boxInfo[1]);

            double MyEquity = Account.Balance + boxInfo[3];

            if (ShowEquityPercentage)
                info += String.Format("\r\n\r\nEquity %\r\n{0:0.00}%", ((MyEquity - Account.Balance) * 100) / MyEquity);

            Chart.DrawStaticText("BoxInfo", info, VAlign, HAlign, ColorFromEnum.GetColor(Boxcolor));

        }

        private void _drawBanner(int index)
        {

            double[] boxInfo = _getBoxInfo();
            string whatLabel = LabelObserve.Length > 0 ? LabelObserve : "All";

            string tmpSpread = String.Format("{0:0.0}", _getSpreadInformation());

            string info = String.Format("{0} ( {1}; {2} ) / {3}", SymbolName, TimeFrame, whatLabel, tmpSpread);

            if (ShowGross)
                info += String.Format(" / {0:0.00}", boxInfo[2]);

            if (ShowNet)
                info += String.Format(" / {0:0.00}", boxInfo[3]);

            if (ShowLeva)
                info += String.Format(" / 1:{0}", Account.PreciseLeverage);

            if (ShowAntimarty)
                info += String.Format(" / Buy : {0:0.00} / Sell : {1:0.00}", boxInfo[0], boxInfo[1]);

            double MyEquity = Account.Balance + boxInfo[3];

            if (ShowEquityPercentage)
                info += String.Format(" / EQ {0:0.00}%", ((MyEquity - Account.Balance) * 100) / MyEquity);

            Chart.DrawStaticText("BoxInfo", info, VAlign, HAlign, ColorFromEnum.GetColor(Boxcolor));

        }

        #endregion

    }

}
