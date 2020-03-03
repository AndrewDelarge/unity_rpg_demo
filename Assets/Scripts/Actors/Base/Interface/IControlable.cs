using GameInput;
using UnityEngine;

namespace Actors.Base.Interface
{
    public interface IControlable
    {
        void Init(Stats actorStats, BaseInput input);
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