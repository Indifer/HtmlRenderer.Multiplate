// "Therefore those skilled at the unorthodox
// are infinite as heaven and earth,
// inexhaustible as the great rivers.
// When they come to an end,
// they begin again,
// like the days and months;
// they die and are reborn,
// like the four seasons."
// 
// - Sun Tsu,
// "The Art of War"

using System.Collections.Generic;
using HtmlRenderer.Dom;
using HtmlRenderer.Entities;
using Android.Graphics;

namespace HtmlRenderer.Utils
{
    /// <summary>
    /// Provides some drawing functionallity
    /// </summary>
    internal static class RenderUtils
    {
        #region Fields and Consts

        ///// <summary>
        ///// image used to draw loading image icon
        ///// </summary>
        //private static Image _loadImage;

        ///// <summary>
        ///// image used to draw error image icon
        ///// </summary>
        //private static Image _errorImage;

        #endregion


        /// <summary>
        /// Check if the given color is visible if painted (has alpha and color values)
        /// </summary>
        /// <param name="color">the color to check</param>
        /// <returns>true - visible, false - not visible</returns>
        public static bool IsColorVisible(Color color)
        {
            return color.A > 0;
        }

        /// <summary>
        /// Clip the region the graphics will draw on by the overflow style of the containing block.<br/>
        /// Recursively travel up the tree to find containing block that has overflow style set to hidden. if not
        /// block found there will be no clipping and null will be returned.
        /// </summary>
        /// <param name="g">the graphics to clip</param>
        /// <param name="box">the box that is rendered to get containing blocks</param>
        /// <returns>the prev region if clipped, otherwise null</returns>
        public static RectF ClipGraphicsByOverflow(IGraphics g, CssBox box)
        {
            var containingBlock = box.ContainingBlock;
            while (true)
            {
                if (containingBlock.Overflow == CssConstants.Hidden)
                {
                    var prevClip = g.GetClip();
                    var rect = box.ContainingBlock.ClientRectangle;
                    rect.X -= 2; // atodo: find better way to fix it
                    rect.Width += 2;
                    rect.Offset(box.HtmlContainer.ScrollOffset);
                    rect.Intersect(prevClip);
                    g.SetClip(rect);
                    return prevClip;
                }
                else
                {
                    var cBlock = containingBlock.ContainingBlock;
                    if (cBlock == containingBlock)
                        return RectangleF.Empty;
                    containingBlock = cBlock;
                }
            }
        }

        /// <summary>
        /// Return original clip region to the graphics object.<br/>
        /// Should be used with <see cref="ClipGraphicsByOverflow"/> return value to return clip back to original.
        /// </summary>
        /// <param name="g">the graphics to clip</param>
        /// <param name="prevClip">the region to set on the graphics (null - ignore)</param>
        public static void ReturnClip(IGraphics g, RectF prevClip)
        {
            if (prevClip != null)
            {
                g.SetClip(prevClip);
            }
        }
        
        /// <summary>
        /// Creates a rounded rectangle using the specified corner radius
        /// </summary>
        /// <param name="rect">Rectangle to round</param>
        /// <param name="nwRadius">Radius of the north east corner</param>
        /// <param name="neRadius">Radius of the north west corner</param>
        /// <param name="seRadius">Radius of the south east corner</param>
        /// <param name="swRadius">Radius of the south west corner</param>
        /// <returns>GraphicsPath with the lines of the rounded rectangle ready to be painted</returns>
        public static Path GetRoundRect(RectF rect, float nwRadius, float neRadius, float seRadius, float swRadius)
        {
            //  NW-----NE
            //  |       |
            //  |       |
            //  SW-----SE

            var path = new Path();

            nwRadius *= 2;
            neRadius *= 2;
            seRadius *= 2;
            swRadius *= 2;

            //NW ---- NE
            path.SetLastPoint(rect.Left + nwRadius, rect.Top);
            path.LineTo(rect.Right - neRadius, rect.Top);

            //NE Arc
            if( neRadius > 0f )
            {
                path.AddArc(
                    new RectF(rect.Right - neRadius, rect.Top, rect.Right, rect.Top + neRadius),
                    -90, 90);
            }

            // NE
            //  |
            // SE
            path.SetLastPoint(rect.Right, rect.Top + neRadius);
            path.LineTo(rect.Right, rect.Bottom - seRadius);

            //SE Arc
            if( seRadius > 0f )
            {
                path.AddArc(
                    new RectF(rect.Right - seRadius, rect.Bottom - seRadius, rect.Right, rect.Bottom),
                    0, 90);
            }

            // SW --- SE
            path.SetLastPoint(rect.Right - seRadius, rect.Bottom);
            path.LineTo(rect.Left + swRadius, rect.Bottom);

            //SW Arc
            if( swRadius > 0f )
            {
                path.AddArc(
                    new RectF(rect.Left, rect.Bottom - swRadius, rect.Left + swRadius, rect.Bottom),
                    90, 90);
            }

            // NW
            // |
            // SW
            path.SetLastPoint(rect.Left, rect.Bottom - swRadius);
            path.LineTo(rect.Left, rect.Top + nwRadius);

            //NW Arc
            if( nwRadius > 0f )
            {
                path.AddArc(
                    new RectF(rect.Left, rect.Top, rect.Left + nwRadius, rect.Top + nwRadius),
                    180, 90);
            }

            path.Close();            

            return path;
        }

    }
}