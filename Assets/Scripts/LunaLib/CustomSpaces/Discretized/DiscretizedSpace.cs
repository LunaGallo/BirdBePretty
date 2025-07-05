using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LunaLib {
    public abstract class DiscretizedSpace<T, D> : CustomSpace<T> {

        public abstract D CustomToDiscretizedPoint(T customPoint);
        public abstract T DiscretizedToCustomPoint(D discretizedPoint);

        public D LocalToDiscretizedPoint(Vector3 localPoint) => CustomToDiscretizedPoint(LocalToCustomPoint(localPoint));
        public Vector3 DiscretizedToLocalPoint(D discretizedPoint) => CustomToLocalPoint(DiscretizedToCustomPoint(discretizedPoint));
        public D WorldToDiscretizedPoint(Vector3 worldPoint) => LocalToDiscretizedPoint(WorldToLocalPoint(worldPoint));
        public Vector3 DiscretizedToWorldPoint(D discretizedPoint) => LocalToWorldPoint(DiscretizedToLocalPoint(discretizedPoint));
        public Vector3 WorldToTileCenter(Vector3 worldPoint) => DiscretizedToWorldPoint(WorldToDiscretizedPoint(worldPoint));


        public override void DrawSpaceGizmo() {
            foreach (D d in GetRepresentativeDiscretizedPoints()) {
                DrawDiscretizedGizmo(d);
            }
        }
        public abstract void DrawDiscretizedGizmo(D discretizedPoint, float scale = 1f);
        public abstract List<D> GetRepresentativeDiscretizedPoints();

    }
}