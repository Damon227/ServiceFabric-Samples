// ***********************************************************************
// Solution         : ServiceFabricLearning
// Project          : Credit.Kolibre.Foundation.ServiceFabric.Actors
// File             : StatefulActor.cs
// Created          : 2017-01-18  7:13 PM
// ***********************************************************************
// <copyright>
//     Copyright © 2016 Kolibre Credit Team. All rights reserved.
// </copyright>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Actors;
using Microsoft.ServiceFabric.Actors.Runtime;
using Microsoft.ServiceFabric.Data;

namespace Credit.Kolibre.Foundation.ServiceFabric.Actors
{
    public class StatefulActor<TActorState> : KolibreActor where TActorState : IActorState, new()
    {
        private readonly Dictionary<string, StatePropertyMetadata> _statePropertyMetadata;
        private readonly Dictionary<string, object> _originalStateProperties;

        protected StatefulActor(ActorService actorService, ActorId actorId) : base(actorService, actorId)
        {
            State = new TActorState();

            _statePropertyMetadata = new Dictionary<string, StatePropertyMetadata>();
            _originalStateProperties = new Dictionary<string, object>();

            InitializeActorStatePropertyMetadata();
        }

        protected TActorState State { get; }

        [SuppressMessage("ReSharper", "ConvertIfStatementToConditionalTernaryExpression")]
        protected virtual async Task InitializeStateAsync()
        {
            _originalStateProperties.Clear();

            foreach (StatePropertyMetadata metadata in _statePropertyMetadata.Values)
            {
                ConditionalValue<object> stateProperty = await StateManager.TryGetStateAsync<object>(metadata.PropertyName);
                object propertyValue = stateProperty.HasValue ? stateProperty.Value : metadata.DefaultValue;
                metadata.PropertyInfo.SetValue(State, propertyValue);
                _originalStateProperties.Add(metadata.PropertyName, propertyValue);
            }
        }

        protected override async Task OnActivateAsync()
        {
            await InitializeStateAsync();

            await base.OnActivateAsync();
        }

        protected override async Task OnPostActorMethodAsync(ActorMethodContext actorMethodContext)
        {
            await SaveStateAsync();

            await base.OnPostActorMethodAsync(actorMethodContext);
        }

        protected virtual async Task SaveStateAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            Dictionary<string, object> changedStateProperties = new Dictionary<string, object>();

            foreach (StatePropertyMetadata metadata in _statePropertyMetadata.Values)
            {
                cancellationToken.ThrowIfCancellationRequested();

                object currentValue = metadata.PropertyInfo.GetValue(State);
                object originalValue = _originalStateProperties[metadata.PropertyName];

                if (!currentValue.Equals(originalValue))
                {
                    await StateManager.AddOrUpdateStateAsync(metadata.PropertyName, currentValue, (s, o) => currentValue, cancellationToken);
                    changedStateProperties[metadata.PropertyName] = currentValue;
                }
            }

            await base.SaveStateAsync();

            foreach (KeyValuePair<string, object> changedStateProperty in changedStateProperties)
            {
                _originalStateProperties[changedStateProperty.Key] = changedStateProperty.Value;
            }
        }

        private static string GetStatePropertyName(PropertyInfo propertyInfo)
        {
            object attribute = propertyInfo.GetCustomAttributes(typeof(StateNameAttribute), false).FirstOrDefault();

            if (attribute == null)
            {
                return propertyInfo.Name;
            }

            return ((StateNameAttribute)attribute).Name;
        }

        private void InitializeActorStatePropertyMetadata()
        {
            Type type = State.GetType();
            PropertyInfo[] propertyInfos = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).Where(p => p.CanRead && p.CanWrite).ToArray();
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                object defaultValue = null;
                if (propertyInfo.PropertyType.IsValueType)
                {
                    defaultValue = Activator.CreateInstance(propertyInfo.PropertyType);
                }

                string propertyName = GetStatePropertyName(propertyInfo);
                _statePropertyMetadata.Add(propertyName, new StatePropertyMetadata { DefaultValue = defaultValue, PropertyInfo = propertyInfo, PropertyName = propertyName });
            }
        }

        #region Nested type: StatePropertyMetadata

        private sealed class StatePropertyMetadata
        {
            public string PropertyName { get; set; }

            public PropertyInfo PropertyInfo { get; set; }

            public object DefaultValue { get; set; }
        }

        #endregion
    }
}