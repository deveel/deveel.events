﻿//
// Copyright (c) Antonello Provenzano and other contributors. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for details.
//

using CloudNative.CloudEvents;

using System.Text.Json;

namespace Deveel.Events
{
    /// <summary>
    /// Extensions for the <see cref="IEventCreator"/> interface.
    /// </summary>
    public static class EventCreatorExtensions
    {
        /// <summary>
        /// Creates a <see cref="CloudEvent"/> from the given data object.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the data object.
        /// </typeparam>
        /// <param name="creator">
        /// The instance of the <see cref="IEventCreator"/> that is used to 
        /// create the event from the data.
        /// </param>
        /// <param name="data">
        /// The data object that is transported by the event.
        /// </param>
        /// <returns>
        /// Returns a <see cref="CloudEvent"/> instance that is
        /// created from the given data object.
        /// </returns>
        public static CloudEvent CreateEventFromData<T>(this IEventCreator creator, T data)
            => creator.CreateEventFromData(typeof(T), data);
    }
}
