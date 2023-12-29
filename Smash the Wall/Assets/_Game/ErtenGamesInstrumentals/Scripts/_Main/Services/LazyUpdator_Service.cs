using Helpers;
using Interfaces;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Services
{
    public class LazyUpdator_Service : IInitializable<float>
    {
        [ShowInInspector] private List<Func<Task>> _tasks = new List<Func<Task>>();

        [ShowInInspector] private int _tasksCount => _tasks.Count;
        [ShowInInspector] private bool _isRunning = false;
        [ShowInInspector] private float _foreachStepDelay = 0;

        public virtual void Initialize(float foreachStepDelay)
        {
            _foreachStepDelay = 0;
        }

        public virtual void AddToQueue(Func<Task> task)
        {
            if (_tasks.Count == 0)
            {
                _isRunning = false;
                DoLazyUpdateWithAwait(Add);
            }
            else Add();

            void Add() { _tasks.SafeAdd(task); }
        }

        public virtual void RemoveFromQueue(Func<Task> task)
        {
            _tasks.SafeRemove(task);
        }

        protected virtual void RemoveAllNullTasks()
        {
            _tasks.RemoveNulls();
        }

        protected virtual async void DoLazyUpdateWithAwait(Action doBeforeUpdate = null)
        {
            if (_isRunning) return;

            doBeforeUpdate?.Invoke();

            try
            {
                while (_tasks.Count > 0 && Application.isPlaying)
                {
                    _isRunning = true;

                    RemoveAllNullTasks();

                    var actions = _tasks.ToArray();
                    foreach (Func<Task> action in actions)
                    {
                        await AsyncHelper.DelayFloat(_foreachStepDelay);

                        try
                        {
                            if (action == null)
                            {
                                _tasks.Remove(action);
                            }
                            else
                            {
                                await action.Invoke();
                            }
                        }
                        catch { }
                    }
                }
            }
            finally
            {
                _isRunning = false;
            }
        }
    }
}