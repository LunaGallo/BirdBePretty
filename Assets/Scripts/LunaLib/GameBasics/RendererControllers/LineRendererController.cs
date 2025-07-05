using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public abstract class LineRendererController : RendererController<LineRenderer> {

        protected class ColorDef {
            public bool isGradient;
            public Gradient gradient;
            public Color start, end;
            public ColorDef(Gradient gradient) {
                isGradient = true;
                this.gradient = gradient;
            }
            public ColorDef(Color start, Color end) {
                isGradient = false;
                this.start = start;
                this.end = end;
            }
        }
        protected virtual ColorDef ColorDefinition => null;

        protected class WidthDef {
            public bool isCurve;
            public AnimationCurve curve;
            public float multiplier;
            public float start, end;
            public WidthDef(AnimationCurve curve, float multiplier) {
                isCurve = false;
                this.curve = curve;
                this.multiplier = multiplier;
            }
            public WidthDef(float start, float end) {
                isCurve = false;
                this.start = start;
                this.end = end;
            }
        }
        protected virtual WidthDef WidthDefinition => null;

        protected virtual List<Vector3> Positions => null;

        protected override void Apply() {
            List<Vector3> posList = Positions;
            if (posList == null || posList.Count <= 0) {
                rendererTarget.positionCount = 0;
            }
            else {
                rendererTarget.positionCount = posList.Count;
                rendererTarget.SetPositions(posList.ToArray());
            }
            ColorDef colorDef = ColorDefinition;
            if (colorDef != null) {
                if (colorDef.isGradient) {
                    rendererTarget.colorGradient = colorDef.gradient;
                }
                else {
                    rendererTarget.startColor = colorDef.start;
                    rendererTarget.endColor = colorDef.end;
                }
            }
            WidthDef widthDef = WidthDefinition;
            if (widthDef != null) {
                if (widthDef.isCurve) {
                    rendererTarget.widthCurve = widthDef.curve;
                    rendererTarget.widthMultiplier = widthDef.multiplier;
                }
                else {
                    rendererTarget.startWidth = widthDef.start;
                    rendererTarget.endWidth = widthDef.end;
                }
            }
        }

    }
}