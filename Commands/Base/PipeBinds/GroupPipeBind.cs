﻿using Microsoft.SharePoint.Client;

namespace SharePointPnP.PowerShell.Commands.Base.PipeBinds
{
    public sealed class GroupPipeBind
    {
        private readonly int _id = -1;
        private readonly Group _group;
        private readonly string _name;
        public int Id => _id;

        public Group Group => _group;

        public string Name => _name;

        internal GroupPipeBind()
        {
        }

        public GroupPipeBind(int id)
        {
            _id = id;
        }

        public GroupPipeBind(Group group)
        {
            _group = group;
        }

        public GroupPipeBind(string name)
        {
            _name = name;
        }

        internal Group GetGroup(Web web)
        {
            Group group = null;
            if (Id != -1)
            {
                group = web.SiteGroups.GetById(Id);
            }
            else if (!string.IsNullOrEmpty(Name))
            {
                group = web.SiteGroups.GetByName(Name);
            }
            else if (Group != null)
            {
                group = Group;
            }

            web.Context.Load(group);
            web.Context.Load(group.Users);
            web.Context.ExecuteQueryRetry();
            return group;
        }
    }
}
