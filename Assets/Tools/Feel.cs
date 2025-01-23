using UnityEngine;
using MoreMountains.Feedbacks;
using System;

namespace Clubhouse.Tools
{
    public static class Feel
    {
        #region Core
        public static bool IsNull(MMF_Player a_feedbackPlayer)
        {
            bool isNull = a_feedbackPlayer == null;
            if (isNull) Debug.LogError("Feedback Player is null");
            return isNull;
        }

        public static bool IsNull(MMF_Feedback a_feedback)
        {
            bool isNull = a_feedback == null;
            if (isNull) Debug.LogError("Feedback is null");
            return isNull;
        }

        public static T GetFeedback<T>(MMF_Player a_feedbackPlayer) where T : MMF_Feedback
        {
            if (IsNull(a_feedbackPlayer)) return null;
            return a_feedbackPlayer.GetFeedbackOfType<T>(MMF_Player.AccessMethods.First, 0);
        }

        public static T GetFeedback<T>(MMF_Player a_feedbackPlayer, int a_index) where T : MMF_Feedback
        {
            if (IsNull(a_feedbackPlayer)) return null;
            if (a_feedbackPlayer.FeedbacksList[a_index] is not T)
            {
                Debug.LogError($"Feedback is not of type {typeof(T).Name}");
                return null;
            }
            return (T)a_feedbackPlayer.FeedbacksList[a_index];
        }

        public static void PlayAllFeedbacks(MMF_Player a_feedbackPlayer)
        {
            if (IsNull(a_feedbackPlayer)) return;
            a_feedbackPlayer?.PlayFeedbacks();
        }

        public static void Play(MMF_Feedback a_feedback, Vector3 a_position = default, float a_feedbacksIntensity = 1f)
        {
            if (IsNull(a_feedback)) return;
            a_feedback.Play(a_position, a_feedbacksIntensity);
        }

        public static void Play(MMF_Feedback a_feedback, Action callback, Vector3 a_position = default, float a_feedbacksIntensity = 1f)
        {
            if (IsNull(a_feedback)) return;
            callback?.Invoke();
            a_feedback.Play(a_position, a_feedbacksIntensity);
        }

        public static void Play<T>(MMF_Player a_feedbackPlayer, Vector3 a_position = default, float a_feedbacksIntensity = 1f) where T : MMF_Feedback
        {
            if (IsNull(a_feedbackPlayer)) return;
            T feedback = GetFeedback<T>(a_feedbackPlayer);
            Play(feedback);
        }

        public static void Play<T>(MMF_Player a_feedbackPlayer, Action<T> callback, Vector3 a_position = default, float a_feedbacksIntensity = 1f) where T : MMF_Feedback
        {
            if (IsNull(a_feedbackPlayer)) return;
            T feedback = GetFeedback<T>(a_feedbackPlayer);
            callback?.Invoke(feedback);
        }
        #endregion 

        #region PositionFeedback
        public static void Move(MMF_Player a_feedbackPlayer) => Play<MMF_Position>(a_feedbackPlayer);
        public static void Move(MMF_Player a_feedbackPlayer, Vector3 a_initialPosition, Vector3 a_targetPosition, float a_duration = -1f)
        => Play<MMF_Position>(a_feedbackPlayer, (feedback) =>
        {
            Move(feedback, a_initialPosition, a_targetPosition, a_duration);
        });

        public static void Move(MMF_Position a_feedback, Vector3 a_initialPosition, Vector3 a_targetPosition, float a_duration = -1f)
        => Play(a_feedback, () =>
        {
            a_feedback.InitialPosition = a_initialPosition;
            a_feedback.DestinationPosition = a_targetPosition;
            if (a_duration >= 0f)
                a_feedback.AnimatePositionDuration = a_duration;
        });
        #endregion

        #region ScaleFeedback
        public static void Scale(MMF_Player a_feedbackPlayer) => Play<MMF_Scale>(a_feedbackPlayer);
        public static void Scale(MMF_Player a_feedbackPlayer, float a_duration) => Play<MMF_Scale>(a_feedbackPlayer, (feedback) =>
        {
            Scale(feedback, a_duration);
        });

        public static void Scale(MMF_Scale a_feedback, float a_duration) => Play(a_feedback, () =>
        {
            a_feedback.AnimateScaleDuration = a_duration;
        });
        #endregion

        #region RotationShake
        public static void RotationShake(MMF_Player a_feedbackPlayer) => Play<MMF_RotationShake>(a_feedbackPlayer);
        #endregion
    }
}
