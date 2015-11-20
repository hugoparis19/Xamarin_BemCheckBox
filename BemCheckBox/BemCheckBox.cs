using System;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;

namespace BemCheckBox
{
    public class BemCheckBox : UIView
    {
        /** The object that acts as the delegate of the receiving check box.
* @discussion The delegate must adopt the \p BEMCheckBoxDelegate protocol. The delegate is not retained.
 */
        private readonly BemCheckBoxDelegate _delegate;
        /** This property allows you to retrieve and set (without animation) a value determining whether the BEMCheckBox object is On or Off.
  * Default to NO.
 */
        private bool _on;
        /// <summary>
        /// The width of the lines of the check mark and the box. Default to 2.0.
        /// </summary>
        private float _lineWidth;
        /// <summary>
        /// The duration in seconds of the animation when the check box switches from on and off. Default to 0.5.
        /// </summary>
        private float _animationDuration;
        /// <summary>
        /// BOOL to control if the box should be hidden or not. Defaults to NO.
        /// </summary>
        private bool _hideBox;

        /// <summary>
        /// The color of the inside of the box when it is On.
        /// </summary>
        private UIColor _fillColor;
        /// <summary>
        /// The color of the check mark when it is On.
        /// </summary>
        private UIColor _checkColor;
        /// <summary>
        /// The color of the box when the checkbox is Off.
        /// </summary>
        private UIColor _tintColor;
        /// <summary>
        /// The type of box.
        /// </summary>
        private BemBoxType _boxType;
        /// <summary>
        /// The animation type when the check mark gets set to On.
        /// @warning Some animations might not look as intended if the different colors of the control are not appropriatly configured.
        /// </summary>
        private BemAnimationType _onAnimationType;

        private UITapGestureRecognizer _handleTapCheckBoxRecognizer;

        ////
        private CAShapeLayer _onBoxLayer;
        private CAShapeLayer _offBoxLayer;
        private CAShapeLayer _checkMarkLayer;
        private BemAnimationManager _animationManager;
        private BemPathManager _pathManager;

        private MyCaAnimationDelegate _animationDelegate;


        private UIColor _ontintColor;


        /// 
        /// 

        public BemCheckBox(CGRect frame, BemCheckBoxDelegate checkBoxDelegate)
            : base(frame)
        {
            _delegate = checkBoxDelegate;
            CommonInit();
        }

        public BemCheckBox(NSCoder coder, BemCheckBoxDelegate checkBoxDelegate)
            : base(coder)
        {
            _delegate = checkBoxDelegate;
            CommonInit();
        }

        public bool On
        {
            get { return _on; }
            set { SetOn(value); }
        }

        private void CommonInit()
        {
            _on = false;
            _hideBox = false;
            SetStyle();
            InitPathManager();
            InitAnimationManager();
            _handleTapCheckBoxRecognizer = new UITapGestureRecognizer(HandleTapCheckBox);
            AddGestureRecognizer(_handleTapCheckBoxRecognizer);

            _animationDelegate = new MyCaAnimationDelegate(this);
        }



        private void SetStyle()
        {
            _fillColor = UIColor.FromRGB(47, 123, 255); // UIColor.Clear;
            _checkColor = UIColor.White;// new UIColor(0, 122/255, 255/255, 1);
            _tintColor = UIColor.FromRGB(171, 171, 171);//UIColor.LightGray;
            _ontintColor = UIColor.FromRGB(47, 123, 255);//UIColor.LightGray;
            _lineWidth = 2f;
            _boxType = BemBoxType.BemBoxTypeCircle;
            _animationDuration = 0.5f;
            _onAnimationType = BemAnimationType.BemAnimationTypeBounce;
            BackgroundColor = UIColor.Clear;
        }

        private void InitPathManager()
        {
            _pathManager = new BemPathManager();
            _pathManager.LineWidth = _lineWidth;
            _pathManager.BoxType = _boxType;
        }

        private void InitAnimationManager()
        {
            _animationManager = new BemAnimationManager(_animationDuration);
        }

        public override void LayoutSubviews()
        {
            _pathManager.Size = Frame.Size.Height;
            base.LayoutSubviews();
        }


        /** Forces a redraw of the entire check box.
         * The current value of On is kept.
         */

        public void Reload()
        {
            if (_offBoxLayer != null)
                _offBoxLayer.RemoveFromSuperLayer();
            _offBoxLayer = null;

            if (_onBoxLayer != null)
                _onBoxLayer.RemoveFromSuperLayer();
            _onBoxLayer = null;

            if (_checkMarkLayer != null)
                _checkMarkLayer.RemoveFromSuperLayer();
            _checkMarkLayer = null;

            SetNeedsDisplay();
            LayoutIfNeeded();
        }

        #region setters

        ///Set the state of the check box to On or Off, optionally animating the transition.*/
        public void SetOn(bool on, bool animated)
        {
            _on = on;
            DrawEntireCheckBox();
            if (_on)
            {
                if (animated)
                {
                    AddOnAnimation();
                }
            }
            else
            {
                if (animated)
                {
                    AddOffAnimation();
                }
                else
                {
                    if (_onBoxLayer != null)
                        _onBoxLayer.RemoveFromSuperLayer();
                    if (_checkMarkLayer != null)
                        _checkMarkLayer.RemoveFromSuperLayer();
                }
            }
        }

        public void SetOn(bool on)
        {
            SetOn(on, true);
        }



        #endregion

        private void HandleTapCheckBox(UITapGestureRecognizer gestureRecognizer)
        {
            SetOn(!_on, true);

            _delegate.DidTapCheckBox(_on);
        }


        public override void Draw(CGRect rect)
        {
            SetOn(_on, false); //base.Draw(rect);
        }


        private void DrawEntireCheckBox()
        {
            if (!_hideBox)
            {
                if (_offBoxLayer == null || _offBoxLayer.Path.BoundingBox.Size.Height == 0)
                {
                    DrawOffBox();
                }
                if (_on)
                {
                    DrawOnBox();
                }
            }
            if (_on)
            {
                DrawCheckMark();
            }
        }

        private void DrawOffBox()
        {
            if (_offBoxLayer != null)
                _offBoxLayer.RemoveFromSuperLayer();

            _offBoxLayer = new CAShapeLayer
            {
                Frame = Bounds,
                Path = _pathManager.PathForBox().CGPath,
                FillColor = UIColor.Clear.CGColor,
                StrokeColor = _tintColor.CGColor,
                LineWidth = _lineWidth,
                RasterizationScale = 2 * UIScreen.MainScreen.Scale,
                ShouldRasterize = true
            };
            Layer.AddSublayer(_offBoxLayer);
        }

        private void DrawOnBox()
        {
            if (_onBoxLayer != null)
                _onBoxLayer.RemoveFromSuperLayer();
            _onBoxLayer = new CAShapeLayer
            {
                Frame = Bounds,
                Path = _pathManager.PathForBox().CGPath,
                FillColor = _fillColor.CGColor,
                StrokeColor = _ontintColor.CGColor,
                LineWidth = _lineWidth,
                RasterizationScale = 2 * UIScreen.MainScreen.Scale,
                ShouldRasterize = true
            };
            Layer.AddSublayer(_onBoxLayer);
        }

        private void DrawCheckMark()
        {
            if (_checkMarkLayer != null)
                _checkMarkLayer.RemoveFromSuperLayer();
            _checkMarkLayer = new CAShapeLayer
            {
                Frame = Bounds,
                Path = _pathManager.PathForCheckMark().CGPath,
                FillColor = UIColor.Clear.CGColor,
                StrokeColor = _checkColor.CGColor,
                LineWidth = _lineWidth,
                LineCap = new NSString("kCALineCapRound"),
                LineJoin = new NSString("kCALineJoinRound"),
                RasterizationScale = 2 * UIScreen.MainScreen.Scale,
                ShouldRasterize = true
            };
            Layer.AddSublayer(_checkMarkLayer);
        }

        private void AddOnAnimation()
        {
            if (_checkMarkLayer == null)
                return;
            if (Math.Abs(_animationDuration) < 0.0001f)
                return;

            CABasicAnimation animation;
            CAKeyFrameAnimation wiggle;
            CABasicAnimation opacityAnimation;
            switch (_onAnimationType)
            {
                case BemAnimationType.BemAnimationTypeStroke:
                    animation = _animationManager.StrokeAnimationReverse(false);
                    _onBoxLayer.AddAnimation(animation, "strokeEnd");
                    animation.Delegate = _animationDelegate;
                    _checkMarkLayer.AddAnimation(animation, "strokeEnd");
                    break;
                case BemAnimationType.BemAnimationTypeFill:
                    wiggle = _animationManager.FillAnimationWithBounces(1, 0.18f, false);
                    opacityAnimation = _animationManager.OpacityAnimationReverse(false);
                    wiggle.Delegate = _animationDelegate;
                    _onBoxLayer.AddAnimation(wiggle, "transform");
                    _checkMarkLayer.AddAnimation(opacityAnimation, "opacity");
                    break;
                case BemAnimationType.BemAnimationTypeBounce:
                    float amplitude = _boxType == BemBoxType.BemBoxTypeSquare ? 0.20f : 0.35f;
                    wiggle = _animationManager.FillAnimationWithBounces(1, amplitude, false);
                    wiggle.Delegate = _animationDelegate;
                    opacityAnimation = _animationManager.OpacityAnimationReverse(false);
                    opacityAnimation.Duration = _animationDuration / 1.4f;
                    _onBoxLayer.AddAnimation(opacityAnimation, "opacity");
                    _checkMarkLayer.AddAnimation(wiggle, "transform");
                    break;
                default:
                    animation = _animationManager.StrokeAnimationReverse(false);
                    _onBoxLayer.AddAnimation(animation, "opacity");
                    animation.Delegate = _animationDelegate;
                    _checkMarkLayer.AddAnimation(animation, "opacity");
                    break;
            }
        }

        private void AddOffAnimation()
        {
            if (_checkMarkLayer == null)
                return;
            if (Math.Abs(_animationDuration) < 0.0001f)
            {
                _onBoxLayer.RemoveFromSuperLayer();
                _checkMarkLayer.RemoveFromSuperLayer();
                return;
            }
            CABasicAnimation animation;
            switch (_onAnimationType)
            {
                case BemAnimationType.BemAnimationTypeStroke:
                    animation = _animationManager.StrokeAnimationReverse(true);
                    _onBoxLayer.AddAnimation(animation, "strokeEnd");
                    animation.Delegate = _animationDelegate;
                    _checkMarkLayer.AddAnimation(animation, "strokeEnd");
                    break;
                case BemAnimationType.BemAnimationTypeFill:
                    CAKeyFrameAnimation wiggle = _animationManager.FillAnimationWithBounces(1, 0.18f, true);
                    wiggle.Duration = _animationDuration;
                    wiggle.Delegate = _animationDelegate;
                    _onBoxLayer.AddAnimation(wiggle, "transform");
                    _checkMarkLayer.AddAnimation(_animationManager.OpacityAnimationReverse(true), "opacity");
                    break;
                case BemAnimationType.BemAnimationTypeBounce:
                    float amplitude = _boxType == BemBoxType.BemBoxTypeSquare ? 0.20f : 0.35f;
                    wiggle = _animationManager.FillAnimationWithBounces(1, amplitude, true);
                    wiggle.Duration = _animationDuration / 1.1;

                    var opacityAnimation = _animationManager.OpacityAnimationReverse(true);
                    opacityAnimation.Delegate = _animationDelegate;

                    _onBoxLayer.AddAnimation(opacityAnimation, "opacity");
                    _checkMarkLayer.AddAnimation(wiggle, "transform");
                    break;
                default:
                    animation = _animationManager.StrokeAnimationReverse(true);
                    _onBoxLayer.AddAnimation(animation, "opacity");
                    animation.Delegate = _animationDelegate;
                    _checkMarkLayer.AddAnimation(animation, "opacity");
                    break;
            }
        }

        /** Sent to the delegate every time the check box gets tapped.
 * @discussion This method gets triggered after the properties are updated (on), but before the animations, if any, are completed.
 * @seealso animationDidStopForCheckBox:
 * @param checkBox: The BEMCheckBox instance that has been tapped.
 */
        public void DidTapCheckBox(BemCheckBox checkBox)
        {
        }


        /** Sent to the delegate every time the check box finishes being animated.
         * @discussion This method gets triggered after the properties are updated (on), and after the animations are completed. It won't be triggered if no animations are started.
         * @seealso didTapCheckBox:
         * @param checkBox: The BEMCheckBox instance that was animated.
         */
        public void AnimationDidStopForCheckBox(BemCheckBox checkBox)
        {
        }

        private class MyCaAnimationDelegate : CAAnimationDelegate
        {
            private readonly BemCheckBox _bemCheckBox;

            public MyCaAnimationDelegate(BemCheckBox bemCheckBox)
            {
                _bemCheckBox = bemCheckBox;
            }

            public override void AnimationStopped(CAAnimation anim, bool finished)
            {
                if (finished)
                {
                    if (!_bemCheckBox.On)
                    {
                        _bemCheckBox._onBoxLayer.RemoveFromSuperLayer();
                        _bemCheckBox._checkMarkLayer.RemoveFromSuperLayer();
                    }

                }
            }
        }
    }
}
