using System;
using UnityEngine;

namespace LunaLib {

    [Serializable]
    public abstract class SpringValue<T> {

        public T targetValue;
        public T currentValue;
        public T velocity;

        public float tightness;
        public float damping;
        public float mass;

        public T CurrentSpringForce => SubtractValues(InvertValue(MultiplyValueBy(SubtractValues(currentValue, targetValue), tightness)), MultiplyValueBy(velocity, damping));

        public void Accelerate(T acceleration, float duration) => velocity = AddValues(velocity, MultiplyValueBy(acceleration, duration));
        public void ApplyForce(T force, float duration) => Accelerate(DivideValueBy(force, mass), duration);

        public void Update(float deltaTime) {
            ApplyForce(CurrentSpringForce, deltaTime);
            currentValue = AddValues(currentValue, MultiplyValueBy(velocity, deltaTime));
        }

        public abstract T AddValues(T a, T b);
        public abstract T SubtractValues(T a, T b);
        public abstract T InvertValue(T v);
        public abstract T MultiplyValueBy(T v, float number);
        public abstract T DivideValueBy(T v, float number);

    }

    [Serializable]
    public class SpringFloat : SpringValue<float> {
        public override float AddValues(float a, float b) => a+b;
        public override float SubtractValues(float a, float b) => a - b;
        public override float InvertValue(float v) => -v;
        public override float MultiplyValueBy(float v, float number) => v * number;
        public override float DivideValueBy(float v, float number) => v / number;
    }

    [Serializable]
    public class SpringVector2 : SpringValue<Vector2> {
        public override Vector2 AddValues(Vector2 a, Vector2 b) => a + b;
        public override Vector2 SubtractValues(Vector2 a, Vector2 b) => a - b;
        public override Vector2 InvertValue(Vector2 v) => -v;
        public override Vector2 MultiplyValueBy(Vector2 v, float number) => v * number;
        public override Vector2 DivideValueBy(Vector2 v, float number) => v / number;
    }

    [Serializable]
    public class SpringVector3 : SpringValue<Vector3> {
        public override Vector3 AddValues(Vector3 a, Vector3 b) => a + b;
        public override Vector3 SubtractValues(Vector3 a, Vector3 b) => a - b;
        public override Vector3 InvertValue(Vector3 v) => -v;
        public override Vector3 MultiplyValueBy(Vector3 v, float number) => v * number;
        public override Vector3 DivideValueBy(Vector3 v, float number) => v / number;
    }

    [Serializable]
    public class SpringColor : SpringValue<Color> {
        public override Color AddValues(Color a, Color b) => a + b;
        public override Color SubtractValues(Color a, Color b) => a - b;
        public override Color InvertValue(Color v) => new(-v.r, -v.g, -v.b, -v.a);
        public override Color MultiplyValueBy(Color v, float number) => v * number;
        public override Color DivideValueBy(Color v, float number) => v / number;
    }

}
