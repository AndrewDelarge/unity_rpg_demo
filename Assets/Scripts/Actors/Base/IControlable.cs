using UnityEngine;

namespace Actors.Base
{
    public interface IControlable
    {
        float GetSpeed();

        float GetCurrentMagnitude();

        void Move(Vector3 direction);

        void MoveTo(Vector3 point);

        void Follow(Vector3 newTarget, float stoppingDistance = 1f);

        void StopFollow();

        void Stop();

        void FaceTarget(Vector3 target);

    }
}