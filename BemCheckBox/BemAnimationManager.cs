using System;
using System.Collections.Generic;
using CoreAnimation;
using Foundation;
using UIKit;

namespace BemCheckBox
{
    internal class BemAnimationManager
    {
        public float AnimationDuration;


        /** Designated initializer.
         * @param animationDuration The duration of the animations created with the BEMAnimationManager object.
         * @return Returns the a fully initialized BEMAnimationManager object.
         */

        public BemAnimationManager(float animationDurationV)
        {
            AnimationDuration = animationDurationV;
        }


        /** Returns a CABasicAnimation which the stroke.
         * @param reverse The direction of the animation. Set to YES if the animation should go from opacity 0 to 1, or NO for the opposite.
         * @return Returns the CABasicAnimation object.
         */

        public CABasicAnimation StrokeAnimationReverse(bool reverse)
        {
            CABasicAnimation animation = CABasicAnimation.FromKeyPath("strokeEnd");
            if (reverse)
            {
                animation.From = new NSNumber(1);
                animation.To = new NSNumber(0);
            }
            else
            {
                animation.From = new NSNumber(0);
                animation.To = new NSNumber(1);
            }
            animation.Duration = AnimationDuration;
            animation.RemovedOnCompletion = false;
            animation.FillMode = CAFillMode.Forwards;
            animation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
            return animation;
        }


        /** Returns a CABasicAnimation which animates the opacity.
         * @param reverse The direction of the animation. Set to YES if the animation should go from opacity 0 to 1, or NO for the opposite.
         * @return Returns the CABasicAnimation object.
         */

        public CABasicAnimation OpacityAnimationReverse(bool reverse)
        {
            CABasicAnimation animation = CABasicAnimation.FromKeyPath("opacity");
            if (reverse)
            {
                animation.From = new NSNumber(1);
                animation.To = new NSNumber(0);
            }
            else
            {
                animation.From = new NSNumber(0);
                animation.To = new NSNumber(1);
            }
            animation.Duration = AnimationDuration;
            animation.RemovedOnCompletion = false;
            animation.FillMode = CAFillMode.Forwards;
            animation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
            return animation;
        }

        /** Returns a CABasicAnimation which animates between two paths.
         * @param fromPath The path to transform (morph) from.
         * @param toPath The path to transform (morph) to.
         * @return Returns the CABasicAnimation object.
         */

        public CABasicAnimation MorphAnimationFromPath(UIBezierPath fromPath, UIBezierPath toPath)
        {
            CABasicAnimation animation = CABasicAnimation.FromKeyPath("path");
            animation.Duration = AnimationDuration;
            animation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);
            animation.SetFrom(fromPath.CGPath);
            animation.SetTo(toPath.CGPath);
            return animation;
        }

        /** Animation engine to create a fill animation.
         * @param bounces The number of bounces for the animation.
         * @param amplitue How far does the animation bounce.
         * @param reserve Flag to track if the animation should fill or empty the layer.
         * @return Returns the CAKeyframeAnimation object.
         */

        public CAKeyFrameAnimation FillAnimationWithBounces(int bounces, float amplitude, bool reverse)
        {
            List<NSObject> values = new List<NSObject>();
            List<NSNumber> keyTimes = new List<NSNumber>();

            if (reverse)
            {
                values.Add(NSValue.FromCATransform3D(CATransform3D.MakeScale(1, 1, 1)));
            }
            else
            {
                values.Add(NSValue.FromCATransform3D(CATransform3D.MakeScale(0, 0, 0)));
            }

            keyTimes.Add(new NSNumber(0));

            for (int i = 1; i <= bounces; i++)
            {
                var scale = (i % 2) > 0 ? (1 + amplitude / i) : (1 - amplitude / i);
                var time = (float)(i * (1.0 / (bounces + 1)));
                values.Add(NSValue.FromCATransform3D(CATransform3D.MakeScale(scale, scale, scale)));
                keyTimes.Add(new NSNumber(time));
            }

            if (reverse)
            {
                values.Add(NSValue.FromCATransform3D(CATransform3D.MakeScale(new nfloat(0.0001), new nfloat(0.0001), new nfloat(0.0001))));
            }
            else
            {
                values.Add(NSValue.FromCATransform3D(CATransform3D.MakeScale(1, 1, 1)));
            }

            keyTimes.Add(new NSNumber(1));

            CAKeyFrameAnimation animation = CAKeyFrameAnimation.FromKeyPath("transform");
            animation.Values = values.ToArray();
            animation.KeyTimes = keyTimes.ToArray();
            animation.RemovedOnCompletion = false;
            animation.FillMode = CAFillMode.Forwards;
            animation.Duration = AnimationDuration;
            animation.TimingFunction = CAMediaTimingFunction.FromName(CAMediaTimingFunction.EaseInEaseOut);

            return animation;
        }


    }
}