﻿using Footage.Dao;
using Footage.Engine;
using Footage.Repository;
using Footage.Service;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Footage
{
    internal class Locator
    {
        private static bool initialized;

        private static IProvider Provider;

        internal static IDispatcher Dispatcher => Provider.Dispatcher;

        internal static void Initialize(IProvider provider)
        {
            Provider = provider;
            initialized = true;
        }

        internal static T Get<T>()
        {
            if (!initialized)
                throw new InvalidOperationException("Footage.Core.Initialize muse be called before using!");

            if (typeof(T) == typeof(IMediaPlayer))
                throw new ArgumentException("IMediaPlayerService cannot be accessed as singleton. Use Create<T> instead.");

            if (typeof(T) == typeof(IDialogService))
                return (T) Provider.DialogService;

            return SimpleIoc.Default.GetInstance<T>();
        }

        internal static T Create<T>()
        {
            if (!initialized)
                throw new InvalidOperationException("Footage.Core.Initialize muse be called before using!");

            if (typeof(T) == typeof(IMediaPlayer))
                return (T)Provider.CreateMediaPlayer();

            return SimpleIoc.Default.GetInstanceWithoutCaching<T>();
        }
    }
}
