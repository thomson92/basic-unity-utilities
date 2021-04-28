using UnityEngine;

namespace UnityUtilities.GeneralHelpers
{
    public static class TransformHelper
    {
        public static void AdjustRotation(Transform target, Transform source)
        {
            if(!target.Equals(source))
            {
                Rotate(target.position, source);
            }
        }

        private static void Rotate(Vector3 targetPos, Transform transform)
        {
            // add  * Time.deltaTime to rotSpeed if want smooth transition but
            // in that case this method should be called in some update hook
            // and now its called in single cast/attack method per animation
            const float rotSpeed = 1;

            var direction = (targetPos - transform.position).normalized;
            var actualRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, actualRotation, rotSpeed);
        }
    }
}
