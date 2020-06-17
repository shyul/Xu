/// ***************************************************************************
/// Pacmio Research Enivironment
/// Copyright 2001-2008, 2014-2020 Xu Li - me@xuli.us
/// 
/// Technical Analysis Chart UI
/// 
/// ***************************************************************************

namespace Xu.Chart
{
    public enum CandleStickType : int
    {
        /// <summary>
        /// Doji form when the open and close of a security are virtually equal. The length of the upper and lower shadows can vary, and the resulting candlestick looks like either a cross, inverted cross or plus sign. Doji convey a sense of indecision or tug-of-war between buyers and sellers. Prices move above and below the opening level during the session, but close at or near the opening level.
        /// </summary>
        Doji,

        /// <summary>
        /// A Doji where the open and close price are at the high of the day. Like other Doji days, this one normally appears at market turning points.
        /// </summary>
        DragonflyDoji,

        /// <summary>
        /// A doji line that develops when the Doji is at, or very near, the low of the day.
        /// </summary>
        GravestoneDoji,

        /// <summary>
        /// This candlestick has long upper and lower shadows with the Doji in the middle of the day's trading range, clearly reflecting the indecision of traders.
        /// </summary>
        LongLeggedDoji,

        /// <summary>
        /// A candlestick with no shadow extending from the body at either the open, the close or at both. The name means close-cropped or close-cut in Japanese, though other interpretations refer to it as Bald or Shaven Head.
        /// </summary>
        Marubozu,

        /// <summary>
        /// Candlestick lines that have small bodies with upper and lower shadows that exceed the length of the body. Spinning tops signal indecision.
        /// </summary>
        SpinningTop,

        /// <summary>
        /// A large price move from open to close, where the length of the candle body is long.
        /// </summary>
        LongBody,

        /// <summary>
        /// A short day represents a small price move from open to close, where the length of the candle body is short.
        /// </summary>
        ShortBody,

        /// <summary>
        /// Candlesticks with a long upper shadow and short lower shadow indicate that buyers dominated during the first part of the session, bidding prices higher. Conversely, candlesticks with long lower shadows and short upper shadows indicate that sellers dominated during the first part of the session, driving prices lower.
        /// </summary>
        LongTopShadows,
        LongButtomShadows,

        /// <summary>
        /// Hammer candlesticks form when a security moves significantly lower after the open, but rallies to close well above the intraday low. The resulting candlestick looks like a square lollipop with a long stick. If this candlestick forms during a decline, then it is called a Hammer.
        /// </summary>
        Hammer,

        /// <summary>
        /// Hanging Man candlesticks form when a security moves significantly lower after the open, but rallies to close well above the intraday low. The resulting candlestick looks like a square lollipop with a long stick. If this candlestick forms during an advance, then it is called a Hanging Man.
        /// </summary>
        HangingMan,


        /// <summary>
        /// A rare reversal pattern characterized by a gap followed by a Doji, which is then followed by another gap in the opposite direction. The shadows on the Doji must completely gap below or above the shadows of the first and third day.
        /// </summary>
        MorningAbandonedDoji,
        EveningAbandonedDoji,


        /// <summary>
        /// A bullish two-day reversal pattern. The first day, in a downtrend, is a long black day. The next day opens at a new low, then closes above the midpoint of the body of the first day.
        /// </summary>
        PiercingLine,

        /// <summary>
        /// A bearish reversal pattern that continues the uptrend with a long white body. The next day opens at a new high, then closes below the midpoint of the body of the first day.
        /// </summary>
        DarkCloudCover,

        /// <summary>
        /// A continuation pattern with a long, black body followed by another black body that has gapped below the first one. The third day is white and opens within the body of the second day, then closes in the gap between the first two days, but does not close the gap.
        /// </summary>
        DownsideTasukiGap,

        /// <summary>
        /// A three-day bearish pattern that only happens in an uptrend. The first day is a long white body followed by a gapped open with the small black body remaining gapped above the first day. The third day is also a black day whose body is larger than the second day and engulfs it. The close of the last day is still above the first long white day.
        /// </summary>
        UpsideTasukiGap,

        /// <summary>
        /// A reversal pattern that can be bearish or bullish, depending upon whether it appears at the end of an uptrend (bearish engulfing pattern) or a downtrend (bullish engulfing pattern). The first day is characterized by a small body, followed by a day whose body completely engulfs the previous day's body and closes in the opposite direction of the trend. This pattern is similar to the outside reversal chart pattern, but does not require the entire range (high and low) to be engulfed, just the open and close.
        /// </summary>
        BullishEngulfing,
        BearishEngulfing,

        /// <summary>
        /// A two-day pattern that has a small body day completely contained within the range of the previous body, and is the opposite color.
        /// </summary>
        Harami,

        /// <summary>
        /// A two-day pattern similar to the Harami. The difference is that the last day is a Doji.
        /// </summary>
        HaramiCross,

        /// <summary>
        /// A three-day bearish reversal pattern similar to the Evening Star. The uptrend continues with a large white body. The next day opens higher, trades in a small range, then closes at its open (Doji). The next day closes below the midpoint of the body of the first day.
        /// </summary>
        EveningDojiStar,

        /// <summary>
        /// A three-day bullish reversal pattern that is very similar to the Morning Star. The first day is in a downtrend with a long black body. The next day opens lower with a Doji that has a small trading range. The last day closes above the midpoint of the first day.
        /// </summary>
        MorningDojiStar,

        /// <summary>
        /// A bearish reversal pattern that continues an uptrend with a long white body day followed by a gapped up small body day, then a down close with the close below the midpoint of the first day.
        /// </summary>
        EveningStar,

        /// <summary>
        /// A three-day bullish reversal pattern consisting of three candlesticks - a long-bodied black candle extending the current downtrend, a short middle candle that gapped down on the open, and a long-bodied white candle that gapped up on the open and closed above the midpoint of the body of the first day.
        /// </summary>
        MorningStar,

        /// <summary>
        /// A bullish continuation pattern in which a long white body is followed by three small body days, each fully contained within the range of the high and low of the first day. The fifth day closes at a new high.
        /// </summary>
        RisingThreeMethods,

        /// <summary>
        /// A bearish continuation pattern. A long black body is followed by three small body days, each fully contained within the range of the high and low of the first day. The fifth day closes at a new low.
        /// </summary>
        FallingThreeMethods,

        /// <summary>
        /// A one-day bullish reversal pattern. In a downtrend, the open is lower, then it trades higher, but closes near its open, therefore looking like an inverted lollipop.
        /// </summary>
        InvertedHammer,

        /// <summary>
        /// A single day pattern that can appear in an uptrend. It opens higher, trades much higher, then closes near its open. It looks just like the Inverted Hammer except that it is bearish.
        /// </summary>
        ShootingStar,



        /// <summary>
        /// A candlestick that gaps away from the previous candlestick is said to be in star position. Depending on the previous candlestick, the star position candlestick gaps up or down and appears isolated from previous price action.
        /// </summary>
        Star,

        /// <summary>
        /// A bullish reversal pattern with two black bodies surrounding a white body. The closing prices of the two black bodies must be equal. A support price is apparent and the opportunity for prices to reverse is quite good.
        /// </summary>
        StickSandwich,

        /// <summary>
        /// A bearish reversal pattern consisting of three consecutive long black bodies where each day closes at or near its low and opens within the body of the previous day.
        /// </summary>
        ThreeBlackCrows,

        /// <summary>
        /// A bullish reversal pattern consisting of three consecutive long white bodies. Each should open within the previous body and the close should be near the high of the day.
        /// </summary>
        ThreeWhiteSoldiers,

        /// <summary>
        /// A three-day bearish pattern that only happens in an uptrend. The first day is a long white body followed by a gapped open with the small black body remaining gapped above the first day. The third day is also a black day whose body is larger than the second day and engulfs it. The close of the last day is still above the first long white day.
        /// </summary>
        UpsideGapTwoCrows,
    }
}
