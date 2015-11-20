using System;
using CoreGraphics;
using UIKit;

namespace BemCheckBox
{
    internal class BemPathManager
    {
        /** The paths are assumed to be created in squares. 
         * This is the size of width, or height, of the paths that will be created.
         */
        public nfloat Size;

        /** The width of the lines on the created paths.
         */
        public float LineWidth;

        /** The type of box.
         * Depending on the box type, paths may be created differently
         * @see BemBoxType
         */
        public BemBoxType BoxType;


        /** Returns a UIBezierPath object for the box of the checkbox
         * @returns The path of the box.
         */

        public UIBezierPath PathForBox()
        {
            UIBezierPath path;
            switch (BoxType)
            {
                case BemBoxType.BemBoxTypeSquare:
                    path = UIBezierPath.FromRoundedRect(new CGRect(0, 0, Size, Size), 3);
                    //[path applyTransform:CGAffineTransformRotate(CGAffineTransformIdentity, M_PI / 2)];
                    path.ApplyTransform(CGAffineTransform.Rotate(CGAffineTransform.MakeIdentity(), new nfloat(Math.PI / 2)));
                    path.ApplyTransform(CGAffineTransform.MakeTranslation(Size, 0));
                    break;

                default:
                {
                    nfloat radius = Size / 2;
                    //path = [UIBezierPath bezierPathWithArcCenter:CGPointMake(self.size / 2, self.size / 2)
                    //                                      radius: radius
                    //                                  startAngle: - M_PI / 4
                    //                                    endAngle:  2 * M_PI - M_PI / 4
                    //                                   clockwise:YES];

                    path = UIBezierPath.FromArc(new CGPoint(Size / 2, Size / 2), radius, new nfloat(-(Math.PI / 4)), new nfloat((2 * Math.PI - Math.PI / 4)), true);
                }
                    break;
            }
            return path;
        }

        /** Returns a UIBezierPath object for the checkmark of the checkbox
         * @returns The path of the checkmark.
         */

        public UIBezierPath PathForCheckMark()
        {
            UIBezierPath checkMarkPath = UIBezierPath.Create();

            checkMarkPath.MoveTo(new CGPoint(Size / 3.1578, Size / 2));
            checkMarkPath.AddLineTo(new CGPoint(Size / 2.0618, Size / 1.57894));
            checkMarkPath.AddLineTo(new CGPoint(Size / 1.3953, Size / 2.7272));

            if (BoxType == BemBoxType.BemBoxTypeSquare)
            {
                checkMarkPath.ApplyTransform(CGAffineTransform.MakeScale(new nfloat(1.5), new nfloat(1.5)));
                checkMarkPath.ApplyTransform(CGAffineTransform.MakeTranslation(-Size / 4, -Size / 4));
            }
            return checkMarkPath;
        }


        /** Returns a UIBezierPath object for an extra long checkmark which is in contact with the box.
         * @returns The path of the checkmark.
         */

        public UIBezierPath PathForLongCheckMark()
        {
            UIBezierPath checkMarkPath = UIBezierPath.Create();

            checkMarkPath.MoveTo(new CGPoint(Size / 3.1578, Size / 2));
            checkMarkPath.AddLineTo(new CGPoint(Size / 2.0618, Size / 1.57894));


            if (BoxType == BemBoxType.BemBoxTypeSquare)
            {
                // If we use a square box, the check mark should be a little bit bigger
                checkMarkPath.AddLineTo(new CGPoint(new CGPoint(Size / 1.2053, Size / 4.5272)));
                checkMarkPath.ApplyTransform(CGAffineTransform.MakeScale(new nfloat(1.5), new nfloat(1.5)));
                checkMarkPath.ApplyTransform(CGAffineTransform.MakeTranslation(-Size / 4, -Size / 4));
            }
            else
            {
                checkMarkPath.AddLineTo(new CGPoint(new CGPoint(Size / 1.1553, Size / 5.9272)));
            }

            return checkMarkPath;
        }

        /** Returns a UIBezierPath object for the flat checkmark of the checkbox
         * @see BEMAnimationTypeFlat
         * @returns The path of the flat checkmark.
         */

        public UIBezierPath PathForFlatCheckMark()
        {
            UIBezierPath flatCheckMarkPath = UIBezierPath.Create();
            flatCheckMarkPath.MoveTo(new CGPoint(Size / 4, Size / 2));
            flatCheckMarkPath.AddLineTo(new CGPoint(new CGPoint(Size / 2, Size / 2)));
            flatCheckMarkPath.AddLineTo(new CGPoint(new CGPoint(Size / 1.2, Size / 2)));
            return flatCheckMarkPath;
        }
    }
}