﻿//-----------------------------------------------------------------------
// <copyright file="Gesture.cs" company="Google">
//
// Copyright 2018 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

// Modifications copyright © 2020 Unity Technologies ApS

#if AR_FOUNDATION_PRESENT || PACKAGE_DOCS_GENERATION

using System;

namespace UnityEngine.XR.Interaction.Toolkit.AR
{
    /// <summary>
    /// A Gesture represents a sequence of touch events that are detected to
    /// represent a particular type of motion (e.g. Dragging, Pinching).
    /// </summary>
    /// <typeparam name="T">The actual gesture.</typeparam>
    /// <remarks>
    /// Gestures are created and updated by instances of <see cref="GestureRecognizer{T}"/>.
    /// </remarks>
    public abstract class Gesture<T> where T : Gesture<T>
    {
        /// <summary>
        /// Initializes and returns an instance of <see cref="Gesture"/> with a given recognizer.
        /// </summary>
        /// <param name="recognizer">The gesture recognizer.</param>
        internal Gesture(GestureRecognizer<T> recognizer)
        {
            this.recognizer = recognizer;
        }

        /// <summary>
        /// Calls the methods in its invocation list when a gesture is started.
        /// </summary>
        public event Action<T> onStart;

        /// <summary>
        /// Calls the methods in its invocation list when a gesture is successfully updated.
        /// </summary>
        public event Action<T> onUpdated;

        /// <summary>
        /// Calls the methods in its invocation list when a gesture is finished.
        /// </summary>
        public event Action<T> onFinished;

        /// <summary>
        /// (Read Only) A boolean value indicating whether the gesture was canceled.
        /// </summary>
        public bool isCanceled { get; private set; }

        /// <summary>
        /// (Read Only) The GameObject this gesture is targeting.
        /// </summary>
        public GameObject targetObject { get; protected set; }

        /// <summary>
        /// (Read Only) The gesture recognizer.
        /// </summary>
        protected internal GestureRecognizer<T> recognizer { get; }

#pragma warning disable IDE1006 // Naming Styles
        /// <inheritdoc cref="isCanceled"/>
        [Obsolete("WasCancelled has been deprecated. Use isCanceled instead. (UnityUpgradable) -> isCanceled")]
        public bool WasCancelled => isCanceled;

        /// <inheritdoc cref="targetObject"/>
        [Obsolete("TargetObject has been deprecated. Use targetObject instead. (UnityUpgradable) -> targetObject")]
        public GameObject TargetObject
        {
            get => targetObject;
            protected set => targetObject = value;
        }

        /// <inheritdoc cref="recognizer"/>
        [Obsolete("m_Recognizer has been deprecated. Use recognizer instead. (UnityUpgradable) -> recognizer")]
        // ReSharper disable once InconsistentNaming -- Deprecated
        protected internal GestureRecognizer<T> m_Recognizer => recognizer;
#pragma warning restore IDE1006 // Naming Styles

        bool m_HasStarted;

        /// <summary>
        /// Updates this gesture.
        /// </summary>
        internal void Update()
        {
            if (!m_HasStarted && CanStart())
            {
                Start();
                return;
            }

            if (m_HasStarted)
            {
                if (UpdateGesture())
                {
                    onUpdated?.Invoke(this as T);
                }
            }
        }

        /// <summary>
        /// Cancels this gesture.
        /// </summary>
        internal void Cancel()
        {
            isCanceled = true;
            OnCancel();
            Complete();
        }

        /// <summary>
        /// Completes this gesture.
        /// </summary>
        protected internal void Complete()
        {
            OnFinish();
            onFinished?.Invoke(this as T);
        }

        /// <summary>
        /// Starts this gesture.
        /// </summary>
        void Start()
        {
            m_HasStarted = true;
            OnStart();
            onStart?.Invoke(this as T);
        }

        /// <summary>
        /// Returns true if this gesture can start.
        /// </summary>
        /// <returns>Returns <see langword="true"/> if the gesture can start. Otherwise, returns <see langword="false"/>.</returns>
        protected internal abstract bool CanStart();

        /// <summary>
        /// This method is called automatically when this gesture is started.
        /// </summary>
        protected internal abstract void OnStart();

        /// <summary>
        /// Updates this gesture.
        /// </summary>
        /// <returns>Returns <see langword="true"/> if the update was successful. Otherwise, returns <see langword="false"/>.</returns>
        protected internal abstract bool UpdateGesture();

        /// <summary>
        /// This method is called automatically when this gesture is canceled.
        /// </summary>
        /// <remarks>
        /// When canceled, this method is called right before <see cref="OnFinish"/>, which is still invoked.
        /// </remarks>
        protected internal abstract void OnCancel();

        /// <summary>
        /// This method is called automatically when this gesture is finished.
        /// </summary>
        protected internal abstract void OnFinish();
    }
}

#endif
