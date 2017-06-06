﻿using System;
using Microsoft.SharePoint.Client.WorkflowServices;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class WorkflowInstancePipeBind
    {
        private readonly WorkflowInstance _instance;
        private readonly Guid _id;

        public WorkflowInstancePipeBind()
        {
            _instance = null;
            _id = Guid.Empty;
        }

        public WorkflowInstancePipeBind(WorkflowInstance instance)
        {
            _instance = instance;
        }

        public WorkflowInstancePipeBind(Guid guid)
        {
            _id = guid;
        }

        public WorkflowInstancePipeBind(string id)
        {
            _id = Guid.Parse(id);
        }

        public Guid Id => _id;

        public WorkflowInstance Instance => _instance;
    }
}
