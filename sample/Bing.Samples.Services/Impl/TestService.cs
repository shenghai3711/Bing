﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bing.Caching;
using Bing.Caching.Aspects;
using Bing.Events;
using Bing.Events.Handlers;
using Bing.Helpers;
using Bing.Logs;
using Bing.Logs.Aspects;
using Bing.Logs.Extensions;
using Bing.Samples.Domains.Models;
using Bing.Samples.Services.Events;
using Bing.Samples.Services.Messages;

namespace Bing.Samples.Services.Impl
{
    public class TestService:ITestService
    {
        private IEventBus _eventBus;

        protected ILog Logger = Log.GetLog(typeof(TestService));

        public TestService(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        [CachingHandler]
        public string GetContent(string content)
        {
            return content;
        }
        
        public void WriteOtherLog(string content)
        {
            Console.WriteLine(content);
        }

        public List<ItemResult> GetItems()
        {
            var provider = Ioc.Create<ICacheProvider>();
            var result=provider.Get("IDropdownService:GetRegionList",typeof(List<ItemResult>));
            if (result.HasValue)
            {
                return result.Value as List<ItemResult>;
            }
            return null;
        }

        public void PublishEvent(string name)
        {
            _eventBus.Publish(new TestEvent()
            {
                Name = name
            });
        }

        public void PublishMessageEvent(string name)
        {
            _eventBus.Publish(new TestMessageEvent()
            {
                Name = name
            });
        }

        public void WriteCustomerLog(string content)
        {
            Logger.Caption("输出自定义日志").Content(content).Debug();

            throw new NotImplementedException();
        }
    }
}
