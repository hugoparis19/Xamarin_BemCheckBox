namespace BemCheckBox
{
    internal enum BemAnimationType
    {
        /// <summary>
        /// Animates the box and the check as if they were drawn.
        /// Should be used with a clear colored fillColor property.
        /// </summary>
        BemAnimationTypeStroke,

        /// <summary>
        /// When tapped, the checkbox is filled from its center.
        /// Should be used with a clear colored fillColor property.
        /// </summary>
        BemAnimationTypeFill,

        /// <summary>
        ///  Animates the check mark with a bouncy effect aka the 2 life crew fx.
        /// </summary>  
        BemAnimationTypeBounce

        /** Morphs the checkmark from a line.
        // * Should be used with a colored onFillColor property.
        // */
        //BemAnimationTypeFlat,

        /** Animates the box and check as if they were drawn in one continuous line.
        // * Should be used with a clear colored onFillColor property.
        // */
        //BemAnimationTypeOneStroke,

        /** When tapped, the checkbox is fading in or out (opacity).
        // */
        //BemAnimationTypeFade
    }
}